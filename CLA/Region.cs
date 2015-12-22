using System;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using OpenHTM.CLA.Extensions;
using OpenHTM.CLA.Statistics;
//using OpenHTM.Shared.Interfaces;

namespace OpenHTM.CLA
{
	/// <summary>
	/// Represents an entire region of columns for the CLA.
	/// </summary>
	/// <remarks>
	/// Code to represent an entire Hierarchical Temporal Memory (HTM) Region of 
	/// <see cref="Column"/>s that implement Numenta's new Cortical Learning Algorithms 
	/// (CLA). 
	/// The Region is defined by a matrix of columns, each of which contains several cells.  
	/// The main idea is that given a matrix of input bits, the Region will first sparsify 
	/// the input such that only a few Columns will become 'active'.  As the input matrix 
	/// changes over time, different sets of Columns will become active in sequence.  
	/// The Cells inside the Columns will attempt to learn these temporal transitions and 
	/// eventually the Region will be able to make predictions about what may happen next
	/// given what has happened in the past. 
	/// SpatialPooling snippet from the Numenta docs:
	/// The code computes activeColumns(t) = the list of columns that win due to 
	/// the bottom-up input at time t. This list is then sent as input to the 
	/// temporal pooler routine.
	/// Phase 1: compute the overlap with the current input for each column
	/// Phase 2: compute the winning columns after inhibition
	/// Phase 3: update synapse permanence and internal variables
	/// 
	/// 1) Start with an input consisting of a fixed number of bits. These bits might 
	///    represent sensory data or they might come from another region lower in the 
	///    hierarchy.
	/// 2) Assign a fixed number of columns to the region receiving this input. Each 
	///    column has an associated dendrite segmentUpdateList. Each dendrite 
	///    segmentUpdateList has a set of potential synapses representing a subset of the 
	///    input bits. Each potential synapse has a permanence value.
	///    Based on their permanence values, some of the potential synapses will be valid.
	/// 3) For any given input, determine how many valid synapses on each column are 
	///    connected to active input bits.
	/// 4) The number of active synapses is multiplied by a 'boosting' factor which is 
	///    dynamically determined by how often a column is active relative to its neighbors. 
	/// 5) The columns with the highest activations after boosting disable all but a fixed 
	///    percentage of the columns within an inhibition radius. The inhibition radius is 
	///    itself dynamically determined by the spread (or 'fan-out') of input bits. 
	///    There is now a sparse set of active columns.
	/// 6) For each of the active columns, we adjust the permanence values of all the 
	///    potential synapses. The permanence values of synapses aligned with active input 
	///    bits are increased. The permanence values of synapses aligned with inactive 
	///    input bits are decreased. The changes made to permanence values may change 
	///    some synapses from being valid to not valid, and vice-versa.
	/// </remarks>
	[Serializable]
	public class Region : INode, IWatchItem
	{
		#region Fields


		/// <summary>
		/// Region Index in List.
		/// </summary>
		public int Index = 0;
		/// <summary>
		/// Statistics used for check accuracy of predictions.
		/// </summary>
		/// 		[XmlIgnore]
		[XmlIgnore]
		public StatisticsRegion Statistics = new StatisticsRegion();

		#endregion

		#region Properties

		/// <summary>
		/// A higher region in hierarchy which this region will feed-forward it.
		/// </summary>
		[XmlIgnore]
		public Region ParentRegion { get; set; }

		/// <summary>
		/// All lower regions/sensors in hierarchy which this region will receive feed-forward input.
		/// </summary>
		[XmlIgnore]
		public List<INode> Children = new List<INode>();

		/// <summary>
		/// The size of the Region's column grid.  If the Region has a defined
		/// Column grid of 100x50 then the Width is 100 and the Height is 50.
		/// </summary>
		public Size Size { get; set; }

		/// <summary>
		/// Indicates if the region was initialized or not.
		/// </summary>
		public bool Initialized { get; set; }

		/// <summary>
		/// The size of the Region's data input grid.  If the Region has a defined
		/// data input grid of 50x50 then the input width is 50 and height is 50.
		/// </summary>
		//public Size InputSize { get; private set; }
		public Size InputSize { get; set; }

		/// <summary>
		/// A reference to the input data array used during the most recently processed
		/// region time step. Input[t][x, y] is 1 if the value in [x,y] position from the
		/// input vector is on.
		/// </summary>
		/// 
		[XmlIgnore]
		public List<int[,]> Input { get; set; }

		/// <summary>
		/// Percentage of input that a column can represent. I.e. percentage of input bits
		/// each Column will have potential proximal (spatial) synapses for.
		/// </summary>
		//internal float PercentageInputPerColumn { get; private set; }
		public float PercentageInputPerColumn { get; set; }

		/// <summary>
		/// The conversion factor to get from Region column grid space back to
		/// original input grid space.  For example if the Region has a column
		/// grid of 50x50 and the input grid is size 100x100 then the input proportion
		/// to X would be 2 since the input X space is twice as long as the grid X space.
		/// </summary>
		public float InputProportionX { get; set; }

		/// <summary>
		/// The conversion factor to get from Region column grid space back to
		/// original input grid space.  For example if the Region has a column
		/// grid of 50x50 and the input grid is size 100x100 then the input proportion
		/// to Y would be 2 since the input Y space is twice as long as the grid Y space.
		/// </summary>
		public float InputProportionY { get; set; }

		/// <summary>
		/// Contains an array of all columns in this <see cref="Region"/>.
		/// </summary>
		//public List<Column> Columns { get; private set; }
		[XmlIgnore]
		public List<Column> Columns { get; set; }

		/// <summary>
		/// The number of context learning cells per column.
		/// </summary>
		//public int CellsPerColumn { get; private set; }
		public int CellsPerColumn { get; set; }

		/// <summary>
		/// Approximate percentage of Columns within average receptive field radius to be 
		/// winners after spatial pooling inhibition.
		/// </summary>
		//internal float PercentageLocalActivity { get; private set; }
		internal float PercentageLocalActivity { get; set; }

		/// <summary>
		/// This variable determines the desired amount of columns to be activated within a
		/// given spatial pooling inhibition radius.  For example if the InhibitionRadius is
		/// 3 it means we have 6x6 grid of columns under local consideration.  During inhibition
		/// we need to decide which columns in this local area are to be activated.  If 
		/// DesiredLocalActivity is 5 then we take only the 5 most strongly activated columns
		/// within the 6x6 local grid.
		/// </summary>
		//public int DesiredLocalActivity { get; private set; }
		public int DesiredLocalActivity { get; set; }

		/// <summary>
		/// This radius determines how many columns in a local area are considered during
		/// spatial pooling inhibition.  
		/// </summary>
		//public float InhibitionRadius { get; private set; }
		public float InhibitionRadius { get; set; }

		/// <summary>
		/// Furthest number of columns away (in column grid space) to allow new distal 
		/// synapse connections. If set to 0 then there is no restriction and connections
		/// can form between any two columns in the region.
		/// </summary>
		//internal int LocalityRadius { get; private set; }
		internal int LocalityRadius { get; set; }

		/// <summary>
		/// The number of new distal synapses added to segments if no matching ones are found 
		/// during learning.
		/// </summary>
		//internal int NumberNewSynapses { get; private set; }
		internal int NumberNewSynapses { get; set; }

		/// <summary>
		/// Minimum percentage of column's proximal synapses that must be active for column 
		/// to be considered during spatial pooling inhibition. This value helps set the
		/// minimum column overlap value during region run.
		/// </summary>
		//internal float PercentageMinOverlap { get; private set; }
		internal float PercentageMinOverlap { get; set; }

