using System;
using System.Drawing;
using System.Collections.Generic;
using System.Data;
using OpenHTM.CLA.Statistics;
//using OpenHTM.Shared.Interfaces;


namespace OpenHTM.CLA
{
		
	/// <summary>
	/// A data structure representing a synapse. Contains a permanence value and the 
	/// source input index.  Also contains a 'location' in the input space that this synapse
	/// roughly represents.
	/// </summary>
	public class Cell : Selectable3DObject
	{
		#region Fields

		/// <summary>
		/// Statistics used for check accuracy of predictions.
		/// </summary>
		/// 
		public StatisticsCell Statistics = new StatisticsCell ();

		// TODO: Make it a parameter?
		/// <summary>
		/// Minimum segment activity for learning.
		/// Original name: minThreshold
		/// </summary>
		private const int _minSynapsesPerSegmentThreshold = 1;

		/// <summary>
		/// A list of <see cref="SegmentUpdate"/> structures, the list of changes for current cell.
		/// Original name: segmentUpdateList
		/// </summary>
		/// 
		public List<SegmentUpdate> SegmentUpdates;


		#endregion

		#region Properties

		/// <summary>
		/// Column which this cell belongs to.
		/// </summary>
		public Column Column { get; private set; }


		/// <summary>
		/// Position in Column
		/// </summary>
		//public int Index { get; private set; }
		public int Index { get; set; }

		/// <summary>
		/// Contains this cell's distal segments.
		/// </summary>
		public List<DistalSegment> DistalSegments { get; private set; }


		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Cell"/> is active.
		/// </summary>
		/// <remarks>
		/// It represents the active state of a cell at time t given the current 
		/// feed-forward input and the past temporal context.  ActiveState[t] is the 
		/// contribution from a cell at time t.  If true, the cell has current feed-forward
		/// input as well as an appropriate temporal context.
		/// </remarks>
		public List<bool> ActiveState { get; internal set; }


		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Cell"/> is predicting.
		/// </summary>
		/// <remarks> 		
		/// It represents the prediction of a cell at time t, given the bottom-up activity 
		/// of other columns and the past temporal context.  PredictiveState[t] is the 
		/// contribution of a cell at time t.  If true, the cell is predicting feed-forward
		/// input in the current temporal context.
		/// </remarks>
		public List<bool> PredictiveState { get; internal set; }


		/// <summary>
		/// Gets or sets a value indicating whether this  <see cref="Cell"/> is learning.
		/// </summary>
		/// <remarks>
		/// Indicates whether this cell is selected as the learning cell for a sequence.
		/// </remarks>
		public List<bool> LearnState { get; internal set; }


		/// <summary>
		/// Indicates whether this cell is predicting to become active at the next time step.
		/// </summary>
		/// <remarks> Also means that the cell is predicting. </remarks>
		public bool IsSegmentPredicting { get; internal set; }


		/// <summary>
		/// Gets the number prediction steps.
		/// </summary>
		public int NumberPredictionSteps { get; private set; }


		/// <summary>
		/// Gets the number of prediction steps on previous step.
		/// </summary>
		public int PrevNumberPredictionSteps { get; private set; }


		#region UI

		public bool IsDataGridSelected { get; set; }

		#endregion

		#endregion

		#region Constructor

		




		/// <summary>
		/// Creates a new Cell belonging to the specified <see cref="Column"/>. The index is an 
		/// integer id to distinguish this Cell from others in the <see cref="Column"/>.
		/// </summary>
		/// <param name="column"></param>
		/// <param name="index"></param>
		public Cell(Column column, int index)
		{
			// Set fields
			this.ActiveState = new List<bool>();
			this.Column = column;
			this.Index = index;
			this.DistalSegments = new List<DistalSegment>();
			this.LearnState = new List<bool>();
			this.PredictiveState = new List<bool>();
			this.SegmentUpdates = new List<SegmentUpdate>();

			// Initialize state vectors with fixed lenght = T
			for (int t = 0; t <= Global.T; t++)
			{
				this.ActiveState.Add(false);
				this.PredictiveState.Add(false);
				this.LearnState.Add(false);
			}

			SelectablelType = SelectableObjectType.Cell;
		}


		/// <summary>
		/// Parameterless constructor for serialization.
		/// </summary>
		public Cell ()
		{ }


		#endregion

		#region Methods

