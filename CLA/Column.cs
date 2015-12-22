using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.IO;
using MathNet.Numerics.Distributions;
using OpenHTM.CLA.Extensions;
using OpenHTM.CLA.Statistics;
//using OpenHTM.Shared.Interfaces;

namespace OpenHTM.CLA
{
	/// <summary>
	/// Represents a single column of cells within an HTM Region. 
	/// </summary>
	public class Column : IWatchItem
	{
		#region Fields

		/// <summary>
		/// Statistics used for check accuracy of predictions.
		/// </summary>
		public StatisticsColumn Statistics = new StatisticsColumn();

		/// <summary>
		/// Region which this column belongs.
		/// </summary>
		public Region Region { get; private set; }
		
		/// <summary>
		/// Toggle whether or not this Column is currently active.
		/// It represents the active state of a column at time t, i.e. if the column is winner due to bottom-up input.
		/// </summary>
		public List<bool> ActiveState = new List<bool>();

		/// <summary>
		/// Returns true if the cell is inhibited by neighbor columns.
		/// </summary>
		/// <remarks>
		/// Results from column inhibition. 
		/// Only possible if Overlap > MinOverlap but Overlap LT MinLocalActivity.
		/// </remarks>
		public List<bool> InhibitedState = new List<bool>();

		// Exponential Moving Average Alpha value
		private const float _emaAlpha = 0.005f;

		/// <summary>
		/// Ramdomizer to choose cells
		/// </summary>
		private Random _random;

		#endregion

		#region Properties

		/// <summary>
		/// A proximal dendrite segment forms synapses with feed-forward inputs.
		/// Columns has only one proximal segment.
		/// </summary>
		//public ProximalSegment ProximalSegment { get; private set; }
		public ProximalSegment ProximalSegment { get; set; }

		/// <summary>
		/// Contains all the cells in this column.
		/// </summary>
		public BindingList<Cell> Cells { get; private set; }


		/// <summary>
		/// The central position (x,y) of this column in the dimensional plane of the input grid.
		/// </summary>
		public Point CentralPositionInInput { get; private set; }


		/// <summary>
		/// The position (x,y) of this column in the dimensional plane of the region
		/// </summary>
		public Point PositionInRegion { get; private set; }


		/// <summary>
		/// The minimum number of inputs that must be active for a column to be 
		/// considered during the inhibition step. Value established by input parameters
		/// and receptive-field size. 
		/// </summary>
		public int MinOverlap { get; set; }

		/// <summary>
		/// Column overlap with a paritcular input pattern.  This is the current number of 
		/// active and connnected proximal synapses from last time step for this Column.
		/// </summary>
		public int Overlap { get; set; }

		/// <summary>
		/// The boost value for a column as computed during learning which is used to 
		/// increase the overlap value for inactive columns.
		/// </summary>
		/// <remarks>
		/// The boost value is a scalar >= 1. 
		/// If activeDutyCyle is above minDutyCycle, the boost value is 1. 
		/// The boost increases linearly once the column's activeDutyCycle starts falling
		/// below its minDutyCycle.
		/// </remarks>
		public float Boost { get; set; }

		/// <summary>
		/// A sliding average representing how often a column has been active after inhibition (e.g. over the last 1000 iterations).
		/// </summary>
		public float ActiveDutyCycle { get; set; }

		/// <summary>
		/// A sliding average representing how often a column has had significant overlap (i.e. greater than minOverlap) with its inputs (e.g. over the last 1000 iterations).
		/// </summary>
		public float OverlapDutyCycle { get; set; }

		#region UI

		public bool IsDataGridSelected { get; set; }

		#endregion

		#endregion

