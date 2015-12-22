using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using OpenHTM.CLA;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Region = OpenHTM.CLA.Region;

namespace OpenHTM.IDE
{
	// notify listeners of selection changes 
	public delegate void StateInformationPanelSelectionChanged_Event(object sender, EventArgs e, object obj);

	public class StateInformationPanel : Panel
	{

		public static event StateInformationPanelSelectionChanged_Event StateInformationPanel_SelectionChanged = delegate { };

		#region Fields

		// Region which is being viewed
		private Region _region;

		#region Zoom

		// Size of the current view.
		// It can increase or decrease depending zoom.
		private SizeF _sizeOfView;

		// Viewing starting position in order to center on the zooming.
		private PointF _startingViewPoint;

		// Zoom restrains
		private static readonly SizeF _sizeColumnInVirtual = new SizeF(1.0f, 1.0f);

		/// <summary>
		/// At maximum scale.
		/// </summary>
		private readonly float _smallestZoomSizeInVirtual = _sizeColumnInVirtual.Width / 4;

		/// <summary>
		/// At minimum scale.
		/// </summary>
		private readonly float _largestZoomSizeInVirtual = _sizeColumnInVirtual.Width * 100;

		#endregion

		#region Mouse

		// Last mouse coordinates.
		private Point _lastMouseLocation;

		// Indicates that last mouse coordinates was stored.
		private bool _lastMouseLocationWasSet;

		// Mouse coordinates in the world
		private PointF _relativeMouseLocationInVirtualWorld;

		// Indicates that mouse is being pressed at moment
		private bool _pressingTheMouse;

		#endregion

		#region Keyboard

		// Indicate that 'control' and 'alt' keys are pressed at moment
		private bool _keyControlIsPressed;
		private bool _keyAltIsPressed;

		#endregion

		#region Graphics

		// Image representing state region at current mode.
		private Bitmap _graphicsBitmap;

		// Ramdom colors dictionary for representing segments.
		private List<Color> _segmentColors = new List<Color>();

		// Number of cells per x,y axis
		private Size _numberCellsInColumn;

		// Relative size of each cell in a column
		private SizeF _sizeCellInColumn;

		#endregion

		#region Selection

		// Selected objects by the user
		private List<object> _selectedEntities = new List<object>();

		// Object which mouse hovers on it
		private object _mouseHoversEntity;

		#endregion

		#region View modes

		// Current view mode
		public Mode _viewerMode = Mode.Regular;

		public bool ViewActiveCells = true;
		public bool ViewPredictingCells = true;
		public bool ViewActiveAndPredictedCells = true;
		public bool ViewNonActiveAndPredictedCells = true;
		public bool ViewCellSegmentUpdates = false;
		public bool ViewRegionalIncreasedPermanenceSynapses = false;
		public bool ViewRegionalDecreasedPermanenceSynapses = false;
		public bool ViewRegionalNewSynapses = false;
		public bool ViewRegionalRemovedSynapses = false;

		#endregion

		#region State information

		// States for synapses
		private Hashtable _lastTimestepSynapses = new Hashtable();
		private List<DistalSynapseChange> _synapsesChanges = new List<DistalSynapseChange>();

		#endregion

		#region Notification

		// Status messages
		private string _notifyText;
		private int _framesToDisplayNotification;
		private Font _notificationFont = new Font("Arial", 20);

		#endregion

		#endregion

		#region Nested classes

		public enum Mode
		{
			Regular,
			FeedFowardInput,
			FeedFowardInputReconstruction,
			PredictionReconstruction,
			Boosting,
			ColumnsSideView
		};

		private class DistalSynapseComparisonCopy
		{
			public Cell OutputCell;
			public DistalSynapse OriginalSynapse;
			public DistalSynapse SavedSynapse;

			public DistalSynapseComparisonCopy(Cell outputCell,
											   DistalSynapse originalSynapse, DistalSynapse savedSynapse)
			{
				this.OutputCell = outputCell;
				this.OriginalSynapse = originalSynapse;
				this.SavedSynapse = savedSynapse;
			}
		}

		private enum EDistalSynapseChange
		{
			Added,
			Removed,
			PermanenceIncresead,
			PermanenceDecreased
		};

		private class DistalSynapseChange
		{
			public DistalSynapse Synapse;
			public Cell OutputCell;
			public EDistalSynapseChange ChangeType;
			public string ChangeText;

			public DistalSynapseChange(DistalSynapse synapse, Cell outputCell,
									   EDistalSynapseChange change, string changeText)
			{
				this.Synapse = synapse;
				this.OutputCell = outputCell;
				this.ChangeType = change;
				this.ChangeText = changeText;
			}
		};

		#endregion

		#region Constructor

		public StateInformationPanel()
		{
			// Checks if we are using it when prediction steps are > 1.
			if (DistalSegment.MaxTimeSteps > 1)
			{
				throw new Exception("Cant use this visualizer when Segment.MaxTimeSteps > 1");
			}

			// Set fields
			this._graphicsBitmap = new Bitmap(this.Width, this.Height);
			this._startingViewPoint = new PointF(0, 0);
			this._sizeOfView = new SizeF(20, 20);
			this._lastMouseLocationWasSet = false;

			// Set events
			//this.MouseWheel += this.Panel_MouseWheel;  // Won't work.
			this.MouseDown += this.Panel_MouseDown;
			this.MouseUp += this.Panel_MouseUp;
			this.MouseMove += this.Panel_MouseMove;
			this.Resize += this.Panel_Resize;
			this.KeyUp += this.Panel_KeyUp;
			this.KeyDown += this.Panel_KeyDown;


			// Create color dictionary
			this.CreateSegmentColorDictionary();


		}

		#endregion

		#region Methods

		public void RefreshControls()
		{
			if (NetControllerForm.Instance.SelectedNode.Region != null)
			{
				this._region = NetControllerForm.Instance.SelectedNode.Region;

				// Calculate how much cells we have for the width and height of a column.
				var cellsInColumnSqrt = (float)Math.Sqrt(this._region.CellsPerColumn);
				this._numberCellsInColumn.Width = (int)cellsInColumnSqrt;
				this._numberCellsInColumn.Height = (int)cellsInColumnSqrt;
				if (Math.Truncate(cellsInColumnSqrt) != cellsInColumnSqrt)
				{
					this._numberCellsInColumn.Width += 1;
				}
				this._sizeCellInColumn.Width = 1.0f / this._numberCellsInColumn.Width;
				this._sizeCellInColumn.Height = 1.0f / this._numberCellsInColumn.Height;

				// Clear all the synapses changes.
				this._synapsesChanges.Clear();

				// Analyze the synapses differences versus "last timestep synapses".
				// first, scan the current synapses in a hashtable form.
				var currentSynapses = new Hashtable();
				foreach (var column in this._region.Columns)
				{
					foreach (var cell in column.Cells)
					{
						foreach (var segment in cell.DistalSegments)
						{
							foreach (DistalSynapse synapse in segment.Synapses)
							{
								currentSynapses[synapse] = new DistalSynapseComparisonCopy(
									cell, synapse,
									new DistalSynapse(segment, synapse.InputSource, synapse.Permanence));
							}
						}
					}
				}

				// Start scanning all the existing synapses.
				foreach (DistalSynapseComparisonCopy existingDistalSynapse in currentSynapses.Values)
				{
					// Does such "old synapse" exist?
					// If not, then this is a new synapse. mark it as new and continue.
					if (this._lastTimestepSynapses.Contains(existingDistalSynapse.OriginalSynapse) == false)
					{
						this._synapsesChanges.Add(new DistalSynapseChange(
													  existingDistalSynapse.OriginalSynapse, existingDistalSynapse.OutputCell,
													  EDistalSynapseChange.Added, "New"));
						continue;
					}

					// If we reached here, then the old synapse exists.
					var oldSynapse =
						(DistalSynapseComparisonCopy)this._lastTimestepSynapses[
																				 existingDistalSynapse.OriginalSynapse];
					// Does the synapse permanence is different or the same?
					if (existingDistalSynapse.OriginalSynapse.Permanence <
						oldSynapse.SavedSynapse.Permanence)
					{
						this._synapsesChanges.Add(new DistalSynapseChange(
													  existingDistalSynapse.OriginalSynapse, existingDistalSynapse.OutputCell,
													  EDistalSynapseChange.PermanenceDecreased,
													  oldSynapse.SavedSynapse.Permanence +
													  " => " + existingDistalSynapse.OriginalSynapse.Permanence));
					}
					else if (existingDistalSynapse.OriginalSynapse.Permanence >
							 oldSynapse.SavedSynapse.Permanence)
					{
						this._synapsesChanges.Add(new DistalSynapseChange(
													  existingDistalSynapse.OriginalSynapse, existingDistalSynapse.OutputCell,
													  EDistalSynapseChange.PermanenceIncresead,
													  oldSynapse.SavedSynapse.Permanence +
													  " => " + existingDistalSynapse.OriginalSynapse.Permanence));
					}
				}

				// Now start scanning for the old synapses in the existing synapses to see which
				// Were removed.
				foreach (DistalSynapseComparisonCopy oldSynapse in this._lastTimestepSynapses.Values)
				{
					if (currentSynapses.Contains(oldSynapse.OriginalSynapse) == false)
					{
						this._synapsesChanges.Add(new DistalSynapseChange(
													  oldSynapse.OriginalSynapse, oldSynapse.OutputCell,
													  EDistalSynapseChange.Removed, "Removed"));
					}
				}

				// Clone the segments to the "last timestep synapses" 
				// After wev'e analyzed the differences.
				this._lastTimestepSynapses.Clear();
				this._lastTimestepSynapses = currentSynapses;

				this.Display();
			}
		}