		/// <summary>
		/// A threshold number of active synapses between active and non-active 
		/// <see cref="Segment"/> state.
		/// Original name: activationThreshold
		/// </summary>
		//internal int SegmentActiveThreshold { get; private set; }
		internal int SegmentActiveThreshold { get; set; }

		/// <summary>
		/// If set to <c>true</c>, the <see cref="Region"/> will assume the <see cref="Column"/>
		/// grid dimensions exactly matches that of the input data.
		/// </summary>
		/// <remarks>The hardcoded flag is something Barry added to bypass the spatial 
		/// pooler for exclusively working with the temporal sequences. It is more than
		/// just disabling spatial learning. In this case the Region will assume the 
		/// column grid dimensions exactly matches that of the input data. Then on each
		/// time step no spatial pooling is performed, instead we assume the input data
		/// is itself describing the state of the active columns. Think of hardcoded as
		/// though the spatial pooling has already been performed outside the network and
		/// we are just passing in the results to the columns as-is.</remarks>
		public bool HardcodedSpatial { get; set; }

		/// <summary>
		/// If true then all proximal (spatial) synapses will be defaulted to full connection
		/// permanence strength.  If false, synapses default to normal distribution around
		/// connection threshold based on nearest corresponding input bits.
		/// </summary>
		/// <remarks>
		/// When enabled this helps in Regions where negative examples are far more
		/// common allowing unused synapses to gradually become unconnected while providing
		/// an easier chance for initial inputs to immediately activate relevant columns
		/// (rather than waiting for boosting.  This flag only has effect when enabled
		/// before Region construction (via passing to the constructor).
		/// </remarks>
		/// <remarks>Lays within (0; 1] interval.</remarks>
		//public bool FullDefaultSpatialPermanence { get; private set; }
		public bool FullDefaultSpatialPermanence { get; set; }

		/// <summary>
		/// Stores options that configure the operation of Parallel.For loops.
		/// </summary>
		//public static ParallelOptions ParallelOptions { get; private set; }
		[XmlIgnore]
		public static ParallelOptions ParallelOptions { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the <see cref="Region"/> class. Allows debugging without parallel loops.
		/// </summary>
		static Region()
		{
#if DEBUG
			ParallelOptions = new ParallelOptions() {MaxDegreeOfParallelism = 1};
#else
			ParallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = -1 };
#endif
		}

		public Region()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="Region"/> class.
		/// </summary>
		/// <param name="index">Region Index. </param>
		/// <param name="parentRegion">Parent region. </param>
		/// <param name="size">System.Drawing.Size structure that represents the 
		///     region size in columns (columns in X axis * columns in Y axis).</param>
		/// <param name="percentageInputPerColumn">The percentage of input bits each column has potential
		///     synapses for (PctInputCol).</param>
		/// <param name="percentageMinOverlap">The minimum percentage of column's proximal synapses
		///     that must be active for a column to be even be considered during spatial 
		///     pooling.</param>
		/// <param name="localityRadius">Furthest number of columns away to allow
		///     proximal+distal synapses from each column/cell.</param>
		/// <param name="percentageLocalActivity">Approximate percentage of Columns within locality
		///     radius to be winners after spatial inhibition.</param>
		/// <param name="cellsPerCol">The number of context learning cells per column.</param>
		/// <param name="segmentActiveThreshold">The minimum number of synapses that must be 
		///     active for a segment to fire.</param>
		/// <param name="numberNewSynapses">The number of new distal synapses added if
		///     no matching ones found during learning.</param>
		/// <param name="fullDefaultSpatialPermanence">If true then all proximal (spatial) 
		/// synapses will be defaulted to full connection permanence strength.  If false, 
		/// synapses default to Normal distribution around connection threshold based on 
		/// nearest corresponding input bits.</param>
		
		//debug js
		public Region(int index, Region parentRegion, Size size, float percentageInputPerColumn, float percentageMinOverlap, int localityRadius, float percentageLocalActivity, int cellsPerCol, int segmentActiveThreshold, int numberNewSynapses, bool fullDefaultSpatialPermanence = false)
		//public Region(int index, Region parentRegion, Size size, float percentageInputPerColumn, float percentageMinOverlap, int localityRadius, float percentageLocalActivity, int cellsPerCol, int segmentActiveThreshold, int numberNewSynapses, bool fullDefaultSpatialPermanence = true)
		{

			// Set fields
			this.Index = index;
			this.ParentRegion = parentRegion;
			this.Columns = new List<Column>();
			this.CellsPerColumn = cellsPerCol;
			this.LocalityRadius = localityRadius;
			this.SegmentActiveThreshold = segmentActiveThreshold;
			this.NumberNewSynapses = numberNewSynapses;
			if (!this.HardcodedSpatial)
			{
				this.Size = size;
				this.PercentageInputPerColumn = percentageInputPerColumn;
				this.PercentageMinOverlap = percentageMinOverlap;
				this.PercentageLocalActivity = percentageLocalActivity;
				this.FullDefaultSpatialPermanence = fullDefaultSpatialPermanence;
			}

			// If this region is child of an higher region in the hierarchy then updates the set of child regions of the latter
			if (this.ParentRegion != null)
			{
				this.ParentRegion.Children.Add(this);
			}

			// Initialize input vectors with fixed lenght = T
			this.Input = new List<int[,]>();
			for (int t = 0; t <= Global.T; t++)
			{
				this.Input.Add(null);
			}
		}


		#endregion

		#region Methods