		#region Constructor


		
		/// <summary>
		/// Initializes a new Column for the given parent <see cref="Region"/> at source 
		/// row/column position srcPos and column grid position pos.
		/// </summary>
		/// <param name="region">The parent Region this Column belongs to.</param>
		/// <param name="centralPositionInInput">A Point(x,y) of this Column's 'center' position in 
		///   terms of the proximal-synapse input space.</param>
		/// <param name="positionInRegion">A Point(x,y) of this Column's position within the Region's 
		///   column grid.</param>
		public Column(Region region, Point centralPositionInInput, Point positionInRegion)
		{
			// Set fields
			this.Region = region;
			this.CentralPositionInInput = centralPositionInInput;
			this.PositionInRegion = positionInRegion;
			this.Overlap = 0;
			this.Boost = 1.0f;
			this.ActiveDutyCycle = 1;
			this.OverlapDutyCycle = 1.0f;
			this._random = new Random((positionInRegion.Y * this.Region.Size.Width) + positionInRegion.X);

			// Initialize state vectors with fixed lenght = T
			for (int t = 0; t <= Global.T; t++)
			{
				this.ActiveState.Add(false);
				this.InhibitedState.Add(false);
			}

			// Fill Column with contextual cells
			this.Cells = new BindingList<Cell>();
			for (int i = 0; i < region.CellsPerColumn; i++)
			{
				this.Cells.Add(new Cell(this, i));
			}

			// The list of potential proximal synapses and their permanence values.
			this.ProximalSegment = new ProximalSegment ( this );
		}


		/// <summary>
		/// Parameterless constructor for serialization.
		/// </summary>
		/// 
		public Column ()
		{ }

		#endregion

		#region Methods

