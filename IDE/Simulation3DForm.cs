using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OpenHTM.IDE
{
	public partial class Simulation3DForm : DockContent
	{
		#region Fields

		// Private singleton instance
		// private static VisualizeTemporalPooler instance = null;

		private Microsoft.Xna.Framework.Point _mousePosition;

		#endregion

		#region Properties

		public bool ShowActiveColumnGrid { get; private set; }
		public bool ShowPredictedGrid { get; private set; }
		public bool ShowTemporalLearning { get; private set; }
		public bool ShowSpatialLearning { get; private set; }
		public bool ShowCoordinateSystem { get; private set; }

		public bool ShowActiveCells { get; private set; }
		public bool ShowCorrectPredictedCells { get; private set; }
		public bool ShowFalsePredictedCells { get; private set; }
		public bool ShowLearningCells { get; private set; }
		public bool ShowPredictingCells { get; private set; }
		public bool ShowSeqPredictingCells { get; private set; }

		/*/// <summary>
        /// Singleton
        /// </summary>
        public static VisualizeTemporalPooler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new VisualizeTemporalPooler();
                }
                return instance;
            }
        }*/

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Simulation3DForm"/> class.
		/// </summary>
		public Simulation3DForm()
		{
			this.InitializeComponent();

			// Set UI properties
			this.ShowActiveCells = true;
			this.ShowCorrectPredictedCells = true;
			this.ShowFalsePredictedCells = true;
			this.ShowLearningCells = true;
			this.ShowPredictingCells = true;
			this.ShowSeqPredictingCells = true;
			this.ShowCoordinateSystem = true;

			this.showActiveButton.Text = this.ShowActiveCells ? "+" : "-";
			this.showCorrectButton.Text = this.ShowCorrectPredictedCells ? "+" : "-";
			this.showFalsePredictedButton.Text = this.ShowFalsePredictedCells ? "+" : "-";
			this.showLearningButton.Text = this.ShowLearningCells ? "+" : "-";
			this.showPredictingButton.Text = this.ShowPredictingCells ? "+" : "-";
			this.showSeqPredictingButton.Text = this.ShowSeqPredictingCells ? "+" : "-";

			this.showActiveButton.ToolTipText = "Show/hide Active Cells";
			this.showCorrectButton.ToolTipText = "Show/hide Correctly Predicted Cells";
			this.showFalsePredictedButton.ToolTipText = "Show/hide Falsely Predicted Cells";
			this.showLearningButton.ToolTipText = "Show/hide Learning Cells";
			this.showPredictingButton.ToolTipText = "Show/hide Predicting Cells";
			this.showSeqPredictingButton.ToolTipText = "Show/hide Sequence Predicting Cells";
		}

		#endregion

		#region Methods

		public IntPtr GetDrawSurface()
		{
			return this.pictureBoxSurface.Handle;
		}

		#endregion

		#region Events

		#region PictureBox

		/// <summary>
		/// Reset graphic device due to the new size
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxSurface_Resize(object sender, EventArgs e)
		{
			Simulation3D.Engine.ResetGraphicsDevice();
		}

		/// <summary>
		/// Compute rotation angle for camera view and rotation angle for X-Y-Axis rotation for whole scene
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxSurface_MouseMove(object sender, MouseEventArgs e)
		{

			float diffX = (e.X - this._mousePosition.X);
			float diffY = (e.Y - this._mousePosition.Y);

			this._mousePosition.X = e.X;
			this._mousePosition.Y = e.Y;
			
			//debug js
			Simulation3D.Engine.mouseLocation = new Microsoft.Xna.Framework.Point ( e.X, e.Y );
		
			// Rotation angle for camera in world space
			if (e.Button == MouseButtons.Left)
			{
				Simulation3D.Engine.RotateWorldSpaceCamera(diffX, diffY);
			}

			// Rotation angle for htm-objects in world space
			if (e.Button == MouseButtons.Right)
			{
				Simulation3D.Engine.RotateWorldSpaceHtmObjects(diffX, diffY);
			}

			// Set focus to the hosting surface in order to mouse wheel event works
			if (!this.pictureBoxSurface.Focused)
			{
				this.pictureBoxSurface.Focus();
			}

			if (!(e.Button == MouseButtons.Left) && !(e.Button == MouseButtons.Right))
			{
				Simulation3D.Engine.Pick ( e.Location, false );
			}
		}

		/// <summary>
		/// Set the zoom the camera
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxSurface_MouseWheel(object sender, MouseEventArgs e)
		{
			// Set camera zoom with mouse wheel value
			Simulation3D.Engine.SetCameraZoom(-e.Delta);
		}

		/// <summary>
		/// Reset to the original view
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnResetCamera_Click ( object sender, EventArgs e )
		{
			// Reset rotation angle and zoom for camera
			Simulation3D.Engine.ResetCamera ();
		}


		/// <summary>
		/// Select/Deselect objects
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxSurface_MouseClick ( object sender, MouseEventArgs e )
		{
			Simulation3D.Engine.Pick ( e.Location, true );
		}


		/// <summary>
		/// elect/Deselect objects
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxSurface_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			//Reset rotation angle and zoom for camera
			Simulation3D.Engine.ResetCamera();
		}

		#endregion

		#region Menu

		private void menuActiveColumnGrid_Click(object sender, EventArgs e)
		{
			this.ShowActiveColumnGrid = !this.ShowActiveColumnGrid;
		}

		private void menuShowPredictedGrid_Click(object sender, EventArgs e)
		{
			this.ShowPredictedGrid = !this.ShowPredictedGrid;
		}

		private void menuShowSpatialLearning_Click(object sender, EventArgs e)
		{
			this.ShowSpatialLearning = !this.ShowSpatialLearning;
		}

		private void menuShowTemporalLearning_Click(object sender, EventArgs e)
		{
			this.ShowTemporalLearning = !this.ShowTemporalLearning;
		}

		private void menuShowCoordinateSystem_Click(object sender, EventArgs e)
		{
			this.ShowCoordinateSystem = !this.ShowCoordinateSystem;
		}

		#endregion

		private void showActiveButton_Click(object sender, EventArgs e)
		{
			this.ShowActiveCells = !this.ShowActiveCells;
			this.showActiveButton.Text = this.ShowActiveCells ? "+" : "-";
		}

		private void showCorrectButton_Click(object sender, EventArgs e)
		{
			this.ShowCorrectPredictedCells = !this.ShowCorrectPredictedCells;
			this.showCorrectButton.Text = this.ShowCorrectPredictedCells ? "+" : "-";
		}

		private void showFalsePredictedButton_Click(object sender, EventArgs e)
		{
			this.ShowFalsePredictedCells = !this.ShowFalsePredictedCells;
			this.showFalsePredictedButton.Text = this.ShowFalsePredictedCells ? "+" : "-";
		}

		private void showLearningButton_Click(object sender, EventArgs e)
		{
			this.ShowLearningCells = !this.ShowLearningCells;
			this.showLearningButton.Text = this.ShowLearningCells ? "+" : "-";
		}

		private void showPredictingButton_Click(object sender, EventArgs e)
		{
			this.ShowPredictingCells = !this.ShowPredictingCells;
			this.showPredictingButton.Text = this.ShowPredictingCells ? "+" : "-";
		}

		private void showSeqPredictingButton_Click(object sender, EventArgs e)
		{
			this.ShowSeqPredictingCells = !this.ShowSeqPredictingCells;
			this.showSeqPredictingButton.Text = this.ShowSeqPredictingCells ? "+" : "-";
		}

		#endregion

		

		
	}
}