		/// <summary>
		/// Prior to receiving any inputs, the region is initialized by computing a list of initial potential synapses for each column.
		/// This consists of a random set of inputs selected from the input space. Each input is represented by a synapse and assigned a random permanence value.
		/// The random permanence values are chosen with two criteria.
		/// First, the values are chosen to be in a small range around connectedPerm (the minimum permanence value at which a synapse is considered "connected").
		/// This enables potential synapses to become connected (or disconnected) after a small number of training iterations.
		/// Second, each column has a natural center over the input region, and the permanence values have a bias towards this center (they have higher values near the center).
		/// </summary>
		/// <remarks> 
		/// In addition to this Uwe added a concept of Locality Radius, which is an 
		/// additional parameter to control how far away synapse connections can be made 
		/// instead of allowing connections anywhere.  The reason for this is that in the
		/// case of video images I wanted to experiment with forcing each Column to only
		/// learn on a small section of the total input to more effectively learn lines or 
		/// corners in a small section without being 'distracted' by learning larger patterns
		/// in the overall input space (which hopefully higher hierarchical Regions would 
		/// handle more successfully).  Passing in 0 for locality radius will mean no 
		/// restriction which will more closely follow the Numenta doc if desired.
		/// </remarks>
		public void Initialize()
		{
			Segment.ActivationThreshold = SegmentActiveThreshold;

			// Loop over all lower regions and sensors to calculate the total size of the input
			// All regions/sensors should the same dimensions
			var inputSize = new Size();
			foreach (var child in this.Children)
			{
				inputSize.Width += child.Size.Width;
				inputSize.Height += child.Size.Height;
			}
			this.InputSize = inputSize;

			// Hard-coded means that input bits are mapped directly to columns.  In other words the 
			// normal spatial pooler is disabled and we instead assume the input sparsification
			// has already been decided by some preprocessing code outside the Region.
			// It is then assumed (though not checked) that the input array will have
			// only a sparse number of "1" values that represent the active columns
			// for each time step.
			if (this.HardcodedSpatial)
			{
				this.Size = this.InputSize;
			}

			// Calculate the conversion factor to get from region's column grid back to original input grid
			// original
			//this.InputProportionX = (this.InputSize.Width - 1) / ((float) this.Size.Width - 1);
			//this.InputProportionY = (this.InputSize.Height - 1) / ((float) this.Size.Height - 1);
			// JS
			this.InputProportionX = (float)this.InputSize.Width / (float)this.Size.Width;
			this.InputProportionY = (float)this.InputSize.Height / (float)this.Size.Height;
			

			// Create the columns based on the size of the region and input data to connect to
			for (int x = 0; x < this.Size.Width; x++)
			{
				for (int y = 0; y < this.Size.Height; y++)
				{
					// Position of this column in the region grid
					var positionInRegion = new Point(x, y);

					// Calculate the conversion factor to get from region's column grid back to original input grid
					var inputPositionX = (int) Math.Round(positionInRegion.X * this.InputProportionX);
					var inputPositionY = (int) Math.Round(positionInRegion.Y * this.InputProportionY);

					// Set 'center' position of columns in the original input grid
					var centralPositionInInput = new Point(inputPositionX, inputPositionY);

					// Create new column and add to region's list
					var column = new Column(this, centralPositionInInput, positionInRegion);

					this.Columns.Add(column);

					//if (diag)
					//{
					//	file.WriteLine ( positionInRegion.X + "," + positionInRegion.Y + ",," + centralPositionInInput.X + "," + centralPositionInInput.Y );
					//}
				}
			}

			// With hardcoded the Region will create a matching number of Columns to
			// mirror the size of the input array. Locality radius may still be
			// defined as it is still used by the temporal pooler.  If non-zero it will
			// restrict temporal segments from connecting further than r number of
			// columns away.
			if (this.HardcodedSpatial)
			{
				this.PercentageInputPerColumn = 1.0f / this.Columns.Count;
				this.PercentageMinOverlap = 1.0f;
				this.PercentageLocalActivity = 1.0f;
				this.DesiredLocalActivity = 1;
			}
			else
			{
				// Create Segments with potential synapses for columns
				foreach (var column in this.Columns)
				{
					column.CreateProximalSegments();
				}

				// Inhibition radius is recomputed
				this.InhibitionRadius = this.AverageReceptiveFieldSize();

				// Set the desired local activity, ie the number of columns that will be
				// activated within a given spatial pooling inhibition radius
				if (this.LocalityRadius == 0)
				{
					this.DesiredLocalActivity = (int) Math.Round(this.InhibitionRadius * this.PercentageLocalActivity);
				}
				else
				{
					this.DesiredLocalActivity = (int) (Math.Pow(this.LocalityRadius, 2) * this.PercentageLocalActivity);
				}
				this.DesiredLocalActivity = Math.Max(2, this.DesiredLocalActivity);

			}
			
			Write (0);

		}

		/// <summary>
		/// Perfoms actions related to time step progression.
		/// </summary>
		public void NextTimeStep ()
		{
			// Remove the element in first position in order to the input vector always have length = T
			// and add a new element to represent the input at current time step
			this.Input.RemoveAt(0);
			this.Input.Add(null);

			// If this region is parent of lower regions/sensors in the hierarchy then 
			// first perfom actions related to time step progression of these. This makes
			// that the lower regions are recursively fed and their outputs are 
			// feed-forward back to their parent region
			if (this.Children.Count > 0)
			{
				foreach (var child in this.Children)
				{
					child.NextTimeStep();
				}
			}

			// If region was not initialized, then compute a list of initial potential synapses for each column.
			if (!this.Initialized)
			{
				this.Initialize();
				this.Initialized = true;
			}

			// Reset at every step.
			this.Statistics.ActivityCounter = 0f;

			// Loop columns in order to perfom actions related to time step progression
			foreach (var column in this.Columns)
			{
				column.NextTimeStep();

				// Reset Selection of DataGrid-Columns:
				column.IsDataGridSelected = false;
			}

			// Compute Region statistics
			this.ComputeBasicStatistics();

			// Perform pooling
			this.PerformSpatialPooling();
			this.PerformTemporalPooling();

			// Column accuracies must be run after the processing finishes
			// TODO: should all statistics be run after region processing?
			this.ComputeColumnAccuracy();
			this.ComputeNumberActiveColumns();

			// If this region is child of an higher region in the hierarchy then 
			// feedforward input to the latter
			if (this.ParentRegion != null)
			{
				this.ParentRegion.SetInput(this.GetOutput());
			}
		}

		/// <summary>
		/// Set the input bit-matrix of the most recently run time step for this 
		/// <see cref="Region"/>.
		/// The input is an array of bottom-up binary inputs from sensory data or the 
		/// previous level.
		/// </summary>
		/// <param name="input">2d Array used for next <see cref="Region"/> time step.</param>
		public void SetInput(int[,] input)
		{
			this.Input[Global.T] = input;
		}

		/// <summary>
		/// Determine the output bit-matrix of the most recently run time step for this 
		/// <see cref="Region"/>.
		/// </summary>
		/// <returns> A 2d int array of same shape as the column grid containing the 
		/// <see cref="Region"/>'s collective output.
		/// </returns>
		/// <remarks>
		/// The <see cref="Region"/> output is a 2d array representing all columns present in 
		/// the <see cref="Region"/>. Bits are set to 1 if a Column is active or it contains at 
		/// least 1 predicting cell, all other bits are 0. The output data will be a 2d array 
		/// of dimensions corresponding the column grid for this <see cref="Region"/>.  
		/// Note: the Numenta doc suggests the <see cref="Region"/> output should potentially 
		/// include bits for each individual cell.  My first-pass implementation is Column only for 
		/// now since in the case or 2 or 3 cells, the spatial positioning of the original grid 
		/// shape can become lost and I'm not sure yet how desirable this is or isn't for the case 
		/// of video input).
		/// </remarks>
		public int[,] GetOutput()
		{
			var output = new int[this.Size.Width,this.Size.Height];

			// Loop over all columns setting each output element to 1 if respective cell 
			// in x,y position is active or is predicting
			foreach (var column in this.Columns)
			{
				foreach (var cell in column.Cells)
				{
					if (cell.ActiveState[Global.T] || cell.PredictiveState[Global.T])
					{
						// TODO: Since active and predicting cells are equally 
						// marked as 1, how the higher region will know what is real and 
						// what can or not happen?
						// Maybe, we should separate these matrices and present at appropriate time steps
						output[column.PositionInRegion.X, column.PositionInRegion.Y] = 1;
					}
				}
			}

			return output;
		}

		/// <summary>
		/// Performs spatial pooling for the current input in this <see cref="Region"/>.
		/// </summary>
		/// <remarks>
		/// The input to this code is an array of bottom-up binary inputs from sensory data
		/// or the previous level.  The code computes the list of columns that win due to
		/// the bottom-up input at time t.  This list is then sent as input to the temporal 
		/// pooler, i.e. getActiveColumns(t) returns the output of the spatial pooling routine.
		/// Thus, the result will be a subset of <see cref="Column"/>s being set as active 
		/// as well as (proximal) synapses in all Columns having updated permanences and boosts,
		/// and the <see cref="Region"/> will update inhibitionRadius.
		/// </remarks>
		private void PerformSpatialPooling()
		{
			// With hardcoded the Region will create a matching number of Columns to
			// mirror the size of the input array and bypass spatial pooling
			if (this.HardcodedSpatial)
			{
				if (this.Columns.Count != this.Input[Global.T].Length)
				{
					throw new Exception("Input and region sizes don't match!");
				}

				// Set each column active state to the correspondent value in input
				Parallel.ForEach(this.Columns, ParallelOptions, column => { column.ActiveState[Global.T] = this.Input[Global.T][column.PositionInRegion.X, column.PositionInRegion.Y] == 1; });

				// Exit without perform spatial pooling
				return;
			}

			// Phase 1: compute the overlap with the current input for each column
			this.SpatialPoolerPhase1();

			// Phase 2: compute active columns, i.e. the winning columns after inhibition
			this.SpatialPoolerPhase2();

			// Phase 3: perform synapse boosting (learning), i.e. update synapse permanence
			// and internal variables
			// Although spatial pooler learning is inherently online, you can turn off 
			// learning by simply skipping this phase.
			if (Global.SpatialLearning)
			{
				this.SpatialPoolerPhase3();
			}
		}