		internal void NextTimeStep()
		{
			// Compute basic statistics
			this.ComputeBasicStatistics();

			// Remove the element in first position in order to the state vector always have lenght = T
			// and add a new element to represent the state at current time step
			this.ActiveState.RemoveAt(0);
			this.ActiveState.Add(false);
			this.InhibitedState.RemoveAt(0);
			this.InhibitedState.Add(false);

			// Loop cells in order to perfom actions related to time step progression
			foreach (var cell in this.Cells)
			{
				cell.NextTimeStep();
			}
		}

		
		/// <summary>
		/// For each (position in inputSpaceRandomPositions): 
		///  1. Create a new InputCell with input bit = position
		///  2. Attach a new ProximalSynapse
		///  3. Add the synapse to the synapse update list.
		/// </summary>
		/// <remarks>
		/// Prior to receiving any inputs, the region is initialized by computing a list of 
		/// initial potential synapses for each column. This consists of a random set of inputs
		/// selected from the input space. Each input is represented by a synapse and assigned
		/// a random permanence value. The random permanence values are chosen with two 
		/// criteria. First, the values are chosen to be in a small range around connectedPerm
		/// (the minimum permanence value at which a synapse is considered "connected"). This 
		/// enables potential synapses to become connected (or disconnected) after a small 
		/// number of training iterations. Second, each column has a natural center over the 
		/// input region, and the permanence values have a bias towards this center (they 
		/// have higher values near the center).
		/// 
		/// The concept of Locality Radius is an additional parameter to control how 
		/// far away synapse connections can be made instead of allowing connections anywhere.  
		/// The reason for this is that in the case of video images I wanted to experiment 
		/// with forcing each Column to only learn on a small section of the total input to 
		/// more effectively learn lines or corners in a small section.
		/// </remarks>
		internal void CreateProximalSegments()
		{
			// Calculates inputRadius for Columns from localityRadius
			// JS - should there be 2 inputRadiae, for X and Y, to allow for non-square input fields?
			var inputRadius = (int) Math.Round(this.Region.LocalityRadius * this.Region.InputProportionX);

			// The coordinates of the input space for the Column
			// Think of input space like a 'imaginary' square below the column center.
			int minY, maxY, minX, maxX;
			if (this.Region.LocalityRadius > 0)
			{
				// Compute values of input square and cut radius on edges
				minX = Math.Max(0, this.CentralPositionInInput.X - inputRadius);
				minY = Math.Max(0, this.CentralPositionInInput.Y - inputRadius);
				maxX = Math.Min(this.Region.InputSize.Width - 1,
				                this.CentralPositionInInput.X + inputRadius);
				maxY = Math.Min(this.Region.InputSize.Height - 1,
				                this.CentralPositionInInput.Y + inputRadius);
			}
			else
			{
				minX = 0;
				minY = 0;
				maxX = this.Region.InputSize.Width - 1;
				maxY = this.Region.InputSize.Height - 1;
			}

			// Compute input area
			int inputArea = (maxX - minX + 1) * (maxY - minY + 1);

			// Proximal synapses per Column (input segment)
			// TODO: give user some control over the number of synapses per segment
			//var synapsesPerSegment =

			//debug js
			//	(int) (inputArea * this.Region.PercentageInputPerColumn);
			var synapsesPerSegment = Math.Max(1, 
				(int)(inputArea * this.Region.PercentageInputPerColumn));
			
			// Updates minimum overlap value, i.e. the minimum number of inputs that must 
			// be active for a column to be considered during the inhibition step.
			//debug js
			//this.MinOverlap =
			//	(int) Math.Round(synapsesPerSegment * this.Region.PercentageMinOverlap);
			this.MinOverlap = Math.Max(1,
				(int)Math.Round ( synapsesPerSegment * this.Region.PercentageMinOverlap ));

			// Create all possible x,y positions for this column input space
			var inputPositions = new List<Point>();
			for (int y = minY; y <= maxY; y++)
			{
				for (int x = minX; x <= maxX; x++)
				{
					var inputPosition = new Point(x, y);
					inputPositions.Add(inputPosition);
				}
			}

			// Random sample of unique input positions (no duplicates).
			// Tie the random seed to this Column's position for reproducibility
			int randomSeed = (this.PositionInRegion.Y * this.Region.Size.Width) + this.PositionInRegion.X;
			IEnumerable<Point> inputRandomPositions =
				inputPositions.RandomSample(synapsesPerSegment, randomSeed, false);

			// Initialize the gaussian normal distribution
			// The values are chosen to be in a small range around connectedPerm
			// (the minimum permanence value at which a synapse is considered "connected")
			
			//var gausianNormalDistribution =
			//	new Normal(Synapse.InitialPermanence, Synapse.PermanenceIncrement);
			//gausianNormalDistribution.RandomSource = new Random(randomSeed);

			// JS
			var gausianNormalDistribution =
				new Normal ( Synapse.ConnectedPermanence, Synapse.PermanenceIncrement );
			gausianNormalDistribution.RandomSource = new Random ( randomSeed );

			// Create proximal synapses to ramdom positions in input
			int longerSide = Math.Max(this.Region.InputSize.Width, this.Region.InputSize.Height);
			foreach (var position in inputRandomPositions)
			{
				var newInputCell = new InputCell(this.Region, position.X, position.Y);

				if (this.Region.FullDefaultSpatialPermanence)
				{
					// Create new synapse and add it to segment
					this.ProximalSegment.CreateSynapse(newInputCell, 1.0f);
				}
				else
				{
					// Get new value for permanence from distribution
					double permanence = gausianNormalDistribution.Sample();

					// Distance from 'center' of this column to the current position in the input space
					var distanceInputFromColumn = new Point();
					distanceInputFromColumn.X = this.CentralPositionInInput.X - position.X;
					distanceInputFromColumn.Y = this.CentralPositionInInput.Y - position.Y;
					double distanceToInput = Math.Sqrt(
						(distanceInputFromColumn.X * distanceInputFromColumn.X) + 
						(distanceInputFromColumn.Y * distanceInputFromColumn.Y));

					// Each column has a natural center over the input region, and the 
					// permanence values have a bias towards this center (they have higher values near 
					// the center)
					int radiusBiasPeak = 2;
					float radiusBiasStandardDeviation = 0.25f;
					double localityBias = radiusBiasPeak / 2.0f *
					                      Math.Exp(Math.Pow(distanceToInput /
					                                        (longerSide * radiusBiasStandardDeviation), 2) / -2);

					StreamWriter file = new StreamWriter ( "localityBias.txt", false );
					
					for (float longSide = 0.1f; longSide < 2.0f; longSide += 0.1f)
					{
						file.WriteLine ( "RadiusBiasPeak,distToInput,longerSide,SD,localityBias" );
						for (float distToInput = 0.0f; distToInput < 2.0f; distToInput += 0.1f)
						{
							localityBias = radiusBiasPeak / 2.0f *
											  Math.Exp ( Math.Pow ( distToInput /
																(longSide * radiusBiasStandardDeviation), 2 ) / -2 );
							file.WriteLine ( String.Format ( "{0},{1},{2},{3},{4}", radiusBiasPeak, distToInput, longSide, radiusBiasStandardDeviation, localityBias ) );
						}
					}
					file.Close ();

					// JS
					//double permanenceBias = Math.Min(1.0f, permanence * localityBias);
					double permanenceBias = Math.Min ( 1.0f, permanence + localityBias );

					// Create new synapse and add it to segment
					this.ProximalSegment.CreateSynapse(newInputCell, permanenceBias);
				}
			}
		}