		/// <summary>
		/// Advances this cell to the next time step. 
		/// </summary>
		/// <remarks>
		/// The current state of this cell (active, learning, predicting) will be the
		/// previous state and the current state will be reset to no cell activity by 
		/// default until it can be determined.
		/// </remarks>
		internal void NextTimeStep()
		{
			// Compute basic statistics
			this.ComputeBasicStatistics();

			// Remove the element in first position in order to the state vector always have lenght = T
			// and add a new element to represent the state at current time step
			this.ActiveState.RemoveAt(0);
			this.ActiveState.Add(false);
			this.PredictiveState.RemoveAt(0);
			this.PredictiveState.Add(false);
			this.LearnState.RemoveAt(0);
			this.LearnState.Add(false);

			// Set fields for the current time step
			this.IsSegmentPredicting = false;
			this.PrevNumberPredictionSteps = this.NumberPredictionSteps;

			// Loop segments in order to perfom actions related to time step progression
			foreach (var segment in this.DistalSegments)
			{
				segment.NextTimeStep();
			}
		}

		/// <summary>
		/// Creates a new distal segment for this Cell.
		/// </summary>
		/// <param name="cellsToConnect"> A set of available cells to add to 
		/// the segmentUpdateList.</param>
		/// <returns> Created segmentUpdateList</returns>
		/// <remarks>
		/// The new segment will initially connect to at most newNumberSynapses 
		/// synapses randomly selected from the set of cells that
		/// were in the learning state at t-1 (specified by the learningCells parameter).
		/// </remarks>
		internal DistalSegment CreateDistalSegment(List<Cell> cellsToConnect)
		{
			var newDistalSegment = new DistalSegment( this );

			foreach (var cell in cellsToConnect)
			{
				newDistalSegment.CreateSynapse(cell, Synapse.InitialPermanence);
			}

			this.DistalSegments.Add(newDistalSegment);
			return newDistalSegment;
		}

		/// <summary>
		/// For this cell, return a <see cref="Segment"/> that is active in the given
		/// time step. If multiple segments are active, sequence segments are given
		/// preference. Otherwise, segments with most activity are given preference.
		/// Original name: getActiveSegment
		/// </summary>
		public DistalSegment GetActiveDistalSegment(int t)
		{
			DistalSegment bestDistalSegment = null;
			bool foundSequenceSegment = false;
			int bestNumberActiveSynapses = 0;

			foreach (var segment in this.DistalSegments)
			{
				int numberActiveSynapses = segment.GetActiveSynapses(t).Count;

				if (numberActiveSynapses >= Segment.ActivationThreshold)
				{
					// Sequence segments are given preference.
					// Otherwise, segments with most activity are given preference.
					if (segment.IsSequence)
					{
						foundSequenceSegment = true;
						if (numberActiveSynapses > bestNumberActiveSynapses)
						{
							bestNumberActiveSynapses = numberActiveSynapses;
							bestDistalSegment = segment;
						}
					}
					else if (!foundSequenceSegment && numberActiveSynapses > bestNumberActiveSynapses)
					{
						bestNumberActiveSynapses = numberActiveSynapses;
						bestDistalSegment = segment;
					}
				}
			}

			return bestDistalSegment;
		}

		/// <summary>
		/// Gets the sequence segment. If it's not null then this cell was predicted to 
		/// become active at current time step.
		/// </summary>
		/// <value>The segment predicting or null if none found.</value>
		public DistalSegment GetSequencePredictingDistalSegment()
		{
			DistalSegment predictingDistalSegment = null;

			try
			{
				if (this.PredictiveState[Global.T - 1])
				{
					DistalSegment distalSegment = this.GetActiveDistalSegment(Global.T - 1);
					if (distalSegment != null && distalSegment.IsSequence)
					{
						predictingDistalSegment = distalSegment;
					}
				}
			}
			catch (InvalidOperationException)
			{
			}

			return predictingDistalSegment;
		}

		///<summary>
		/// Add a new <see cref="SegmentUpdate"/> object to this <see cref="Cell"/>
		/// containing proposed changes to the specified segment. 
		///</summary>
		///<remarks>
		/// If the segment is None, then a new segment is to be added, otherwise
		/// the specified segment is updated.  If the segment exists, find all active
		/// synapses for the segment (either at t or t-1)
		/// and mark them as needing to be updated.  If newSynapses is true, then
		/// Region.newNumberSynapses - len(activeSynapses) new synapses are added to the
		/// segment to be updated.  The (new) synapses are randomly chosen from the set
		/// of current learning cells (within Region.localityRadius if set).
		///
		/// These segment updates are only applied when the applySegmentUpdates
		/// method is later called on this Cell.
		///</remarks>
		internal SegmentUpdate UpdateDistalSegmentActiveSynapses(int t, DistalSegment distalSegment, bool newSynapses = false)
		{
			// Let ActiveSynapses be the list of active synapses where the originating 
			// cells have their ActiveState output = true at time step t.
			// (This list is empty if segment = null since the segment doesn't exist.)
			var activeSynapses = new List<Synapse>();
			if (distalSegment != null)
			{
				activeSynapses = distalSegment.GetActiveSynapses(t);
			}

			var segmentUpdate = new SegmentUpdate(this, distalSegment, activeSynapses, newSynapses);
			this.SegmentUpdates.Add(segmentUpdate);

			return segmentUpdate;
		}