		/// <summary>
		/// Spatial Pooler Phase 1: Compute the overlap with the current input for each column.
		/// </summary>
		/// <remarks>
		/// Given an input vector, the first phase calculates the overlap of each column with
		/// that vector.
		/// The overlap for each column is simply the number of connected synapses with 
		/// active inputs, multiplied by its boost.
		/// If this value is below minOverlap, we set the overlap score to zero.
		/// 
		/// Attention: refactored regarding MinOverlap from column: overlap is now computed as 
		/// the former overlap per area as this will make areas with inequal size comparable.
		/// </remarks>
		private void SpatialPoolerPhase1()
		{
			// Compute the overlap with the current input for each column.
			Parallel.ForEach(this.Columns, ParallelOptions, column =>
			{
				// Determine if this segment is active if enough active synapses are present
				column.ProximalSegment.ProcessSegment();

				// Calculates the overlap of the column with input vector.
				// "overlap" is the current number of active and connnected synapses
				int overlap = column.ProximalSegment.GetActiveConnectedSynapses(Global.T).Count;

				// The overlap for the column is simply the number of connected synapses 
				// with active inputs, multiplied by its boost.
				// If this value is below minOverlap, we set the overlap score to zero.
				if (overlap < column.MinOverlap)
				{
					overlap = 0;
				}
				else
				{
					overlap = (int) Math.Round(overlap * column.Boost);
				}

				column.Overlap = overlap;
			});
		}

		/// <summary>
		/// Spatial Pooler Phase 2: Compute the winning columns after inhibition.
		/// </summary>
		/// <remarks>
		/// The second phase calculates which columns remain as winners after the 
		/// inhibition step.
		/// desiredLocalActivity is a parameter that controls the number of columns that 
		/// end up winning.
		/// For example, if desiredLocalActivity is 10, a column will be a winner if its 
		/// overlap score is greater than the score of the 10'th highest column within its
		/// inhibition radius.
		/// In other words, compute whether or not this Column will be active after the 
		/// effects of local inhibition are applied.  A Column must have overlap greater 
		/// than 0 and have its overlap value be within the k'th largest of its neighbors 
		/// (where k = desiredLocalActivity).
		/// </remarks>
		private void SpatialPoolerPhase2()
		{
			// Calculates which columns remain as winners after the inhibition step.
			Parallel.ForEach(this.Columns, ParallelOptions, column =>
			{
				column.ActiveState[Global.T] = false;
				column.InhibitedState[Global.T] = false;

				if (column.Overlap > 0)
				{
					// A column will be a winner if its overlap score is greater than the score
					// of the k'th highest column within its inhibition radius.
					if (this.IsWithinKthScore(column, this.DesiredLocalActivity))
					{
						column.ActiveState[Global.T] = true;
					}
					else
					{
						column.InhibitedState[Global.T] = true;
					}
				}
			});
		}

		/// <summary>
		/// Spatial Pooler Phase 3: Update synapse permanence and internal variables.
		/// </summary>
		/// <remarks>
		/// The third phase performs learning; it updates the permanence values of all 
		/// synapses as necessary, as well as the boost and inhibition radius.
		/// For winning columns, if a synapse is active, its permanence value is incremented, 
		/// otherwise it is decremented. Permanence values are constrained to be between 0 and 1.
		/// There are two separate boosting mechanisms in place to help a column learn connections: 
		///   1. If a column does not win often enough (as measured by column.ActiveDutyCycle), 
		///      its overall boost value is increased. Alternatively, 
		///   2. If a column's connected synapses do not overlap well with any inputs often enough 
		///      (as measured by column.OverlapDutyCycle), its permanence values are boosted. 
		/// 
		/// Note: once learning is turned off, column.Boost is frozen.
		/// Finally, at the end of Phase 3 the inhibition radius is recomputed.
		/// </remarks>
		private void SpatialPoolerPhase3()
		{
			// Performs learning; it updates the permanence values of all synapses as 
			// necessary, as well as the boost and inhibition radius.
			Parallel.ForEach(this.Columns, ParallelOptions, column =>
			{
				if (column.ActiveState[Global.T])
				{
					// The main learning rule is implemented in these lines.			
					foreach (var proximalSynapse in column.ProximalSegment.Synapses)
					{
						// For winning columns, if a synapse is active, its permanence value 
						// is incremented, otherwise it is decremented. 
						// Permanence values are constrained to be between 0 and 1.
						// (Even disconnected synapses can be increased in this case)
						if (proximalSynapse.IsActive(Global.T))
						{
							proximalSynapse.IncreasePermanence();
						}
						else
						{
							proximalSynapse.DecreasePermanence();
						}
					}
				}

				// minDutyCycle is a variable representing the minimum desired firing rate
				// for a cell. If a cell's firing rate falls below this value, it will be
				// boosted. This value is calculated as 1% of the maximum firing rate of
				// its neighbors.
				float minDutyCycle = 0.01f * column.GetMaxDutyCycle();

				// 1. If a column does not win often enough (as measured by column.ActiveDutyCycle), 
				//    its overall boost value is increased.
				{
					// Note: once learning is turned off, column.Boost is frozen.
					column.ActiveDutyCycle = column.UpdateActiveDutyCycle();
					if (column.ActiveDutyCycle > minDutyCycle)
					{
						column.Boost = 1.0f;
					}
					else if (column.ActiveDutyCycle <= 0.0f)
					{
						// Fix at +5% if activeDutyCycle all the way to 0 so we can force at least
						// some boosting.
						column.Boost *= 1.05f;
					}
					else
					{
						column.Boost = minDutyCycle / column.ActiveDutyCycle;
					}
				}

				// 2. If a column's connected synapses do not overlap well with any inputs often enough 
				//    (as measured by column.OverlapDutyCycle), its permanence values are boosted. 
				{
					column.OverlapDutyCycle = column.UpdateOverlapDutyCycle();
					if (column.OverlapDutyCycle < minDutyCycle)
					{
						foreach (var synapse in column.ProximalSegment.Synapses)
						{
							synapse.IncreasePermanence();
						}
					}
				}
			});

			// Finally, the inhibition radius is recomputed
			this.InhibitionRadius = this.AverageReceptiveFieldSize();
		}