		public Size GetSizeOnDisplay(SizeF original)
		{
			var sizeOnDisplay = new Size(
				(int)(original.Width / this._sizeOfView.Width * this._graphicsBitmap.Width),
				(int)(original.Height / this._sizeOfView.Height * this._graphicsBitmap.Height));
			return sizeOnDisplay;
		}

		public void GetCellVirtualPointAndSize(Cell cell, out PointF absolutePositionCell, out SizeF sizeCell)
		{
			// Relative cell position in column
			var relativePositionCell = new PointF();
			relativePositionCell.Y = cell.Index / this._numberCellsInColumn.Width;
			relativePositionCell.X = cell.Index % this._numberCellsInColumn.Width;

			// Absolute cell position in column
			absolutePositionCell = new PointF();
			absolutePositionCell.X = cell.Column.PositionInRegion.X +
									 relativePositionCell.X * this._sizeCellInColumn.Width;
			absolutePositionCell.Y = cell.Column.PositionInRegion.Y +
									 relativePositionCell.Y * this._sizeCellInColumn.Height;
			sizeCell = new SizeF(this._sizeCellInColumn.Width, this._sizeCellInColumn.Height);

			// If the viewer mode is sideview of the cells,
			// Calculate the position and size differently.
			if (this._viewerMode == Mode.ColumnsSideView)
			{
				PointF positionColumn;
				SizeF sizeColumn;
				this.GetColumnVirtualPointAndSize(cell.Column, out positionColumn, out sizeColumn);
				absolutePositionCell = positionColumn;
				absolutePositionCell.Y = _sizeColumnInVirtual.Height * cell.Index;
				sizeCell = _sizeColumnInVirtual;
			}
		}

		public void GetColumnVirtualPointAndSize(Column column, out PointF columnPoint, out SizeF size)
		{
			columnPoint = new PointF(column.PositionInRegion.X, column.PositionInRegion.Y);
			size = _sizeColumnInVirtual;

			// If the viewer mode is sideview of the columns,
			// Calculate the position and size differently.
			if (this._viewerMode == Mode.ColumnsSideView)
			{
				columnPoint = new PointF(
					column.PositionInRegion.X * this._region.Size.Height + column.PositionInRegion.Y, 0);
				size = new SizeF(_sizeColumnInVirtual.Width,
								 _sizeColumnInVirtual.Height * this._region.CellsPerColumn);
			}
		}

		public void GetColumnDisplayPointAndSize(Column column, out Point displayPoint, out Size displaySize)
		{
			PointF columnLocation;
			SizeF columnSize;
			this.GetColumnVirtualPointAndSize(column, out columnLocation, out columnSize);

			displayPoint = this.ConvertViewPointToDisplayPoint(columnLocation);
			displaySize = this.GetSizeOnDisplay(columnSize);
		}

		public void GetCellDisplayPointAndSize(Cell cell, out Point displayPoint, out Size displaySize)
		{
			PointF cellLocation;
			SizeF cellSize;
			this.GetCellVirtualPointAndSize(cell, out cellLocation, out cellSize);

			displayPoint = this.ConvertViewPointToDisplayPoint(cellLocation);
			displaySize = this.GetSizeOnDisplay(cellSize);
		}

		public void DrawVirtualString(string drawString, Graphics grpOnBitmap,
									  PointF position, SizeF size, Color color)
		{
			// If the string is empty, return.
			if (drawString == string.Empty)
			{
				return;
			}

			// If the requested size is too small, then don't display.
			Size requestedActualSize = this.GetSizeOnDisplay(size);
			if ((requestedActualSize.Width < 20) || (requestedActualSize.Height < 20))
			{
				return;
			}

			// Find what is the best font size to fill the requested display size.
			int advancingFontSize = 5;
			int bestFontSize = advancingFontSize;
			var drawFont = new Font("Arial", advancingFontSize);
			SizeF filledAdvancingDisplaySize = grpOnBitmap.MeasureString(drawString, drawFont);
			SizeF filledBestSize = filledAdvancingDisplaySize;
			drawFont.Dispose();
			while ((filledAdvancingDisplaySize.Width <= requestedActualSize.Width) &&
				   (filledAdvancingDisplaySize.Height <= requestedActualSize.Height))
			{
				bestFontSize = advancingFontSize;
				filledBestSize = filledAdvancingDisplaySize;
				advancingFontSize *= 2;
				drawFont = new Font("Arial", advancingFontSize);
				filledAdvancingDisplaySize = grpOnBitmap.MeasureString(drawString, drawFont);
				drawFont.Dispose();
			}

			// If the best filled size is too big for the requested size, don't draw it.
			if ((filledBestSize.Width > requestedActualSize.Width) ||
				(filledBestSize.Height > requestedActualSize.Height))
			{
				return;
			}

			drawFont = new Font("Arial", bestFontSize);
			var drawBrush =
				new SolidBrush(color);
			var drawFormat = new StringFormat();
			Point displayPoint = this.ConvertViewPointToDisplayPoint(position);

			// Draw as closest as you can to center : recalculate the top most left position based
			// On the text size :
			var centeredDisplayPoint = new Point(
				displayPoint.X + ((requestedActualSize.Width - (int)filledBestSize.Width) / 2),
				displayPoint.Y + ((requestedActualSize.Height - (int)filledBestSize.Height) / 2));

			grpOnBitmap.DrawString(drawString, drawFont, drawBrush,
								   centeredDisplayPoint.X, centeredDisplayPoint.Y, drawFormat);
			drawFont.Dispose();
			drawBrush.Dispose();
		}

		public void DrawColumnRectangle(Column column, Graphics grpOnBitmap, Color color,
			bool drawFill, bool drawOutline)
		{
			Size columnSizeOnDisplay;
			Point columnPoint;
			this.GetColumnDisplayPointAndSize(column, out columnPoint, out columnSizeOnDisplay);

			if (drawFill)
			{
				grpOnBitmap.FillRectangle(new SolidBrush(color),
										  columnPoint.X, columnPoint.Y,
										  columnSizeOnDisplay.Width, columnSizeOnDisplay.Height);
			}

			if (drawOutline)
			{
				grpOnBitmap.DrawRectangle(new Pen(Color.Black, 3),
										  columnPoint.X, columnPoint.Y,
										  columnSizeOnDisplay.Width, columnSizeOnDisplay.Height);
			}
		}

		public void DrawCellRectangle(Cell cell, Graphics grpOnBitmap, Color color,
			bool drawFill, bool drawOutline)
		{
			Point cellPoint;
			Size cellSize;
			this.GetCellDisplayPointAndSize(cell, out cellPoint, out cellSize);

			if (drawFill)
			{
				grpOnBitmap.FillRectangle(new SolidBrush(color),
										  cellPoint.X, cellPoint.Y,
										  cellSize.Width, cellSize.Height);
			}

			if (drawOutline)
			{
				grpOnBitmap.DrawRectangle(new Pen(Color.WhiteSmoke, 1),
										  cellPoint.X, cellPoint.Y,
										  cellSize.Width, cellSize.Height);
			}
		}

		public bool ShowingInternalCells()
		{
			SizeF cellDisplaySize = this.GetSizeOnDisplay(new SizeF(
															  this._sizeCellInColumn.Width, this._sizeCellInColumn.Height));
			return (!(cellDisplaySize.Width < 10)) || (!(cellDisplaySize.Height < 10));
		}