		///<summary>
		/// This function reinforces each segment in this Cell's SegmentUpdate.
		/// </summary>
		/// <remarks>
		/// Using the segmentUpdate, the following changes are
		/// performed. If positiveReinforcement is true then synapses on the active
		/// list get their permanence counts incremented by permanenceInc. All other
		/// synapses get their permanence counts decremented by permanenceDec. If
		/// positiveReinforcement is false, then synapses on the active list get
		/// their permanence counts decremented by permanenceDec. After this step,
		/// any synapses in segmentUpdate that do yet exist get added with a permanence
		/// count of initialPerm. These new synapses are randomly chosen from the
		/// set of all cells that have learnState output = 1 at time step t.
		///</remarks>
		internal void ApplySegmentUpdates(bool positiveReinforcement, int latestCreationStep = -1)
		{
			// This loop iterates through a list of SegmentUpdate's and reinforces each segment.
			foreach (var segmentUpdate in this.SegmentUpdates)
			{
				DistalSegment distalSegment = segmentUpdate.DistalSegment;

				if (distalSegment != null)
				{
					if (positiveReinforcement)
					{
						distalSegment.UpdatePermanences(segmentUpdate.ActiveDistalSynapses);
					}
					else
					{
						distalSegment.DecreasePermanences(segmentUpdate.ActiveDistalSynapses);
					}
				}

				// Add new synapses (and new segment if necessary)
				if (segmentUpdate.AddNewSynapses && positiveReinforcement)
				{
					if (distalSegment == null)
					{
						// Only add new segment if end cells to connect are available
						if (segmentUpdate.CellsToConnect.Count > 0)
						{
							distalSegment = segmentUpdate.CreateDistalSegment();
						}
					}
					else if (segmentUpdate.CellsToConnect.Count > 0)
					{
						// Add new synapses to existing segment
						segmentUpdate.CreateDistalSynapses();
					}
				}
			}

			// Delete segment update instances after they are applied
			this.SegmentUpdates.Clear();
		}

		/// <summary>
		/// Gets the best matching <see cref="Segment"/>.
		/// </summary>
		/// <param name="t">Only consider active segments at time t.</param>
		/// <param name="numberPredictionSteps">Number of time steps in the future an activation will occur.</param>
		/// <remarks>
		/// For this cell (at t or t-1), find the <see cref="Segment"/> (only
		/// consider sequence segments if isSequence is True, otherwise only consider
		/// non-sequence segments) with the largest number of active synapses. 
		/// This routine is aggressive in finding the best match. The permanence 
		/// value of synapses is allowed to be below connectedPerm. 
		/// The number of active synapses is allowed to be below activationThreshold, 
		/// but must be above minThreshold. The routine returns that segment. 
		/// If no segments are found, then null is returned.
		/// </remarks>
		internal DistalSegment GetBestMatchingDistalSegment(int t, int numberPredictionSteps)
		{
			DistalSegment bestDistalSegment = null;
			int bestNumberActiveSynapses = _minSynapsesPerSegmentThreshold;

			foreach (var segment in this.DistalSegments)
			{
				if (segment.NumberPredictionSteps == numberPredictionSteps)
				{
					// Get all the active synapses in the given t time step, no matter connection value
					int numberActiveSynapses = segment.GetActiveSynapses(t).Count;
					if (numberActiveSynapses >= bestNumberActiveSynapses)
					{
						bestNumberActiveSynapses = numberActiveSynapses;
						bestDistalSegment = segment;
					}
				}
			}

			return bestDistalSegment;
		}

		/// <summary>
		/// Updates the number of prediction steps.
		/// </summary>
		internal void UpdateNumberPredictionSteps()
		{
			this.NumberPredictionSteps = DistalSegment.MaxTimeSteps;
			foreach (var segment in this.DistalSegments)
			{
				if (segment.ActiveState[Global.T] && segment.NumberPredictionSteps < this.NumberPredictionSteps)
				{
					this.NumberPredictionSteps = segment.NumberPredictionSteps;
				}
			}
		}