		/// <summary>
		/// Performs temporal pooling based on the current spatial pooler output.
		/// </summary>
		/// <remarks>
		/// The input to this code is return of getActiveColumns(t), as computed by the spatial pooler.
		/// The code computes the active and predictive state for each cell at the current timestep, t.
		/// The boolean OR of the active and predictive states for each cell forms the 
		/// output of the temporal pooler for the next level.
		/// 
		/// Phase 3 is only required for learning. However, unlike spatial pooling, 
		/// Phases 1 and 2 contain some learning-specific operations when learning is turned on.
		/// Since temporal pooling is significantly more complicated than spatial pooling, 
		/// we first separate the inference-only version of the temporal pooler, 
		/// followed by a version that combines inference and learning.
		/// A description of some of the implementation details, terminology, and supporting routines
		/// are at the end of the chapter, after the pseudocode.
		/// 
		/// Cells maintain a list of dendrite segments, where each segment contains a list 
		/// of synapses plus a permanence value for each synapse.
		/// Changes to a cell's synapses are marked as temporary until the cell becomes 
		/// active from feed-forward input.
		/// These temporary changes are maintained in SegmentUpdateList.
		/// Each segment also maintains a boolean flag, IsSequenceSegment, indicating 
		/// whether the segment predicts feed-forward input on the next time step.
		/// 
		/// The implementation of potential synapses is different from the implementation 
		/// in the spatial pooler. In the spatial pooler, the complete list of potential 
		/// synapses is represented as an explicit list. In the temporal pooler, each segment
		/// can have its own (possibly large) list of potential synapses.
		/// In practice maintaining a long list for each segment is computationally 
		/// expensive and memory intensive.
		/// Therefore in the temporal pooler, we randomly add active synapses to each 
		/// segment during learning (controlled by the parameter newSynapseCount).
		/// This optimization has a similar effect to maintaining the full list of 
		/// potential synapses, but the list per segment is far smaller while still 
		/// maintaining the possibility of learning new temporal patterns.
		/// 
		/// The pseudocode also uses a small state machine to keep track of the cell 
		/// states at different time steps. We maintain three different states for each cell.
		/// The arrays ActiveState and PredictiveState keep track of the active and 
		/// predictive states of each cell at each time step.
		/// The array LearnState determines which cell outputs are used during learning.
		/// When an input is unexpected, all the cells in a particular column become 
		/// active in the same time step.
		/// Only one of these cells (the cell that best matches the input) has its 
		/// LearnState turned on.
		/// We only add synapses from cells that have LearnState turned on (this avoids 
		/// over representing a fully active column in dendritic segments).
		/// </remarks>
		private void PerformTemporalPooling()
		{
			// Phase 1: compute the active state, ActiveState[t], for each cell
			this.TemporalPoolerPhase1();

			// Phase 2: compute the predicted state, PredictiveState[t], for each cell
			this.TemporalPoolerPhase2();

			// Phase 3: update synapses
			if (Global.TemporalLearning)
			{
				this.TemporalPoolerPhase3();
			}
		}

		/// <summary>
		/// Temporal Pooler Phase 1: Compute the active state, ActiveState[t], for each cell.
		/// </summary>
		/// <remarks>
		/// The first phase calculates the ActiveState for each cell that is in a winning 
		/// column. For those columns, the code further selects one cell per column as 
		/// the learning cell (LearnState).
		/// The logic is as follows: if the bottom-up input was predicted by any cell 
		/// (i.e. its PredictiveState output was true due to a sequence segment), then 
		/// those cells become active.
		/// If that segment became active from cells chosen with LearnState on, this cell 
		/// is selected as the learning cell.
		/// If the bottom-up input was not predicted, then all cells in the column become active.
		/// In addition, the best matching cell is chosen as the learning cell and a new 
		/// segment is added to that cell.
		/// </remarks>
		private void TemporalPoolerPhase1()
		{
			// Calculates the ActiveState for each cell that is in a winning column.
			// For those columns, the code further selects one cell per column as the 
			// learning cell (LearnState).
			Parallel.ForEach(this.Columns, ParallelOptions, column =>
			{
				if (column.ActiveState[Global.T])
				{
					bool bottomupPredicted = false;
					bool learnCellChosen = false;

					foreach (var cell in column.Cells)
					{
						// If the bottom-up input was predicted by any cell (i.e. its 
						// PredictiveState output was true due to a sequence segment), 
						// then those cells become active.
						DistalSegment distalSegment = cell.GetSequencePredictingDistalSegment();
						if (distalSegment != null)
						{
							bottomupPredicted = true;
							cell.ActiveState[Global.T] = true;

							// If that segment became active from cells chosen with 
							// LearnState on, this cell is selected as the learning cell.
							if (Global.TemporalLearning && distalSegment.WasActiveFromLearning())
							{
								learnCellChosen = true;
								cell.LearnState[Global.T] = true;
							}
						}
					}

					// If the bottom-up input was not predicted, then all cells in the become active.
					if (!bottomupPredicted)
					{
						foreach (var cell in column.Cells)
						{
							cell.ActiveState[Global.T] = true;
						}
					}

					// The best matching cell is chosen as the learning cell and a new segment is added to that cell.
					if (Global.TemporalLearning && !learnCellChosen)
					{
						// isSequence=true, time step=t-1
						Tuple<Cell, DistalSegment> tupleCellSegment = column.GetBestMatchingCell(Global.T - 1, 1);
						Cell bestCell = tupleCellSegment.Item1;
						DistalSegment distalSegment = tupleCellSegment.Item2;
						bestCell.LearnState[Global.T] = true;

						Debug.Assert(bestCell != null, "Assert bestCell != null failed. We need a bestCell here. Help me out.");

						// segmentUpdate is added internally to Cell's update list.
						// then set number of prediction steps to 1 (meaning it's a sequence segment)
						SegmentUpdate segmentUpdate = bestCell.UpdateDistalSegmentActiveSynapses(Global.T - 1, distalSegment, true);
						segmentUpdate.NumberPredictionSteps = 1;
					}
				}
			});
		}

		/// <summary>
		/// Temporal Pooler Phase 2: Compute the predicted state, PredictiveState[t], 
		/// for each cell.
		/// </summary>
		/// <remarks>
		/// The second phase calculates the predictive state for each cell.
		/// A cell will turn on its predictive state output if one of its segments 
		/// becomes active, i.e. if enough of its lateral inputs are currently active 
		/// due to feed-forward input.
		/// In this case, the cell queues up the following changes:
		///   a) reinforcement of the currently active segment, and
		///   b) reinforcement of a segment that could have predicted this activation,
		///      i.e. a segment that has a (potentially weak) match to activity during 
		///      the previous time step.
		/// </remarks>
		private void TemporalPoolerPhase2()
		{
			// Calculates the predictive state for each cell.
			Parallel.ForEach(this.Columns, ParallelOptions, column =>
			{
				foreach (var cell in column.Cells)
				{
					// Process all segments on the cell to cache the activity for later
					foreach (var segment in cell.DistalSegments)
					{
						// Determine if this segment is active if enough active synapses are present
						segment.ProcessSegment();
					}

					// Now check for active segment, we only need one for the cell to predict
					foreach (var segment in cell.DistalSegments)
					{
						// A cell will turn on its predictive state output if one of its 
						// segments becomes active, i.e. if enough of its lateral inputs 
						// are currently active due to feed-forward input.
						if (segment.ActiveState[Global.T])
						{
							cell.PredictiveState[Global.T] = true;
							cell.UpdateNumberPredictionSteps();

							if (segment.IsSequence)
							{
								cell.IsSegmentPredicting = true;
							}

							// a) reinforcement of the currently active segment
							if (Global.TemporalLearning)
							{
								// Add segment update to this cell
								cell.UpdateDistalSegmentActiveSynapses(Global.T, segment);
							}

							break;
						}
					}

					// b) reinforcement of a segment that could have predicted 
					//    this activation, i.e. a segment that has a (potentially weak)
					//    match to activity during the previous time step
					if (Global.TemporalLearning && cell.PredictiveState[Global.T])
					{
						DistalSegment predictiveDistalSegment = cell.GetBestMatchingDistalSegment(Global.T - 1, cell.NumberPredictionSteps + 1);

						// Only add new segments if predition steps will be <= max allowed time steps
						if (predictiveDistalSegment == null)
						{
							if (cell.NumberPredictionSteps + 1 <= DistalSegment.MaxTimeSteps)
							{
								// Either update existing or add new segment for this cell considering
								// only segments matching the number of prediction steps of the
								// best currently active segment for this cell
								SegmentUpdate predictiveSegmentUpdate = cell.UpdateDistalSegmentActiveSynapses(Global.T - 1, predictiveDistalSegment, true);
								predictiveSegmentUpdate.NumberPredictionSteps = cell.NumberPredictionSteps + 1;
							}
						}
					}
				}
			});
		}