		/// <summary>
		/// Returns the maximum active duty cycle of the columns that are within
		/// inhibitionRadius of this column.
		/// </summary>
		/// <returns>Maximum active duty cycle among neighboring columns</returns>
		public float GetMaxDutyCycle()
		{
			float maxDutyCycle = 0.0f;
			foreach (var neighborColumn in this.Region.GetNeighbors(this))
			{
				if (neighborColumn.ActiveDutyCycle > maxDutyCycle)
				{
					maxDutyCycle = neighborColumn.ActiveDutyCycle;
				}
			}

			return maxDutyCycle;
		}

		/// <summary>
		/// Computes a moving average of how often this column has been active 
		/// after inhibition.
		/// Exponential moving average (EMA):
		/// St = a * Yt + (1-a)*St-1
		/// </summary>
		public float UpdateActiveDutyCycle()
		{
			float newActiveDutyCycle = (1.0f - _emaAlpha) * this.ActiveDutyCycle;
			if (this.ActiveState[Global.T])
			{
				newActiveDutyCycle += _emaAlpha;
			}

			return newActiveDutyCycle;
		}

		/// <summary>
		/// Computes a moving average of how often this column has overlap greater than
		/// MinOverlap.
		/// Exponential moving average (EMA):
		/// St = a * Yt + (1-a)*St-1
		/// </summary>
		public float UpdateOverlapDutyCycle()
		{
			float newOverlapDutyCycle = (1.0f - _emaAlpha) * this.OverlapDutyCycle;
			if (this.Overlap > this.MinOverlap)
			{
				newOverlapDutyCycle += _emaAlpha;
			}

			return newOverlapDutyCycle;
		}