		public void ShowColumns_RegularMode(Graphics grpOnBitmap, Column column)
		{
			Point columnPointOnDisplay;
			Size columnSizeOnDisplay;
			PointF columnPointVirtual;
			SizeF columnSizeVirtual;
			this.GetColumnDisplayPointAndSize(column, out columnPointOnDisplay, out columnSizeOnDisplay);
			this.GetColumnVirtualPointAndSize(column, out columnPointVirtual, out columnSizeVirtual);

			Color columnColor = Color.Tan;

			bool columnWasPredicted = false;
			foreach (var cell in column.Cells)
			{
				if (cell.PredictiveState[Global.T - 1])
				{
					columnWasPredicted = true;
					break;
				}
			}

			bool columnIsPredicting = false;
			foreach (var cell in column.Cells)
			{
				if (cell.PredictiveState[Global.T])
				{
					columnIsPredicting = true;
					break;
				}
			}

			if (column.ActiveState[Global.T] && this.ViewActiveCells)
			{
				columnColor = Color.Yellow;
			}
			if (column.ActiveState[Global.T] && columnWasPredicted &&
				this.ViewActiveAndPredictedCells)
			{
				columnColor = Color.Aqua;
			}
			if ((column.ActiveState[Global.T] == false) && columnWasPredicted &&
				this.ViewNonActiveAndPredictedCells)
			{
				columnColor = Color.Red;
			}
			if (columnIsPredicting && this.ViewPredictingCells)
			{
				columnColor = Color.LimeGreen;
			}

			grpOnBitmap.FillRectangle(new SolidBrush(columnColor),
									  columnPointOnDisplay.X, columnPointOnDisplay.Y,
									  columnSizeOnDisplay.Width, columnSizeOnDisplay.Height);

			// Does the mouse hovers over the column? let's check.
			if (this._keyControlIsPressed)
			{
				if ((this._relativeMouseLocationInVirtualWorld.X >= columnPointVirtual.X) &&
					(this._relativeMouseLocationInVirtualWorld.Y >= columnPointVirtual.Y) &&
					(this._relativeMouseLocationInVirtualWorld.X < (columnPointVirtual.X + columnSizeVirtual.Width)) &&
					(this._relativeMouseLocationInVirtualWorld.Y < (columnPointVirtual.Y + columnSizeVirtual.Height)))
				{
					this._mouseHoversEntity = column;

					// Paint a light over the highlighted cell.
					grpOnBitmap.FillRectangle(new SolidBrush(Color.FromArgb(127, Color.White)),
											  columnPointOnDisplay.X, columnPointOnDisplay.Y,
											  columnSizeOnDisplay.Width, columnSizeOnDisplay.Height);
				}
			}
		}

		public void ShowColumns_InputMode(Graphics grpOnBitmap, Column column,
			float maxOverlap)
		{
			// Display the column overlap and if it's inhibited or not.
			Color columnColor = Color.Black;
			var overlapColor = (byte)((column.Overlap / maxOverlap) * byte.MaxValue);
			string text = string.Empty;

			// Only draw the column if it has some overlap.
			if (column.Overlap > 0)
			{
				if (column.InhibitedState[Global.T])
				{
					columnColor = Color.FromArgb(overlapColor, 0, 0);
					text += "Inhibited : ";
				}
				else
				{
					columnColor = Color.FromArgb(0, overlapColor, 0);
					text += "Active : ";
				}

				text += column.Overlap + " Overlap";

				// Draw the column and add column text above it.
				this.DrawColumnRectangle(column, grpOnBitmap, columnColor, true, true);
				this.DrawVirtualString(text, grpOnBitmap, column.PositionInRegion,
									   _sizeColumnInVirtual, Color.Black);
			}
		}

		public void ShowColumns_BoostingMode(Graphics grpOnBitmap, Column column,
			float minBoosting, float maxBoosting)
		{
			// Only draw the column if it has some boosting.
			if (column.Boost > 0)
			{
				float precentageFromBoostingRange =
					(column.Boost - minBoosting) / (maxBoosting - minBoosting);
				var colorByte = (byte)(precentageFromBoostingRange * byte.MaxValue);
				Color columnColor = Color.FromArgb(colorByte, 255 - colorByte, 255 - colorByte);

				// Draw the column and add column text above it.
				this.DrawColumnRectangle(column, grpOnBitmap, columnColor, true, true);

				// Invert the color of the column in order to show textual information :
				Color invertedColor = Color.FromArgb(byte.MaxValue - columnColor.R,
													 byte.MaxValue - columnColor.G, byte.MaxValue - columnColor.B);

				float proximalSegmentPermanenceSum = 0;
				float averageProximalSegmentPermanence = 0;
				foreach (ProximalSynapse synapse in column.ProximalSegment.Synapses)
				{
					proximalSegmentPermanenceSum += synapse.Permanence;
				}
				averageProximalSegmentPermanence =
					proximalSegmentPermanenceSum / column.ProximalSegment.Synapses.Count;

				string text = "Boosting : " + column.Boost +
							  "\nActiveDutyCycle : " + column.ActiveDutyCycle +
							  "\nOverlapDutyCycle : " + column.OverlapDutyCycle +
							  "\nAverage proximal segment permanence : " + averageProximalSegmentPermanence;

				this.DrawVirtualString(text, grpOnBitmap, column.PositionInRegion,
									   _sizeColumnInVirtual, Color.Black);
			}
		}

		public void ShowInputReconstructionModeRows(Graphics grpOnBitmap)
		{
			float[,] feedForwardInputReconstruction =
				this._region.GetInputReconstructionOfFeedforwardInput();
			float minFeedForwardInput = 0, maxFeedForwardInput = 0;

			// Find max.
			foreach (var amount in feedForwardInputReconstruction)
			{
				if (amount > maxFeedForwardInput)
				{
					maxFeedForwardInput = amount;
				}
			}

			// The min feedforward input must be bigger than 0 (ignore the inputs who received
			// Zero)
			minFeedForwardInput = maxFeedForwardInput;
			
			// Find min.
			foreach (var amount in feedForwardInputReconstruction)
			{
				if ((amount < minFeedForwardInput) && (amount > 0))
				{
					minFeedForwardInput = amount;
				}
			}

			// Draw grid.
			for (int x = 0; x < this._region.InputSize.Width; x++)
			{
				for (int y = 0; y < this._region.InputSize.Height; y++)
				{
					Size sizeOnDisplay = this.GetSizeOnDisplay(_sizeColumnInVirtual);
					Point displayPoint = 
						this.ConvertViewPointToDisplayPoint(new PointF(x, y));
					float reconstructionStrength = feedForwardInputReconstruction[x, y];

					if (reconstructionStrength > 0)
					{
						float percentageFromReconstructionRange =
							(reconstructionStrength - minFeedForwardInput) /
							(maxFeedForwardInput - minFeedForwardInput);
						var colorByte = (byte)(percentageFromReconstructionRange * byte.MaxValue);
						Color color = Color.FromArgb(colorByte, colorByte, colorByte);

						grpOnBitmap.FillRectangle(new SolidBrush(color),
												  displayPoint.X, displayPoint.Y,
												  sizeOnDisplay.Width, sizeOnDisplay.Height);
					}

					grpOnBitmap.DrawRectangle(new Pen(Color.Black, 3),
											  displayPoint.X, displayPoint.Y,
											  sizeOnDisplay.Width, sizeOnDisplay.Height);
				}
			}
		}

		// Note that the prediction is for one step forward.
		public void ShowPredictionReconstructionModeRows(Graphics grpOnBitmap)
		{
			float[,] predictionReconstruction =
				this._region.GetPredictionReconstruction(1);
			float minPrediction = 0, maxPrediction = 0;

			// Find max.
			foreach (var amount in predictionReconstruction)
			{
				if (amount > maxPrediction)
				{
					maxPrediction = amount;
				}
			}

			// The min feedforward input must be bigger than 0 (ignore the inputs who received
			// Zero)
			minPrediction = maxPrediction;

			// Find min.
			foreach (var amount in predictionReconstruction)
			{
				if ((amount < minPrediction) && (amount > 0))
				{
					minPrediction = amount;
				}
			}

			// Draw grid.
			for (int x = 0; x < this._region.InputSize.Width; x++)
			{
				for (int y = 0; y < this._region.InputSize.Height; y++)
				{
					Size sizeOnDisplay = this.GetSizeOnDisplay(_sizeColumnInVirtual);
					Point displayPoint = 
						this.ConvertViewPointToDisplayPoint(new PointF(x, y));
					float reconstructionStrength = predictionReconstruction[x, y];

					if (reconstructionStrength > 0)
					{
						float precentageFromReconstructionRange =
							(reconstructionStrength - minPrediction) /
							(maxPrediction - minPrediction);
						var colorByte = (byte)(precentageFromReconstructionRange * byte.MaxValue);
						Color color = Color.FromArgb(colorByte, colorByte, colorByte);

						grpOnBitmap.FillRectangle(new SolidBrush(color),
												  displayPoint.X, displayPoint.Y,
												  sizeOnDisplay.Width, sizeOnDisplay.Height);
					}

					grpOnBitmap.DrawRectangle(new Pen(Color.Black, 3),
											  displayPoint.X, displayPoint.Y,
											  sizeOnDisplay.Width, sizeOnDisplay.Height);
				}
			}
		}