		/// <summary>
		/// Temporal Pooler Phase 3: Update synapses.
		/// </summary>
		/// <remarks>
		/// The third and last phase actually carries out learning.
		/// In this phase segment updates that have been queued up are actually implemented
		/// once we get feed forward input and the cell is chosen as a learning cell.
		/// Otherwise, if the cell ever stops predicting for any reason, we negatively
		/// reinforce the segments.
		/// </remarks>
		private void TemporalPoolerPhase3()
		{
			// Carries out learning
			Parallel.ForEach(this.Columns, ParallelOptions, column =>
			{
				foreach (var cell in column.Cells)
				{
					if (cell.LearnState[Global.T])
					{
						// Segment updates that have been queued up are actually 
						// implemented once we get feed forward input and the cell is 
						// chosen as a learning cell.
						cell.ApplySegmentUpdates(true);
					}
					else if (!cell.PredictiveState[Global.T] && cell.PredictiveState[Global.T - 1])
					{
						// Otherwise, if the cell ever stops predicting for any reason, 
						// we negatively reinforce the segments.
						cell.ApplySegmentUpdates(false);
					}
					else if (cell.PredictiveState[Global.T] && cell.PredictiveState[Global.T - 1] && cell.NumberPredictionSteps > cell.PrevNumberPredictionSteps)
					{
						cell.ApplySegmentUpdates(false, (int) this.Statistics.StepCounter - 1);
					}
				}
			});
		}

		/// <summary>
		/// Get a reference to the Column at the specified column grid coordinate.
		/// </summary>
		/// <param name="x">the x coordinate component of the column's position.</param>
		/// <param name="y">the y coordinate component of the column's position.</param>
		/// <returns>a reference to the Column at that position.</returns>
		internal Column GetColumn(int x, int y)
		{
			return this.Columns[(y * this.Size.Width) + x];
		}

		/// <summary>
		/// The radius of the average connected receptive field size of all the columns. 
		/// </summary>
		/// <returns>The average connected receptive field size (in column grid space).
		/// </returns>
		/// <remarks>
		/// The connected receptive field size of a column includes only the connected 
		/// synapses (those with permanence values >= connectedPerm). This is used to 
		/// determine the extent of lateral inhibition between columns.
		/// </remarks>
		private float AverageReceptiveFieldSize()
		{
			int numberConnectedSynapses = 0;
			double sumReceptiveFieldSize = 0.0;
			foreach (var column in this.Columns)
			{
				foreach (var synapse in column.ProximalSegment.GetConnectedSynapses())
				{
					// Cast to proximalSynapse:
					var proximalSynapse = synapse as ProximalSynapse;
					Debug.Assert(synapse != null, "proximalSynapse != null");

					// Distance from 'center' of this column to the proximal synapse in the input space
					var distanceSynapseFromColumn = new Point();
					distanceSynapseFromColumn.X = column.CentralPositionInInput.X - proximalSynapse.InputSource.X;
					distanceSynapseFromColumn.Y = column.CentralPositionInInput.Y - proximalSynapse.InputSource.Y;

					// Calculate average connected receptive field size of this column
					double receptiveFieldSize = Math.Sqrt((distanceSynapseFromColumn.X * distanceSynapseFromColumn.X) + (distanceSynapseFromColumn.Y * distanceSynapseFromColumn.Y));

					// Translate back proportionally the field size from input size to region size
					double inputProportion = (double) (this.InputProportionX + this.InputProportionY) / 2;
					receptiveFieldSize /= inputProportion;

					sumReceptiveFieldSize += receptiveFieldSize;
					numberConnectedSynapses++;
				}
			}

			return (float) (sumReceptiveFieldSize / numberConnectedSynapses);
		}

		/// <summary>
		/// Returns a list of all the columns that are within inhibitionRadius of a column.
		/// </summary>
		/// <returns></returns>
		public List<Column> GetNeighbors(Column column)
		{
			var neighborColumns = new List<Column>();

			// First find bounds of neighbors within inhibition radius of the column
			int positionX = column.PositionInRegion.X;
			int positionY = column.PositionInRegion.Y;
			var inhibitionRadius = (int) Math.Round(this.InhibitionRadius);
			int initialX = Math.Max(0, Math.Min(positionX - 1, positionX - inhibitionRadius));
			int initialY = Math.Max(0, Math.Min(positionY - 1, positionY - inhibitionRadius));
			int finalX = Math.Min(this.Size.Width, Math.Max(positionX + 1, positionX + inhibitionRadius));
			int finalY = Math.Min(this.Size.Height, Math.Max(positionY + 1, positionY + inhibitionRadius));

			// Extra 1's for correct looping
			finalX = Math.Min(this.Size.Width, finalX + 1);
			finalY = Math.Min(this.Size.Height, finalY + 1);

			// Loop over all columns that are within inhibitionRadius of given column
			for (int x = initialX; x < finalX; ++x)
			{
				for (int y = initialY; y < finalY; ++y)
				{
					Column neighborColumn = this.GetColumn(x, y);
					neighborColumns.Add(neighborColumn);
				}
			}

			return neighborColumns;
		}

		/// <summary>
		/// Return true if the given Column has an overlap value that is at least the
		/// k'th largest amongst all neighboring columns within inhibitionRadius.
		/// </summary>
		/// <remarks>
		/// This function is effectively determining which columns are to be inhibited 
		/// during the spatial pooling procedure of the region.
		/// </remarks>
		internal bool IsWithinKthScore(Column column, int k)
		{
			// Loop over all columns that are within inhibitionRadius of given column
			// Count how many neighbor columns have strictly greater overlap than our
			// given column. If this count is < k then we are within the kth score
			int numberColumns = 0;
			foreach (var neighborColumn in this.GetNeighbors(column))
			{
				if (neighborColumn.Overlap > column.Overlap)
				{
					numberColumns += 1;
				}
			}

			// If count is < k, we are within the kth score of all neighbors
			return numberColumns < k;
		}