		/// <summary>
		/// For this column, return the cell with the best matching <see cref="Segment"/> 
		/// (at time t-1 or t). Only consider segments that are predicting cell activation 
		/// to occur in exactly numberPredictionSteps many time steps from now. If no cell 
		/// has a matching segment, then return the cell with the fewest number of segments.
		/// </summary>
		/// <param name="t">only consider active segments at time t.</param>
		/// <param name="numberPredictionSteps">only consider segments that are predicting
		/// cell activation to occur in exactly this many time steps from now.</param>
		/// <returns>Tuple containing the best cell and its best <see cref="Segment"/> 
		/// (may be None).</returns>
		internal Tuple<Cell, DistalSegment> GetBestMatchingCell(int t, int numberPredictionSteps)
		{
			Cell bestCell = null;
			DistalSegment bestDistalSegment = null;
			int bestNumberActiveSynapses = 0;

			// Chooses the cell with the best matching segment.
			foreach (var cell in this.Cells)
			{
				DistalSegment distalSegment = cell.GetBestMatchingDistalSegment(t, numberPredictionSteps);

				if (distalSegment != null)
				{
					int numberActiveSynapses = distalSegment.GetActiveSynapses(t).Count;
					if (numberActiveSynapses > bestNumberActiveSynapses)
					{
						bestCell = cell;
						bestDistalSegment = distalSegment;
						bestNumberActiveSynapses = numberActiveSynapses;
					}
				}
			}

			// If there are no active sequences, return the cell with the fewest number of 
			// segments
			if (bestCell == null)
			{
				int bestNumberActiveSegments = int.MaxValue;
				foreach (var cell in this.Cells)
				{
					int numberActiveSegments = cell.DistalSegments.Count;
					if (numberActiveSegments < bestNumberActiveSegments)
					{
						bestNumberActiveSegments = numberActiveSegments;
						bestCell = cell;
					}
				}

				// Pick a random cell among those with equal segment counts
				// TODO: is there a more efficient way to do this maybe?
				var candidates = new List<Cell>();
				foreach (var cell in this.Cells)
				{
					if (cell.DistalSegments.Count == bestNumberActiveSegments)
					{
						candidates.Add(cell);
					}
				}
				if (candidates.Count > 1)
				{
					bestCell = candidates[this._random.Next(candidates.Count)];
				}

				// Leave bestSegment null to indicate a new segment is to be added
			}

			return new Tuple<Cell, DistalSegment>(bestCell, bestDistalSegment);
		}

		

		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public virtual string ID ()
		{
			string str = "";
			str = String.Format ( "Cl{0},{1} {2}", PositionInRegion.X, PositionInRegion.Y, Region.ID () );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public virtual void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			
			//rename ancestor Table
			DataTable dt = this.Statistics.DataTable ( tableName );
			//this.Statistics.AddWatchTable ( ref dataSet );  //later
			this.ProximalSegment.AddWatchTable ( ref dataSet, tableName );

			//Cells - BindingList<Cell>
			foreach (Cell cell in Cells)
			{
				cell.AddWatchTable ( ref dataSet, tableName );
			}

			//base.AddWatchTable ( ref dataSet );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public virtual void AddWatchData ( ref DataTable dt )
		{
			AddColumns ( ref dt );
			AddDataRow ( ref dt );

			//base.AddWatchData ( ref dt );
		}

		/// <summary>
		/// Convert object specific data to DataTable
		/// </summary>
		/// <returns>DataTable representing object.</returns>
		public virtual DataTable DataTable ( string tableName = "" )
		{
			DataTable dt = new DataTable ( ID () + " " + tableName );
			AddColumns ( ref dt );
			AddDataRow ( ref dt );
			return dt;
		}

		/// <summary>
		/// Add object specific columns to DataTable
		/// </summary>
		/// <returns>DataTable representing contents.</returns>
		public virtual void AddColumns ( ref DataTable dt )
		{
			//add Columns
			dt.Columns.Add ( "Region", typeof ( string ) );
			dt.Columns.Add ( "PositionInRegion", typeof ( Point ) );
			dt.Columns.Add ( "_emaAlpha", typeof ( Single ) );
			dt.Columns.Add ( "CentralPositionInInput", typeof ( Point ) );
			dt.Columns.Add ( "MinOverlap", typeof ( int ) );
			dt.Columns.Add ( "Overlap", typeof ( int ) );
			dt.Columns.Add ( "Boost", typeof ( Single ) );
			dt.Columns.Add ( "ActiveDutyCycle", typeof ( Single ) );
			dt.Columns.Add ( "OverlapDutyCycle", typeof ( Single ) );
		}

		/// <summary>
		/// Add DataRow with object data to DataTable. 
		/// Note: DataRow schema must match the object. (by prior call to AddColumns() )
		/// </summary>
		/// <returns></returns>
		public virtual void AddDataRow ( ref DataTable dt )
		{
			//add row
			DataRow dr = dt.NewRow ();
			AddRowData ( ref dr );
			dt.Rows.Add ( dr );
		}

		/// <summary>
		/// Fill DataRow with object data. 
		/// Note: DataRow schema must match the object. (by prior call to AddColumns() )
		/// </summary>
		/// <returns></returns>
		public virtual void AddRowData ( ref DataRow dr )
		{
			//add data
			dr["Region"] = Region.ID ();
			dr["PositionInRegion"] = PositionInRegion;
			dr["_emaAlpha"] = _emaAlpha;
			dr["CentralPositionInInput"] = CentralPositionInInput;
			dr["MinOverlap"] = MinOverlap;
			dr["Overlap"] = Overlap;
			dr["Boost"] = Boost;
			dr["ActiveDutyCycle"] = ActiveDutyCycle;
			dr["OverlapDutyCycle"] = OverlapDutyCycle;
		}

		#endregion
		



		#region Statistics

		/// <summary>
		/// Computes statistics on column level from acumulated cells. 
		/// The parameters are updated by the cells stepping.
		/// </summary>
		public void ComputeBasicStatistics()
		{
			this.Statistics.StepCounter++;

			this.Region.Statistics.SegmentPredictionCounter += this.Statistics.SegmentPredictionCounter;
			this.Region.Statistics.CorrectSegmentPredictionCounter += this.Statistics.CorrectSegmentPredictionCounter;
			this.Region.Statistics.PredictionCounter += this.Statistics.PredictionCounter;
			this.Region.Statistics.CorrectPredictionCounter += this.Statistics.CorrectPredictionCounter;

			// Column activity
			if (this.ActiveState[Global.T])
			{
				this.Statistics.ColumnActivityCounter++;
				this.Region.Statistics.ActivityCounter += this.Statistics.ColumnActivityCounter;
			}

			this.Statistics.ActivityRate = this.Statistics.ColumnActivityCounter / this.Statistics.StepCounter;

			// Basics
			if (this.Statistics.SegmentPredictionCounter > 0)
			{
				this.Statistics.PredictPrecision = this.Statistics.CorrectSegmentPredictionCounter / this.Statistics.SegmentPredictionCounter;
			}

			this.Statistics.ActivityPrecision = this.Statistics.CorrectSegmentPredictionCounter / this.Statistics.ActivityCounter;

			// Reset the values:
			this.Statistics.SegmentPredictionCounter = 0;
			this.Statistics.CorrectSegmentPredictionCounter = 0;
			this.Statistics.PredictionCounter = 0;
			this.Statistics.CorrectPredictionCounter = 0;

			// Temporary lists
			var listMaxCellActivityCounter = new List<float>();
			var listMaxCorrectPredictionCounter = new List<float>();
			var listMaxLearningCounter = new List<float>();
			var listMaxNumberSegments = new List<float>();
			var listMaxPredictionCounter = new List<float>();
			var listMaxNumberSynapses = new List<float>();

			// Get Max-Values from Cells
			foreach (var cell in this.Cells)
			{
				listMaxCellActivityCounter.Add(cell.Statistics.ActivityCounter);
				listMaxCorrectPredictionCounter.Add(cell.Statistics.CorrectPredictionCounter);
				listMaxLearningCounter.Add(cell.Statistics.LearningCounter);
				listMaxNumberSegments.Add(cell.Statistics.NumberSegments);
				listMaxPredictionCounter.Add(cell.Statistics.PredictionCounter);
				listMaxNumberSynapses.Add(cell.Statistics.MaxNumberSynapses);
			}

			// Sort the lists
			listMaxCellActivityCounter.Sort();
			listMaxCorrectPredictionCounter.Sort();
			listMaxLearningCounter.Sort();
			listMaxNumberSegments.Sort();
			listMaxPredictionCounter.Sort();
			listMaxNumberSynapses.Sort();

			// Get Max values:
			this.Statistics.MaxCellActivityCounter = listMaxCellActivityCounter.Last();
			this.Statistics.MaxCorrectPredictionCounter = listMaxCorrectPredictionCounter.Last();
			this.Statistics.MaxLearningCounter = listMaxLearningCounter.Last();
			this.Statistics.MaxNumberSegments = listMaxNumberSegments.Last();
			this.Statistics.MaxPredictionCounter = listMaxPredictionCounter.Last();
			this.Statistics.MaxNumberSynapses = listMaxNumberSynapses.Last();
		}

		#endregion

#if DEBUG
		public override string ToString()
		{
			return "[" + this.PositionInRegion.X.ToString() + "," + this.PositionInRegion.Y.ToString()
			       + "]: Overlap=" + this.Overlap + " S=" + this.Statistics.NumberSegments + (this.ActiveState[Global.T] ? " A" : " ") +
			       (this.InhibitedState[Global.T] ? "I" : "");
		}
#endif

		#endregion
	}
}