		public void ShowInternalCells_RegularMode(Graphics grpOnBitmap, Column column)
		{
			// Display the cells in the column
			foreach (var cell in column.Cells)
			{
				Color cellColor = Color.Tan;

				Color colorAverage = Color.Black;
				/*if (cell.IsActive)
					clrCellColor = Color.Yellow;
				if ((cell.WasPredicted == true) && (cell.IsActive == false))
					clrCellColor = Color.Red;
				if (cell.IsPredicting == true)
					clrCellColor = Color.Green;
				if ((cell.WasPredicted == true) && (cell.IsActive == true))
					clrCellColor = Color.Aqua;*/
				if (cell.ActiveState[Global.T] && this.ViewActiveCells)
				{
					if ((colorAverage.R == 0) && (colorAverage.G == 0) && (colorAverage.B == 0))
					{
						colorAverage = Color.Yellow;
					}
					else
					{
						colorAverage = Color.FromArgb((colorAverage.R + Color.Yellow.R) / 2,
													  (colorAverage.G + Color.Yellow.G) / 2,
													  (colorAverage.B + Color.Yellow.B) / 2);
					}
				}
				if (cell.PredictiveState[Global.T - 1] && (cell.ActiveState[Global.T] == false)
					&& this.ViewNonActiveAndPredictedCells)
				{
					if ((colorAverage.R == 0) && (colorAverage.G == 0) && (colorAverage.B == 0))
					{
						colorAverage = Color.Red;
					}
					else
					{
						colorAverage = Color.FromArgb((colorAverage.R + Color.Red.R) / 2,
													  (colorAverage.G + Color.Red.G) / 2,
													  (colorAverage.B + Color.Red.B) / 2);
					}
				}

				if (cell.PredictiveState[Global.T] && this.ViewPredictingCells)
				{
					if ((colorAverage.R == 0) && (colorAverage.G == 0) && (colorAverage.B == 0))
					{
						colorAverage = Color.LimeGreen;
					}
					else
					{
						colorAverage = Color.FromArgb((colorAverage.R + Color.LimeGreen.R) / 2,
													  (colorAverage.G + Color.LimeGreen.G) / 2,
													  (colorAverage.B + Color.LimeGreen.B) / 2);
					}
				}
				if (cell.PredictiveState[Global.T - 1] && cell.ActiveState[Global.T]
					&& this.ViewActiveAndPredictedCells)
				{
					if ((colorAverage.R == 0) && (colorAverage.G == 0) && (colorAverage.B == 0))
					{
						colorAverage = Color.Aqua;
					}
					else
					{
						colorAverage = Color.FromArgb((colorAverage.R + Color.Aqua.R) / 2,
													  (colorAverage.G + Color.Aqua.G) / 2,
													  (colorAverage.B + Color.Aqua.B) / 2);
					}
				}

				if ((colorAverage.R != 0) || (colorAverage.G != 0) || (colorAverage.B != 0))
				{
					cellColor = colorAverage;
				}

				Point cellPoint;
				Size cellSizeOnDisplay;
				PointF cellPointVirtual;
				SizeF cellSizeVirtual;

				this.GetCellVirtualPointAndSize(cell, out cellPointVirtual, out cellSizeVirtual);
				this.GetCellDisplayPointAndSize(cell, out cellPoint, out cellSizeOnDisplay);
				grpOnBitmap.FillRectangle(new SolidBrush(cellColor),
										  cellPoint.X, cellPoint.Y,
										  cellSizeOnDisplay.Width, cellSizeOnDisplay.Height);
				grpOnBitmap.DrawRectangle(new Pen(Color.WhiteSmoke, 1),
										  cellPoint.X, cellPoint.Y,
										  cellSizeOnDisplay.Width, cellSizeOnDisplay.Height);

				// Find out how much segments this cell has. if it has more than one, then
				// Make a gray circle above it.
				if (cell.DistalSegments.Count > 0)
				{
					grpOnBitmap.FillEllipse(new SolidBrush(Color.FromArgb(127, Color.Gray)),
											cellPoint.X, cellPoint.Y,
											cellSizeOnDisplay.Width, cellSizeOnDisplay.Height);

					// Add the number of segments on top.
					this.DrawVirtualString(cell.DistalSegments.Count.ToString(), grpOnBitmap,
										   cellPointVirtual, cellSizeVirtual, Color.LightGray);
				}

				string textToDrawOnCell = string.Empty;
				// If the cell is a previous learning cell, draw "PL" on it.
				if (cell.LearnState[Global.T])
				{
					textToDrawOnCell += "L";
				}

				if (cell.LearnState[Global.T - 1])
				{
					textToDrawOnCell += "PL";
				}

				// Draw the text on the cell.
				this.DrawVirtualString(textToDrawOnCell, grpOnBitmap,
									   cellPointVirtual, cellSizeVirtual,
									   Color.Black);

				// Does the mouse hover over the cell? let's check.
				if (this._keyControlIsPressed)
				{
					if ((this._relativeMouseLocationInVirtualWorld.X >= cellPointVirtual.X) &&
						(this._relativeMouseLocationInVirtualWorld.Y >= cellPointVirtual.Y) &&
						(this._relativeMouseLocationInVirtualWorld.X < (cellPointVirtual.X + cellSizeVirtual.Width)) &&
						(this._relativeMouseLocationInVirtualWorld.Y < (cellPointVirtual.Y + cellSizeVirtual.Height)))
					{
						this._mouseHoversEntity = cell;

						// Paint a light over the highlighted cell.
						grpOnBitmap.FillRectangle(new SolidBrush(Color.FromArgb(127, Color.White)),
												  cellPoint.X, cellPoint.Y,
												  cellSizeOnDisplay.Width, cellSizeOnDisplay.Height);
					}
				}
			}
		}

		public void DisplayCells(Graphics grpOnBitmap)
		{
			// Figure out if we want to display internals cells or just columns.
			bool showingInternalCells = this.ShowingInternalCells();

			// If the mode is "input" compute overlap alpha values.
			float maxOverlap = 0;
			float maxBoosting = 0, minBoosting = 0;

			if (this._viewerMode == Mode.FeedFowardInput)
			{
				foreach (var column in this._region.Columns)
				{
					if (column.Overlap > maxOverlap)
					{
						maxOverlap = column.Overlap;
					}
				}
			}

			if (this._viewerMode == Mode.Boosting)
			{
				foreach (var column in this._region.Columns)
				{
					if (column.Boost > maxBoosting)
					{
						maxBoosting = column.Boost;
					}
				}

				minBoosting = maxBoosting;
				foreach (var column in this._region.Columns)
				{
					if ((column.Boost < minBoosting) && (column.Boost > 0))
					{
						minBoosting = column.Boost;
					}
				}
			}

			// Display columns
			foreach (var column in this._region.Columns)
			{
				Point columnPoint;
				Size columnSizeOnDisplay;
				this.GetColumnDisplayPointAndSize(column, out columnPoint, out columnSizeOnDisplay);

				// A list of conditions in order to speed up rendering and to ignore this column.
				if (columnPoint.X > this._graphicsBitmap.Width)
				{
					continue;
				}
				if (columnPoint.Y > this._graphicsBitmap.Height)
				{
					continue;
				}
				if ((columnPoint.X < 0) && ((columnPoint.X + columnSizeOnDisplay.Width) < 0))
				{
					continue;
				}
				if ((columnPoint.Y < 0) && ((columnPoint.Y + columnSizeOnDisplay.Height) < 0))
				{
					continue;
				}

				if (showingInternalCells)
				{
					if ((this._viewerMode == Mode.Regular) ||
						(this._viewerMode == Mode.ColumnsSideView))
					{
						this.ShowInternalCells_RegularMode(grpOnBitmap, column);
					}
					if (this._viewerMode == Mode.FeedFowardInput)
					{
						this.ShowColumns_InputMode(grpOnBitmap, column, maxOverlap);
					}
					if (this._viewerMode == Mode.Boosting)
					{
						this.ShowColumns_BoostingMode(grpOnBitmap, column, minBoosting, maxBoosting);
					}
				}

				// If we are displaying columns only, then fill the column rectangle
				// With an appropriate color.
				if (showingInternalCells == false)
				{
					if ((this._viewerMode == Mode.Regular) ||
						(this._viewerMode == Mode.ColumnsSideView))
					{
						this.ShowColumns_RegularMode(grpOnBitmap, column);
					}
					if (this._viewerMode == Mode.FeedFowardInput)
					{
						this.ShowColumns_InputMode(grpOnBitmap, column, maxOverlap);
					}
					if (this._viewerMode == Mode.Boosting)
					{
						this.ShowColumns_BoostingMode(grpOnBitmap, column, minBoosting, maxBoosting);
					}
				}

				grpOnBitmap.DrawRectangle(new Pen(Color.Black, 3),
										  columnPoint.X, columnPoint.Y,
										  columnSizeOnDisplay.Width, columnSizeOnDisplay.Height);
			}
		}