		/// <summary>
		/// Populates output array with the current prediction values for each
		/// column in this <see cref="Region"/>.
		/// </summary>
		/// <returns> A 2d int array of same shape as the column grid populated with the
		/// prediction values for each column in the <see cref="Region"/> based on the 
		/// most recently processed time step.
		/// </returns>
		/// <remarks>
		/// The value returned for a column represents the fewest number of time steps 
		/// the column believes an activation will occur.
		/// For example a 1 value means the column is predicting it will become active
		/// in the very next time step (t+1). A value of 2 means it expects activation
		/// in 2 time steps (t+2) etc.  A value of 0 means the column is not currently
		/// making any prediction.
		/// </remarks>
		public int[,] GetPredictingColumnsByTimeStep()
		{
			var outputData = new int[this.Size.Width,this.Size.Height];

			foreach (var column in this.Columns)
			{
				// If a column has multiple predicting cells, find the one that is making
				// the prediction that will happen the earliest and store that value
				bool columnIsPredicting = false;
				int numberPredictionSteps = DistalSegment.MaxTimeSteps;

				foreach (var cell in column.Cells)
				{
					if (cell.PredictiveState[Global.T] && cell.NumberPredictionSteps < numberPredictionSteps)
					{
						numberPredictionSteps = cell.NumberPredictionSteps;
						columnIsPredicting = true;
					}
				}

				if (columnIsPredicting)
				{
					outputData[column.PositionInRegion.X, column.PositionInRegion.Y] = numberPredictionSteps;
				}
			}

			return outputData;
		}

		/// <summary>
		/// Populates output array with the current prediction values for each
		/// column in this <see cref="Region"/>.
		/// </summary>
		/// <returns> A 2d float array of same shape as the column grid populated with the
		/// prediction values for each column in the <see cref="Region"/> based on the 
		/// most recently processed time step.
		/// </returns>
		/// <param name="predictionStep">How many steps in the future to get prediction 
		/// for.</param>
		/// <remarks>
		/// The value returned represents how intense the column predicts itself.
		/// The intensity is the sum of the permanences of active synapses at the best segment.
		/// </remarks>
		public float[,] GetPredictingColumnsByStrength(int predictionStep)
		{
			var outputData = new float[this.Size.Width,this.Size.Height];

			foreach (var column in this.Columns)
			{
				float strength = 0;

				foreach (var cell in column.Cells)
				{
					DistalSegment distalSegment = cell.GetBestMatchingDistalSegment(Global.T, predictionStep);

					if ((distalSegment != null) && (distalSegment.ActiveState[Global.T]))
					{
						foreach (var activeSynapse in distalSegment.GetActiveSynapses(Global.T))
						{
							strength += activeSynapse.Permanence;
						}
					}
				}

				outputData[column.PositionInRegion.X, column.PositionInRegion.Y] = strength;
			}

			return outputData;
		}

		/// <summary>
		/// Gets the input reconstruction of the actual input space from the active and
		/// Connected synapses from the column's feedforward input.
		/// </summary>
		/// <returns>A float matrix </returns>
		public float[,] GetInputReconstructionOfFeedforwardInput()
		{
			var reconstructionMatrix = new float[this.InputSize.Width,this.InputSize.Height];

			foreach (var column in this.Columns)
			{
				if (column.ActiveState[Global.T])
				{
					foreach (ProximalSynapse activeSynapse in column.ProximalSegment.GetActiveConnectedSynapses(Global.T))
					{
						reconstructionMatrix[activeSynapse.InputSource.X, activeSynapse.InputSource.Y] += activeSynapse.Permanence;
					}
				}
			}

			return reconstructionMatrix;
		}

		/// <summary>
		/// Converts the column's predictions value into reconstruction of the actual
		/// Input space.
		/// </summary>
		/// <returns>A float matrix </returns>
		public float[,] GetPredictionReconstruction(int predictionStep)
		{
			float[,] columnsPrediction = this.GetPredictingColumnsByStrength(predictionStep);
			var reconstructionMatrix = new float[this.InputSize.Width,this.InputSize.Height];

			foreach (var column in this.Columns)
			{
				foreach (ProximalSynapse synapse in column.ProximalSegment.Synapses)
				{
					if (synapse.IsConnected())
					{
						reconstructionMatrix[synapse.InputSource.X, synapse.InputSource.Y] += columnsPrediction[column.PositionInRegion.X, column.PositionInRegion.Y] * synapse.Permanence;
					}
				}
			}

			return reconstructionMatrix;
		}


		/// <summary>
		/// Saves the content from <see cref="Region"/> instance to XML file.
		/// </summary>
		/// <param name="filePath">The XML file path.</param>
		public void SaveToFile ( string filePath )
		{
			FileStream fileStream = null;

			// Serialize Config instance to XML file
			try
			{
				fileStream = new FileStream ( filePath, FileMode.Create, FileAccess.Write );
				new XmlSerializer ( this.GetType() ).Serialize ( fileStream, this );
			}
			catch (Exception ex)
			{
				throw new Exception ( "Cannot save Region to file.", ex );
			}
			finally
			{
				fileStream.Close ();
			}
		}

		/// <summary>
		/// Loads the content from XML file to <see cref="Region"/> instance.
		/// </summary>
		/// <param name="filePath">The XML file path.</param>
		public Region LoadFromFile ( string filePath )
		{
			FileStream fileStream = null;

			Region region = null;

			try
			{
				// Dessearialize XML to a new instance
				fileStream = new FileStream ( filePath, FileMode.Open, FileAccess.Read );
				region = (Region)new XmlSerializer ( typeof ( Region ) ).Deserialize ( fileStream );
			}
			catch (Exception ex)
			{
				throw new Exception ( "Cannot load Region from file.", ex );
			}
			finally
			{
				fileStream.Close ();
			}
			return region;
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
			str = String.Format ( "R{0}", this.Index );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public virtual void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			this.Statistics.AddWatchTable ( ref dataSet, tableName );

			//Input - List
			DataTable dt = new DataTable ( "Inputs " + ID() + " " + tableName );
			dt.Columns.Add ( "Input", typeof ( int[,] ) );
			foreach (var iArr in Input)
			{
				DataRow dr = dt.NewRow ();
				dr["Input"] = iArr;	 //this may not work
				dt.Rows.Add ( dr );
			}
			dataSet.Tables.Add ( dt );

			//Columns - List
			dt = new DataTable ( "Columns " + ID () + " " + tableName );
			dt.Columns.Add ( "Input", typeof ( int[,] ) );
			foreach (var iArr in Input)
			{
				DataRow dr = dt.NewRow ();
				dr["Input"] = iArr;	 //this may not work
				dt.Rows.Add ( dr );
			}
			dataSet.Tables.Add ( dt );
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
			dt.Columns.Add ( "ParentRegionID", typeof ( string ) );
			dt.Columns.Add ( "Children", typeof ( int ) );
			dt.Columns.Add ( "Size", typeof ( Size ) );
			dt.Columns.Add ( "Initialized", typeof ( bool ) );
			dt.Columns.Add ( "InputSize", typeof ( Size ) );
			dt.Columns.Add ( "PercentageInputPerColumn", typeof ( Single ) );
			dt.Columns.Add ( "InputProportionX", typeof ( Single ) );
			dt.Columns.Add ( "InputProportionY", typeof ( Single ) );
			dt.Columns.Add ( "CellsPerColumn", typeof ( int ) );
			dt.Columns.Add ( "PercentageLocalActivity", typeof ( Single ) );
			dt.Columns.Add ( "DesiredLocalActivity", typeof ( int ) );
			dt.Columns.Add ( "InhibitionRadius", typeof ( Single ) );
			dt.Columns.Add ( "LocalityRadius", typeof ( int ) );
			dt.Columns.Add ( "NumberNewSynapses", typeof ( int ) );
			dt.Columns.Add ( "PercentageMinOverlap", typeof ( Single ) );
			dt.Columns.Add ( "SegmentActiveThreshold", typeof ( int ) );
			dt.Columns.Add ( "HardcodedSpatial", typeof ( bool ) );
			dt.Columns.Add ( "FullDefaultSpatialPermanence", typeof ( bool ) );
			//dt.Columns.Add ( "ParallelOptions", typeof ( ParallelOptions ) );
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
			dr["ParentRegionID"] = ParentRegion.Index;
			dr["Children"] = Children.Count;
			dr["Size"] = Size;
			dr["Initialized"] = Initialized;
			dr["InputSize"] = InputSize;
			dr["PercentageInputPerColumn"] = PercentageInputPerColumn;
			dr["InputProportionX"] = InputProportionX;
			dr["InputProportionY"] = InputProportionY;
			dr["CellsPerColumn"] = CellsPerColumn;
			dr["PercentageLocalActivity"] = PercentageLocalActivity;
			dr["DesiredLocalActivity"] = DesiredLocalActivity;
			dr["InhibitionRadius"] = InhibitionRadius;
			dr["LocalityRadius"] = LocalityRadius;
			dr["NumberNewSynapses"] = NumberNewSynapses;
			dr["PercentageMinOverlap"] = PercentageMinOverlap;
			dr["SegmentActiveThreshold"] = SegmentActiveThreshold;
			dr["HardcodedSpatial"] = HardcodedSpatial;
			dr["FullDefaultSpatialPermanence"] = FullDefaultSpatialPermanence;
			//dr["ParallelOptions"] = ParallelOptions;
		}