		#endregion


		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public virtual string ID ()
		{
			string str = "";
			str = String.Format ( "C{0} {1} {2}", Column.PositionInRegion.X, Column.PositionInRegion.Y, this.Index );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public virtual void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( DataTable ( tableName ) );
			this.Statistics.AddWatchTable ( ref dataSet, ID () + " " + tableName );

			//SegmentUpdates - List
			foreach (SegmentUpdate su in SegmentUpdates)
			{
				su.AddWatchTable ( ref dataSet, tableName );
			}

			base.AddWatchTable ( ref dataSet, ID() + " " + tableName );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public virtual void AddWatchData ( ref DataTable dt )
		{
			this.AddColumns ( ref dt );
			this.AddDataRow ( ref dt );

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
			dt.Columns.Add ( "Column", typeof ( Point ) );
			dt.Columns.Add ( "Index", typeof ( Single ) );
			dt.Columns.Add ( "IsSegmentPredicting", typeof ( bool ) );
			dt.Columns.Add ( "NumberPredictionSteps", typeof ( int ) );
			dt.Columns.Add ( "PrevNumberPredictionSteps", typeof ( int ) );
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
			dr["Column"] = Column.PositionInRegion;
			dr["Index"] = Index;
			dr["IsSegmentPredicting"] = IsSegmentPredicting;
			dr["NumberPredictionSteps"] = NumberPredictionSteps;
			dr["PrevNumberPredictionSteps"] = PrevNumberPredictionSteps;
		}

		#endregion
		



#if DEBUG
		public override string ToString()
		{
			return "[" + this.Column.PositionInRegion.X.ToString() + "," + 
				this.Column.PositionInRegion.Y.ToString() + "]:" + this.Index + ": " + 
				(this.ActiveState[Global.T] ? "A" : "") + (this.LearnState[Global.T] ? "L" : "") + 
				(this.PredictiveState[Global.T] ? "P" : "") + (this.IsSegmentPredicting ? "S" : "") + 
				(this.LearnState[Global.T - 1] ? "Wl" : "") + (this.PredictiveState[Global.T - 1] ? "Wp" : "") + 
				" Segs=" + this.Statistics.NumberSegments;
		}
#endif

		#region Statistics

		/// <summary>
		/// Updates basic statistics on cell-level.
		/// </summary>
		/// <remarks>
		/// A: Absolute paramters:
		///  StepCounter: aboluste number of steps
		///  Activity Counter: absolute number of cell activations
		///  Prediction Counter: absolute number of cell predictions
		///  Correct Prediction Counter: absolute number of CORRECT cell predictions
		///  Learning Counter: absolute number of learning activations
		///  Segment Counter: absolute number of segments per cell
		///  MaxSynCounter: max. Number of synapses per cell
		/// B: Averages:
		///  Activity Rate: ActivityCounter/StepCounter
		///  Precision: CorrectPredictions/Predictions
		/// </remarks>
		public void ComputeBasicStatistics()
		{
			this.Statistics.StepCounter++;

			if (this.ActiveState[Global.T])
			{
				// Update cell
				this.Statistics.ActivityCounter++;

				// Update column and region
				this.Column.Statistics.ActivityCounter++;

				if (this.PredictiveState[Global.T - 1])
				{
					this.Statistics.CorrectPredictionCounter++;
					this.Column.Statistics.CorrectPredictionCounter++;

					if (this.GetSequencePredictingDistalSegment() != null)
					{
						this.Statistics.CorrectSegmentPredictionCounter++;
						this.Column.Statistics.CorrectSegmentPredictionCounter++;
						this.Statistics.ActivityPrecision = this.Statistics.CorrectSegmentPredictionCounter / this.Statistics.ActivityCounter;
					}
				}
			}

			if (this.PredictiveState[Global.T - 1])
			{
				this.Statistics.PredictionCounter++;
				this.Column.Statistics.PredictionCounter++;

				if (this.GetSequencePredictingDistalSegment() != null)
				{
					this.Statistics.SegmentPredictionCounter++;
					this.Column.Statistics.SegmentPredictionCounter++;
					this.Statistics.PredictPrecision = this.Statistics.CorrectSegmentPredictionCounter / this.Statistics.SegmentPredictionCounter;
				}
			}

			if (this.LearnState[Global.T])
			{
				this.Statistics.LearningCounter++;
				this.Column.Statistics.LearningCounter++;
			}

			this.Statistics.ActivityRate = this.Statistics.ActivityCounter / this.Statistics.StepCounter;

			// Display Number of Segments per Cell:
			if (this.DistalSegments != null)
			{
				this.Statistics.NumberSegments = this.DistalSegments.Count;
				int maxNumberSynapses = 0;
				foreach (var item in this.DistalSegments)
				{
					int newNumberSynapses = item.Synapses.Count;
					if (newNumberSynapses > maxNumberSynapses)
					{
						maxNumberSynapses = newNumberSynapses;
					}
				}

				this.Statistics.MaxNumberSynapses = maxNumberSynapses;
			}
		}

		#endregion


	}

	

}