		public float GetMaxPermanenceForRegion()
		{
			float maxPermanence = 0;
			foreach (var column in this._region.Columns)
			{
				foreach (var cell in column.Cells)
				{
					foreach (var segment in cell.DistalSegments)
					{
						foreach (var synapse in segment.Synapses)
						{
							if (synapse.Permanence > maxPermanence)
							{
								maxPermanence = synapse.Permanence;
							}
						}
					}
				}
			}

			return maxPermanence;
		}

		/// <summary>
		/// Calculates the synapse color for the selected cells.
		/// </summary>
		/// <param name="synapse">Selected synapse</param>
		/// <param name="maxPermanenceForSelectedCells">
		/// Max permanence of the selected cells</param>
		/// <returns>Connection color to display</returns>
		public Color CalculateColorForConnection(Synapse synapse,
			float maxPermanenceForSelectedCells, Color segmentColor)
		{
			// If the view is default, then 
			if (this._keyAltIsPressed == false)
			{
				return Color.FromArgb(127, segmentColor);
			}
			else // If the view is alternate, then show the weight of the synapses.
			{
				if (synapse.Permanence < Synapse.ConnectedPermanence)
				{
					// Choose blue color where the color depends on the permanence until
					// Connected, max transparency is 50%.
					var alpha = (int)((synapse.Permanence /
										Synapse.ConnectedPermanence) * byte.MaxValue * 0.5f);
					return Color.FromArgb(alpha, Color.Blue);
				}
				else
				{
					// Choose red color where the color transparency depends on the 
					// Precentage of the permanence from the max permanence and must begin
					// At least with 25% transperancy.
					float synapsePermanenceOffset = synapse.Permanence - Synapse.ConnectedPermanence;
					float maxPermanenceOffset = maxPermanenceForSelectedCells -
												Synapse.ConnectedPermanence;
					float precentageFromMaxPermanence =
						synapsePermanenceOffset / maxPermanenceOffset;
					float alpha = 0.25f + (precentageFromMaxPermanence * 0.75f);
					return Color.FromArgb((int)(alpha * byte.MaxValue), Color.Red);
				}
			}
		}

		public List<Cell> ExpandEntityIntoListOfCells(object entity)
		{
			// Expand the selected entity into a group of cells.
			var selectedCells = new List<Cell>();
			if (entity is Column)
			{
				foreach (var cell in ((Column)entity).Cells)
				{
					selectedCells.Add ( cell );
				}
			}
			if (entity is Cell)
			{
				selectedCells.Add ( (Cell)entity );
			}


			return selectedCells;
		}

		public void DisplayConnections(Graphics grpOnBitmap)
		{
			// In column sideview, you must show cells because the resolution of the columns
			// Is not high enough to make sense from it.
			bool showingInternalCells = this.ShowingInternalCells() || this._viewerMode == Mode.ColumnsSideView;

			foreach (var entity in this._selectedEntities)
			{
				List<Cell> selectedCells = this.ExpandEntityIntoListOfCells(entity);
				// First, highlight the cells and the columns.
				bool highlightedTheColumn = false;
				foreach (var cell in selectedCells)
				{
					if (showingInternalCells == false)
					{
						// If we zoom far and only show individual columns then
						// Highlight the column only once to avoid redrawing.
						if (highlightedTheColumn == false)
						{
							this.DrawColumnRectangle(cell.Column, grpOnBitmap,
													 Color.FromArgb(127, Color.Aqua), true, false);
							highlightedTheColumn = true;
						}
					}
					else
					{
						this.DrawCellRectangle(cell, grpOnBitmap,
											   Color.FromArgb(127, Color.Aqua), true, false);
					}
				}
			}

			foreach (var entity in this._selectedEntities)
			{
				List<Cell> selectedCells = this.ExpandEntityIntoListOfCells(entity);
				float maxPermanenceForRegion =
					this.GetMaxPermanenceForRegion();

				// Once we're done highlighting cells + columns, we can draw the connections on top.
				int colorIndex = 0;
				foreach (var cell in selectedCells)
				{
					// If we don't show internal cells (we still zoom far)
					// Then show the connections that originate from the center of the columns,
					// And color the segments based on the colors of the cells. (each segment from different
					// Cell is marked in a different color)
					if (showingInternalCells == false)
					{
						foreach (var segment in cell.DistalSegments)
						{
							foreach (DistalSynapse synapse in segment.Synapses)
							{
								this.DrawConnectionBetweenColumns(grpOnBitmap, cell.Column,
																synapse, this.CalculateColorForConnection(synapse,
																maxPermanenceForRegion, this._segmentColors[colorIndex]));
							}
						}

						// Change the color only when moving to the next cell.
						colorIndex++;
					}
					// If we zoom forward and show the individual cells,
					// Then the cells connections originate from the center of
					// The cells and the individual cells are highlighted.
					else
					{
						foreach (var segment in cell.DistalSegments)
						{
							foreach (DistalSynapse synapse in segment.Synapses)
							{
								this.DrawConnectionBetweenCells(grpOnBitmap, cell,
																synapse.InputSource, this.CalculateColorForConnection(synapse,
																maxPermanenceForRegion, this._segmentColors[colorIndex]),
																"Permanence : " + synapse.Permanence);
							}

							// Change the color only when moving to the next segment.
							colorIndex++;
						}

						// If the selected view is to see the cell's segment updates..
						if (this.ViewCellSegmentUpdates)
						{
							// Show segment updates as yellow lines with text.
							foreach (var update in cell.SegmentUpdates)
							{
								foreach (var cellToLearnFrom in update.CellsToConnect)
								{
									this.DrawConnectionBetweenCells(grpOnBitmap, cell,
																	cellToLearnFrom, Color.Yellow,
																	"Update", true);
								}

								foreach (DistalSynapse synapse in update.DistalSegment.Synapses)
								{
									this.DrawConnectionBetweenCells(grpOnBitmap, cell,
																	synapse.InputSource, Color.Yellow,
																	"Update", true);
								}
							}
						}
					}
				}
			}

			// Draw the synapses changes.
			foreach (var change in this._synapsesChanges)
			{
				// If there are synapses changes that we shouldn't draw, then skip them.
				if ((change.ChangeType == EDistalSynapseChange.Added) &&
					(this.ViewRegionalNewSynapses == false))
				{
					continue;
				}

				if ((change.ChangeType == EDistalSynapseChange.Removed) &&
					(this.ViewRegionalRemovedSynapses == false))
				{
					continue;
				}

				if ((change.ChangeType == EDistalSynapseChange.PermanenceIncresead) &&
					(this.ViewRegionalIncreasedPermanenceSynapses == false))
				{
					continue;
				}

				if ((change.ChangeType == EDistalSynapseChange.PermanenceDecreased) &&
					(this.ViewRegionalDecreasedPermanenceSynapses == false))
				{
					continue;
				}

				Color changeColor = Color.Black;
				switch (change.ChangeType)
				{
					case EDistalSynapseChange.Added:
						changeColor = Color.White;
						break;
					case EDistalSynapseChange.Removed:
						changeColor = Color.Black;
						break;
					case EDistalSynapseChange.PermanenceIncresead:
						changeColor = Color.LightGreen;
						break;
					case EDistalSynapseChange.PermanenceDecreased:
						changeColor = Color.Red;
						break;
				}

				if (showingInternalCells == false)
				{
					this.DrawConnectionBetweenColumns(grpOnBitmap, change.OutputCell.Column,
													  change.Synapse, changeColor);
				}
				else
				{
					this.DrawConnectionBetweenCells(grpOnBitmap, change.OutputCell,
													change.Synapse.InputSource, changeColor,
													change.ChangeText + ". Permanence : " + change.Synapse.Permanence);
				}
			}
		}