		#endregion


		#region Statistics

		/// <summary>
		/// Updates statistics values.
		/// </summary>
		public void ComputeBasicStatistics()
		{
			this.Statistics.StepCounter++;

			// Calculates Column-Activity Rate
			this.Statistics.ActivityRate = this.Statistics.ActivityCounter / (this.Statistics.StepCounter * this.Columns.Count);

			// Regional Precision-Counter
			if (this.Statistics.SegmentPredictionCounter > 0)
			{
				this.Statistics.PredictPrecision = this.Statistics.CorrectSegmentPredictionCounter / this.Statistics.SegmentPredictionCounter;
			}

			if (this.Statistics.ActivityCounter > 0)
			{
				this.Statistics.ActivityPrecision = this.Statistics.CorrectSegmentPredictionCounter / this.Statistics.ActivityCounter;
			}

			// segmentPredictionCounter = 0;
			// correctSegmentPredictionCounter = 0;
			// ActivityCounter = 0;
		}

		///<summary>
		///Calculate both the activation accuracy and the prediction accuracy for all
		///the column cells in this region within the last processed time step.
		///</summary>
		///<remarks>
		///The activation accuracy is the number of correctly predicted active columns
		///out of the total active columns.  The prediction accuracy is the number of
		///correctly predicted active columns out of the total sequence-segment
		///predicted columns (most recent time step).  The results are internally stored
		///in the region properties ColumnActivationAccuracy and ColumnPredictionAccuracy.
		///</remarks>
		private void ComputeColumnAccuracy()
		{
			//want to know % active columns that were correctly predicted
			int sumP = 0;
			int sumA = 0;
			int sumAP = 0;
			foreach (var col in this.Columns)
			{
				if (col.ActiveState[Global.T])
				{
					++sumA;
				}

				foreach (var cell in col.Cells)
				{
					bool addP = cell.GetSequencePredictingDistalSegment() != null;
					if (addP)
					{
						++sumP;
						if (col.ActiveState[Global.T])
						{
							++sumAP;
						}
						break;
					}
				}
			}

			//compare active columns now, to predicted columns from t-1
			float pctA = 0.0f;
			float pctP = 0.0f;
			if (sumA > 0)
			{
				pctA = sumAP / (float) sumA;
			}
			if (sumP > 0)
			{
				pctP = sumAP / (float) sumP;
			}

			this.Statistics.ColumnActivationAccuracy = pctA;
			this.Statistics.ColumnPredictionAccuracy = pctP;
		}

		///<summary>
		///Compute the total number of active columns in the Region as calculated from
		///the most recently run time step.  For hardcoded Regions this number will match
		///the number of 1 values from the input data.  For spatial pooling Regions this
		///number will represent the number of winning columns after inhibition.
		///</summary>
		private void ComputeNumberActiveColumns()
		{
			this.Statistics.NumberActiveColumns = 0;
			foreach (var column in this.Columns)
			{
				if (column.ActiveState[Global.T])
				{
					this.Statistics.NumberActiveColumns += 1;
				}
			}
		}

		#endregion

		#endregion

		#region Diagnostics

		private void Write (int idx )
		{
			StreamWriter file = new StreamWriter ( String.Format ( "# 000", idx ) + " Region " + Size.Width + "x" + Size.Height + " Input " + InputSize.Width + "x" + InputSize.Height + ".csv" );

			file.WriteLine ( "Region:, InputSize," + InputSize.Width + "x" + InputSize.Height + "," + "InputProportionX,," + InputProportionX );
			file.WriteLine ( "Size," + Size.Width + "x" + Size.Height + ",," + "InputProportionY,," + InputProportionY );

			file.WriteLine ( "HardcodedSpatial,,," + HardcodedSpatial );
			file.WriteLine ( "InhibitionRadius,,," + InhibitionRadius );
			file.WriteLine ( "PercentageInputPerColumn,,," + PercentageInputPerColumn );
			file.WriteLine ( "PercentageMinOverlap,,," + PercentageMinOverlap );
			file.WriteLine ( "PercentageLocalActivity,,," + PercentageLocalActivity );
			file.WriteLine ( "DesiredLocalActivity,,," + DesiredLocalActivity );

			file.WriteLine ();
			file.WriteLine ( "Column input mapping:" );
			file.WriteLine ();
			file.WriteLine ( "positionInRegion" + ",,," + "centralPositionInInput" );
			foreach (var column in this.Columns)
			{
				file.WriteLine ( column.PositionInRegion.X + "," + column.PositionInRegion.Y + ",," + column.CentralPositionInInput.X + "," + column.CentralPositionInInput.Y );
			}

			file.WriteLine ( "Column Proximal Segment:" );
			foreach (var column in this.Columns)
			{
				file.WriteLine ( ",Column," + column.PositionInRegion.X + "," + column.PositionInRegion.Y );
				file.WriteLine ( ",,Proximal Segment Synapses:" );
				foreach (ProximalSynapse syn in column.ProximalSegment.Synapses)
				{
					file.WriteLine ( ",,," + "Permanence,," + syn.Permanence + ",," + "Active," + syn.IsActive ( Global.T ) );
				}
			}

			file.WriteLine ( "Cells:" );
			foreach (var column in this.Columns)
			{
				file.WriteLine ( "Column:," + column.PositionInRegion.X + "," + column.PositionInRegion.Y + "," + "CentralPositionInInput,,," + column.CentralPositionInInput.X + "," + column.CentralPositionInInput.Y );
				foreach (Cell cell in column.Cells)
				{
					file.WriteLine ( ",Cell#," + cell.Index + ",," + "DistalSegments,," + cell.DistalSegments.Count );
					//file.WriteLine ( ",,ToString:," + cell.ToString() );
					foreach (DistalSegment seg in cell.DistalSegments)
					{
						file.WriteLine ( ",,DistalSegment:," + seg.ActiveState + "," + "IsSequence,," + seg.IsSequence );
						foreach (DistalSynapse syn in seg.Synapses)
						{
							file.WriteLine ( ",,,DistalSyn:,Perm," + syn.Permanence + "," + "IsConn." + syn.IsConnected () + ",IsActive," + syn.IsActive ( Global.T ) );
							file.WriteLine ( ",,,,ToString:," + syn.ToString () );
						}
					}

				}
			}
			file.Close ();
		}

		#endregion
	}
}