		public void DisplaySignals(Graphics grpOnBitmap)
		{
			/*
			// Display all the signals that are travelling.
			foreach (NeuralSignalReceiveWait rcvSignal in m_netNetwork.SignalsTravelling)
			{
				PointF pntFrom = ConvertViewPointToDisplayPoint(rcvSignal.conConnection.nrnFrom.Location);
				PointF pntTo = ConvertViewPointToDisplayPoint(rcvSignal.conConnection.nrnTo.Location);
				float dPrecentageOfDistanceTravelled =
					(float)(m_netNetwork.CurrentMicrosecond - rcvSignal.nMicrosecondsAtStart) / (float)
					(rcvSignal.nMicrosecondsAtReceive - rcvSignal.nMicrosecondsAtStart);
				PointF pntCurrent = new PointF((pntTo.X - pntFrom.X) * dPrecentageOfDistanceTravelled + pntFrom.X,
												(pntTo.Y - pntFrom.Y) * dPrecentageOfDistanceTravelled + pntFrom.Y);

				int nSizeOnDisplayWidth = (int)(Cell.SIZE / m_sizeOfView.Width * m_bmpGraphicsBitmap.Width);
				int nSizeOnDisplayHeight = (int)(Cell.SIZE / m_sizeOfView.Height * m_bmpGraphicsBitmap.Height);

				grpOnBitmap.FillEllipse(new SolidBrush(Color.FromArgb(150, Color.DeepSkyBlue)),
					pntCurrent.X - (nSizeOnDisplayWidth / 2),
					pntCurrent.Y - (nSizeOnDisplayHeight / 2),
					nSizeOnDisplayWidth, nSizeOnDisplayHeight);

				int nStaticSignalSize = 2;
				grpOnBitmap.FillEllipse(new SolidBrush(Color.FromArgb(30, Color.DeepSkyBlue)),
						pntCurrent.X - (nStaticSignalSize / 2),
						pntCurrent.Y - (nStaticSignalSize / 2),
						nStaticSignalSize, nStaticSignalSize);
			}*/
		}

		public void Display()
		{
			//test only 
			if (Properties.Settings.Default.StealthMode)
				return;

			if (this._region != null)
			{
				try
				{
					Graphics graphicsOnBitmap = Graphics.FromImage(this._graphicsBitmap);
					graphicsOnBitmap.CompositingQuality = CompositingQuality.HighSpeed;
					graphicsOnBitmap.InterpolationMode = InterpolationMode.Low;
					graphicsOnBitmap.SmoothingMode = SmoothingMode.HighSpeed;

					graphicsOnBitmap.Clear(Color.Wheat);

					// Handle special view modes that do not include columns view.
					switch (this._viewerMode)
					{
						case Mode.FeedFowardInputReconstruction:
							this.ShowInputReconstructionModeRows(graphicsOnBitmap);
							break;
						case Mode.PredictionReconstruction:
							this.ShowPredictionReconstructionModeRows(graphicsOnBitmap);
							break;
						default:
							this.DisplaySignals(graphicsOnBitmap);
							this.DisplayCells(graphicsOnBitmap);
							this.DisplayConnections(graphicsOnBitmap);
							this.DisplayNotification(graphicsOnBitmap);
							break;
					}

					// Draw the completed image on the panel
					Graphics graphics = this.CreateGraphics();
					graphics.DrawImage(this._graphicsBitmap, 0, 0, this.Width, this.Height);
				}
				catch (ArgumentException) { }
				// ArgumentException is sometimes raised by Graphics.FromImage() or 
				// graphics.DrawImage() because of control size 0.
			}
		}

		#region Private

		private bool AreColorsSimilar(Color colorOne, Color colorTwo, int wantedDifference)
		{
			int difference =
				Math.Abs(colorOne.R - colorTwo.R) +
				Math.Abs(colorOne.G - colorTwo.G) +
				Math.Abs(colorOne.B - colorTwo.B);
			return (difference <= wantedDifference);
		}

		private Point ConvertViewPointToDisplayPoint(PointF viewPoint)
		{
			return new Point(
				(int)(((viewPoint.X + this._startingViewPoint.X) / this._sizeOfView.Width) * this._graphicsBitmap.Width),
				(int)(((viewPoint.Y + this._startingViewPoint.Y) / this._sizeOfView.Height) * this._graphicsBitmap.Height));
		}

		private PointF ConvertDisplayPointToViewPoint(Point displayPoint)
		{
			float x = ((this._sizeOfView.Width * displayPoint.X) -
					   (this._graphicsBitmap.Width * this._startingViewPoint.X)) / this._graphicsBitmap.Width;
			float y = ((this._sizeOfView.Height * displayPoint.Y) -
					   (this._graphicsBitmap.Height * this._startingViewPoint.Y)) / this._graphicsBitmap.Height;
			return new PointF(x, y);
		}

		/// <summary>
		///  Create a dictionary with ramdom colors for drawing segments.
		/// </summary>
		private void CreateSegmentColorDictionary()
		{
			var random = new Random(DateTime.Now.Millisecond);

			// Generate different colors for segments.
			// The segment colors must be bright, different than red, green or blue,
			// And each color must not be the same.
			for (int i = 0; i < 100; i++)
			{
				bool foundColor = false;
				int numberOfTrials = 0;
				Color randomizedColor = Color.Black;
				const int maxNumberOfTrials = 100000;

				while ((foundColor == false) && (numberOfTrials < maxNumberOfTrials))
				{
					numberOfTrials++;

					var bRandomizedRed = (byte)random.Next(256);
					var bRandomizedGreen = (byte)random.Next(256);
					var bRandomizedBlue = (byte)random.Next(256);

					// If not too bright, skip.
					if ((bRandomizedRed < 100) || (bRandomizedBlue < 100) ||
						(bRandomizedGreen < 100))
					{
						continue;
					}

					// If too similar to R, G, B, skip.
					randomizedColor = Color.FromArgb(
													 bRandomizedRed, bRandomizedGreen, bRandomizedBlue);
					if (this.AreColorsSimilar(randomizedColor, Color.Red, 10))
					{
						continue;
					}
					if (this.AreColorsSimilar(randomizedColor, Color.Green, 10))
					{
						continue;
					}
					if (this.AreColorsSimilar(randomizedColor, Color.Blue, 10))
					{
						continue;
					}

					// If too similar to the already existing colors, skip.
					bool foundSimilarColorInExistingColors = false;

					foreach (var existingColor in this._segmentColors)
					{
						if (this.AreColorsSimilar(randomizedColor, existingColor,
												  10 - (maxNumberOfTrials - numberOfTrials) /
												  (maxNumberOfTrials / 10)))
						{
							foundSimilarColorInExistingColors = true;
							break;
						}
					}

					foundColor = !foundSimilarColorInExistingColors;
				}

				// See if we found a color or not.
				if (foundColor)
				{
					this._segmentColors.Add(randomizedColor);
				}
				else
				{
					throw new Exception("It cannot generate colors for visualizer");
				}
			}
		}

		private void DisplayNotification(Graphics grpOnBitmap)
		{
			// Displaying the notification if neccessary.
			// If we have not yet displayed enough frames of the notification, then draw
			// The notification.
			if (this._framesToDisplayNotification > 0)
			{
				grpOnBitmap.DrawString(this._notifyText, this._notificationFont, Brushes.Black, 0, 0);
				this._framesToDisplayNotification--;
			}
		}

		private void DrawConnectionBetweenColumns(Graphics grpOnBitmap, Column outputColumn,
			DistalSynapse synapse, Color color)
		{
			// Note that the start and end points are from the center of the
			// Column rectangle, that's why we add COLUMN_SIZE_VIRTUAL / 2
			// To the start points of the column rectangles.
			SizeF columnSizeVirtual;
			PointF columnStartPointVirtual, columnEndPointVirtual;
			this.GetColumnVirtualPointAndSize(synapse.InputSource.Column,
											  out columnStartPointVirtual, out columnSizeVirtual);
			this.GetColumnVirtualPointAndSize(outputColumn,
											  out columnEndPointVirtual, out columnSizeVirtual);

			Point pntConnectionStart = this.ConvertViewPointToDisplayPoint(new PointF(
																			   columnStartPointVirtual.X + columnSizeVirtual.Width / 2,
																			   columnStartPointVirtual.Y + columnSizeVirtual.Height / 2));
			Point pntConnectionEnd = this.ConvertViewPointToDisplayPoint(new PointF(
																			 columnEndPointVirtual.X + columnSizeVirtual.Width / 2,
																			 columnEndPointVirtual.Y + columnSizeVirtual.Height / 2));

			grpOnBitmap.DrawLine(new Pen(color, 5.0f),
								 pntConnectionStart, pntConnectionEnd);
		}

		private void DrawConnectionBetweenCells(Graphics grpOnBitmap, Cell outputCell,
			Cell inputCell, Color color, string text, bool dashed = false)
		{
			SizeF cellSize;
			PointF cellStartPointVirtual, cellEndPointVirtual;
			this.GetCellVirtualPointAndSize(inputCell, out cellStartPointVirtual, out cellSize);
			this.GetCellVirtualPointAndSize(outputCell, out cellEndPointVirtual, out cellSize);

			// Note that the start and end points are from the center of the
			// cell rectangle, that's why we add cell size / 2
			// To the start points of the cell rectangles.
			cellStartPointVirtual.X += cellSize.Width / 2;
			cellEndPointVirtual.X += cellSize.Width / 2;
			cellStartPointVirtual.Y += cellSize.Height / 2;
			cellEndPointVirtual.Y += cellSize.Height / 2;

			Point pntConnectionStart = this.ConvertViewPointToDisplayPoint(cellStartPointVirtual);
			Point pntConnectionEnd = this.ConvertViewPointToDisplayPoint(cellEndPointVirtual);

			PointF closestPoint;
			double distanceToLine = this.FindDistanceToLineSegment(this._lastMouseLocation,
																   pntConnectionStart, pntConnectionEnd, out closestPoint);

			bool focusedOnConnection = false;
			float connectionWidth = 5.0f;
			if (distanceToLine < 10)
			{
				focusedOnConnection = true;
				connectionWidth = 30f;
			}

			var pen = new Pen(color, connectionWidth);
			pen.StartCap = LineCap.Round;
			pen.EndCap = LineCap.ArrowAnchor;
			if (dashed)
			{
				pen.DashStyle = DashStyle.Dash;
			}

			grpOnBitmap.DrawLine(pen,
								 pntConnectionStart, pntConnectionEnd);

			// If the user focuses on the connection with the mouse, 
			// Then display the permanence.
			if (focusedOnConnection)
			{
				// Draw text on top of the line.
				GraphicsState gs = grpOnBitmap.Save();

				var angleOfConnectionRadians = (float)Math.Atan2(pntConnectionEnd.Y - pntConnectionStart.Y,
																  pntConnectionEnd.X - pntConnectionStart.X);
				float angleOfConnectionDegrees =
					MathHelper.ToDegrees(angleOfConnectionRadians);
				if ((angleOfConnectionDegrees < 90) && (angleOfConnectionDegrees >= 0))
				{
					angleOfConnectionDegrees += 180;
				}
				else if ((angleOfConnectionDegrees <= 0) && (angleOfConnectionDegrees >= -90))
				{
					angleOfConnectionDegrees += 180;
				}

				var font = new Font("Arial", 12);
				SizeF textSize = grpOnBitmap.MeasureString(text, font);
				grpOnBitmap.RotateTransform(angleOfConnectionDegrees + 180);
				var pntConnectionCenter = new Point((pntConnectionStart.X + pntConnectionEnd.X) / 2,
													(pntConnectionStart.Y + pntConnectionEnd.Y) / 2);
				var pntStringDrawingPoint = new Point((int)(pntConnectionCenter.X - (textSize.Width / 2)),
													  (int)(pntConnectionCenter.Y - (textSize.Height / 2)));
				grpOnBitmap.TranslateTransform(pntConnectionCenter.X,
											   pntConnectionCenter.Y, MatrixOrder.Append);

				grpOnBitmap.DrawString(text, font, Brushes.Black, 0, 0);

				grpOnBitmap.Restore(gs);
			}
		}

		private double FindDistanceToLineSegment(PointF pt, PointF p1, PointF p2,
			out PointF closest)
		{
			float dx = p2.X - p1.X;
			float dy = p2.Y - p1.Y;
			if ((dx == 0) && (dy == 0))
			{
				// It's a point not a line segment.
				closest = p1;
				dx = pt.X - p1.X;
				dy = pt.Y - p1.Y;
				return Math.Sqrt(dx * dx + dy * dy);
			}

			// Calculate the t that minimizes the distance.
			float t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) / (dx * dx + dy * dy);

			// See if this represents one of the segment's
			// end points or a point in the middle.
			if (t < 0)
			{
				closest = new PointF(p1.X, p1.Y);
				dx = pt.X - p1.X;
				dy = pt.Y - p1.Y;
			}
			else if (t > 1)
			{
				closest = new PointF(p2.X, p2.Y);
				dx = pt.X - p2.X;
				dy = pt.Y - p2.Y;
			}
			else
			{
				closest = new PointF(p1.X + t * dx, p1.Y + t * dy);
				dx = pt.X - closest.X;
				dy = pt.Y - closest.Y;
			}

			return Math.Sqrt(dx * dx + dy * dy);
		}

		private void InitializeComponent()
		{
			//this.SuspendLayout();
		}

		private void Notify(string text)
		{
			this._notifyText = text;
			this._framesToDisplayNotification = 10;

			// Refresh statuses at the parent form so that they are accurate.
			StateInformationForm.Instance.RefreshMenus();

			this.Display();
		}

		private void Notify(string text, bool boolean)
		{
			if (boolean)
			{
				text += "On";
			}
			else
			{
				text += "Off";
			}

			this.Notify(text);
		}

		#endregion

		#region View selection

		public void ViewSelect_ActiveCells()
		{
			this.ViewActiveCells = !this.ViewActiveCells;
			this.Notify("Showing active cells is ", this.ViewActiveCells);
		}

		public void ViewSelect_PredictingCells()
		{
			this.ViewPredictingCells = !this.ViewPredictingCells;
			this.Notify("Showing predicting cells is ", this.ViewPredictingCells);
		}

		public void ViewSelect_ActiveAndPredictedCells()
		{
			this.ViewActiveAndPredictedCells = !this.ViewActiveAndPredictedCells;
			this.Notify("Showing active and predicted cells is ", this.ViewActiveAndPredictedCells);
		}

		public void ViewSelect_NonActiveAndPredictedCells()
		{
			this.ViewNonActiveAndPredictedCells = !this.ViewNonActiveAndPredictedCells;
			this.Notify("Showing non-active and predicted cells is ", this.ViewNonActiveAndPredictedCells);
		}

		public void ViewSelect_CellsSegmentUpdates()
		{
			this.ViewCellSegmentUpdates = !this.ViewCellSegmentUpdates;
			this.Notify("Showing selected cell segment updates is ", this.ViewCellSegmentUpdates);
		}

		public void ViewSelect_RegionalIncreasedPermanenceSynapses()
		{
			this.ViewRegionalIncreasedPermanenceSynapses = !this.ViewRegionalIncreasedPermanenceSynapses;
			this.Notify("Showing regional synapses whose permanence had increased ", this.ViewRegionalIncreasedPermanenceSynapses);
		}

		public void ViewSelect_RegionalDecreasedPermanenceSynapses()
		{
			this.ViewRegionalDecreasedPermanenceSynapses = !this.ViewRegionalDecreasedPermanenceSynapses;
			this.Notify("Showing regional synapses whose permanence had decreased ", this.ViewRegionalDecreasedPermanenceSynapses);
		}

		public void ViewSelect_RegionalNewSynapses()
		{
			this.ViewRegionalNewSynapses = !this.ViewRegionalNewSynapses;
			this.Notify("Showing new regional synapses ", this.ViewRegionalNewSynapses);
		}

		public void ViewSelect_RegionalRemovedSynapses()
		{
			this.ViewRegionalRemovedSynapses = !this.ViewRegionalRemovedSynapses;
			this.Notify("Showing removed regional synapses ", this.ViewRegionalRemovedSynapses);
		}

		public void ViewSelect_SelectedAll()
		{
			this.ViewActiveCells = true;
			this.ViewPredictingCells = true;
			this.ViewActiveAndPredictedCells = true;
			this.ViewNonActiveAndPredictedCells = true;
			this.ViewCellSegmentUpdates = true;
			this.ViewRegionalIncreasedPermanenceSynapses = true;
			this.ViewRegionalDecreasedPermanenceSynapses = true;
			this.ViewRegionalNewSynapses = true;
			this.ViewRegionalRemovedSynapses = true;
			this.Notify("Selected all view information");
		}

		public void ViewSelect_DeselectedAll()
		{
			this.ViewActiveCells = false;
			this.ViewPredictingCells = false;
			this.ViewActiveAndPredictedCells = false;
			this.ViewNonActiveAndPredictedCells = false;
			this.ViewCellSegmentUpdates = false;
			this.ViewRegionalIncreasedPermanenceSynapses = false;
			this.ViewRegionalDecreasedPermanenceSynapses = false;
			this.ViewRegionalNewSynapses = false;
			this.ViewRegionalRemovedSynapses = false;
			this.Notify("Deselected all view information");
		}

		public void ViewSelect_DefaultView()
		{
			this.ViewActiveCells = true;
			this.ViewPredictingCells = true;
			this.ViewActiveAndPredictedCells = true;
			this.ViewNonActiveAndPredictedCells = true;
			this.ViewCellSegmentUpdates = false;
			this.ViewRegionalIncreasedPermanenceSynapses = false;
			this.ViewRegionalDecreasedPermanenceSynapses = false;
			this.ViewRegionalNewSynapses = false;
			this.ViewRegionalRemovedSynapses = false;
			this.Notify("View information is now in default view");
		}

		#endregion

		#endregion

		#region Event handlers

		private void Panel_Resize(object sender, EventArgs e)
		{
			this._graphicsBitmap.Dispose();

			if (this.Width > 0 && this.Height > 0)
			{
				this._graphicsBitmap = new Bitmap(this.Width, this.Height);
			}
		}

		private void Panel_KeyDown(object o, KeyEventArgs e)
		{
			bool needToRefreshView = false;

			if (e.Control)
			{
				this._keyControlIsPressed = true;
			}
			if (e.Alt)
			{
				// Unfortunely, OnKeyDown is activated so long as the key is down.
				// And not once. therefore we need to make sure we refresh the view
				// Only once.
				if (this._keyAltIsPressed == false)
				{
					this._keyAltIsPressed = true;
					needToRefreshView = true;
				}
			}

			//base.OnKeyDown(e);

			if (needToRefreshView)
			{
				this.Display();
			}
		}



		private void Panel_KeyUp(object o, KeyEventArgs e)
		{
			if (e.Control)
			{
				this._keyControlIsPressed = false;
			}
			if (e.KeyData == (Keys.LButton | Keys.ShiftKey))
			{
				this._keyControlIsPressed = false;
			}
			if (e.Alt)
			{
				this._keyAltIsPressed = false;
			}
			if (e.KeyData == (Keys.RButton | Keys.ShiftKey))
			{
				this._keyAltIsPressed = false;
			}
			if ((e.KeyData == Keys.D1) || (e.KeyData == Keys.NumPad1))
			{
				this.ViewSelect_ActiveCells();
			}
			if ((e.KeyData == Keys.D2) || (e.KeyData == Keys.NumPad2))
			{
				this.ViewSelect_PredictingCells();
			}
			if ((e.KeyData == Keys.D3) || (e.KeyData == Keys.NumPad3))
			{
				this.ViewSelect_ActiveAndPredictedCells();
			}
			if ((e.KeyData == Keys.D4) || (e.KeyData == Keys.NumPad4))
			{
				this.ViewSelect_NonActiveAndPredictedCells();
			}
			if ((e.KeyData == Keys.D5) || (e.KeyData == Keys.NumPad5))
			{
				this.ViewSelect_CellsSegmentUpdates();
			}
			if ((e.KeyData == Keys.D6) || (e.KeyData == Keys.NumPad6))
			{
				this.ViewSelect_RegionalIncreasedPermanenceSynapses();
			}
			if ((e.KeyData == Keys.D7) || (e.KeyData == Keys.NumPad7))
			{
				this.ViewSelect_RegionalDecreasedPermanenceSynapses();
			}
			if ((e.KeyData == Keys.D8) || (e.KeyData == Keys.NumPad8))
			{
				this.ViewSelect_RegionalNewSynapses();
			}
			if ((e.KeyData == Keys.D9) || (e.KeyData == Keys.NumPad9))
			{
				this.ViewSelect_RegionalRemovedSynapses();
			}
			if (e.KeyData == Keys.Back)
			{
				this.ViewSelect_DeselectedAll();
			}

			this.Display();
		}

		private void Panel_MouseMove(object sender, MouseEventArgs e)
		{
			bool needToDisplay = false;

			//JS
			if (!this.Focused)
				this.Focus ();

			// Check if the last mouse location is set.
			if (!this._lastMouseLocationWasSet)
			{
				this._lastMouseLocation = new Point(e.X, e.Y);
				this._lastMouseLocationWasSet = true;
				return;
			}

			// Update the mouse coordinates in the world.
			this._relativeMouseLocationInVirtualWorld =
				this.ConvertDisplayPointToViewPoint(new Point(e.X, e.Y));

			// If currently the user is pressing the mouse, then move the view according
			// To the viewer size and the mouse location (from last time)
			if (this._pressingTheMouse)
			{
				var offsetMouseLocation = new Point(this._lastMouseLocation.X - e.X,
													this._lastMouseLocation.Y - e.Y);

				// Calculate the precentage that the mouse has moved in the screen size.
				var precentageOffset = new PointF((float)offsetMouseLocation.X / this.Width, (float)offsetMouseLocation.Y / this.Height);

				// Convert the precentage of the screen size, to the actual precentage from the
				// Viewer size.
				var offsetInViewer = new PointF(precentageOffset.X * this._sizeOfView.Width, precentageOffset.Y * this._sizeOfView.Height);

				this._startingViewPoint.X -= offsetInViewer.X;
				this._startingViewPoint.Y -= offsetInViewer.Y;

				needToDisplay = true;
			}

			this._lastMouseLocation = new Point(e.X, e.Y);

			// If there are selected entities, display.
			if (this._selectedEntities.Count > 0)
			{
				needToDisplay = true;
			}

			// If the control is pressed, then display.
			if (this._keyControlIsPressed)
			{
				needToDisplay = true;
			}

			if (needToDisplay)
			{
				this.Display();
			}
		}

		private void Panel_MouseUp(object sender, MouseEventArgs e)
		{
			this._pressingTheMouse = false;
		}

		private void Panel_MouseDown(object sender, MouseEventArgs e)
		{
			this._pressingTheMouse = true;
			this._lastMouseLocationWasSet = false;

			// Do we hover on something and pressing control? if yes, then add it
			// To the list of entities selected.
			if ((e.Button == MouseButtons.Left) &&
				this._keyControlIsPressed) 
			{
				if (this._mouseHoversEntity != null)
				{
					// Only add the object if it doesn't already exists in the selected list.
					if (this._selectedEntities.Contains ( this._mouseHoversEntity ) == false)
					{
						this._selectedEntities.Add ( this._mouseHoversEntity );
						//List<Cell> selectedCells = this.ExpandEntityIntoListOfCells ( this._selectedEntities[0] );
						//if (selectedCells.Count > 0)
							StateInformationPanel_SelectionChanged ( this, e, _selectedEntities );
					}
				}
			}

			// If wer'e pressing right click, clear all selected entities.
			if (e.Button == MouseButtons.Right)
			{
				this._selectedEntities.Clear ();
				StateInformationPanel_SelectionChanged ( this, e, _selectedEntities );
			}

			this.Display ();
		}

		public void Panel_MouseWheel(object sender, MouseEventArgs e)
		{
			// Delta can be minimum -120 when zooming out, or maximum 120 when zooming in.
			// Each "20" is a mouse tick.
			// Convert the delta to first be between 120 to 360 range, then divide by 240. 
			// This should give us a value between 0.5 to 2 to zoom by.
			var zoomAmount = (float)((e.Delta + 240) / 240.0);

			var sizeNewViewingSize = new SizeF();
			sizeNewViewingSize.Width = this._sizeOfView.Width / zoomAmount;
			sizeNewViewingSize.Height = this._sizeOfView.Height / zoomAmount;

			// Check if the zoom is too small / too big.
			// If it is, then don't accept the new size.
			if ((sizeNewViewingSize.Width < this._smallestZoomSizeInVirtual) ||
				(sizeNewViewingSize.Height < this._smallestZoomSizeInVirtual) ||
				(sizeNewViewingSize.Width > this._largestZoomSizeInVirtual) ||
				(sizeNewViewingSize.Height > this._largestZoomSizeInVirtual))
			{
				sizeNewViewingSize = this._sizeOfView;
			}

			// If one of the sizes is invalid, return.
			if ((sizeNewViewingSize.Width == 0) || (sizeNewViewingSize.Height == 0))
			{
				return;
			}

			// Calculate the size difference between the two.
			var sizeNewToOldDifference = new SizeF(sizeNewViewingSize.Width - this._sizeOfView.Width,
												   sizeNewViewingSize.Height - this._sizeOfView.Height);

			// Make the viewing starting position to shift inwards or outwards,
			// In order to center on the zooming.
			this._startingViewPoint.X += sizeNewToOldDifference.Width / 2;
			this._startingViewPoint.Y += sizeNewToOldDifference.Height / 2;

			// Apply the new size.
			this._sizeOfView = sizeNewViewingSize;

			this.Display();
		}

		#endregion
	}
}
