using System;
using System.Data;
using System.Collections.Generic;

namespace OpenHTM.CLA
{
	/// <summary>
	/// Represents a single dendrite segment that forms synapses (connections) to  
	/// cells in lateral columns.
	/// </summary>
	/// <remarks>
	/// Each Segment also maintains a boolean flag indicating 
	/// whether the Segment predicts feed-forward input on the next time step.
	/// Distal segments are used in temporal pooling.  Segments are considered 'active' 
	/// if enough of its existing synapses are connected and individually active.
	/// </remarks>
	public class DistalSegment : Segment
	{
		#region Fields


		/// <summary>
		/// Cell which this segment belongs to.
		/// </summary>
		public Cell parentCell { get; private set; }
		
		// TODO: refactor to Global or Region?
		/// <summary>
		/// Maximum number of prediction steps to track.
		/// </summary>
		public static readonly int MaxTimeSteps = 1;

		private int _numberPredictionSteps;

		#endregion

		#region Properties

		/// <summary>
		/// Indicates whether the <see cref="Segment"/> predicts feed-forward input on the 
		/// next time step.
		/// </summary>
		//public bool IsSequence { get; private set; }
		public bool IsSequence { get; set; }

		///<summary>
		///Define the number of time steps in the future an activation will occur
		///in if this segment becomes active.
		///</summary>
		///<remarks>
		/// For example if the segment is intended to predict activation in the very next 
		/// time step (t+1) then this value is 1. If the value is 2 this segment is said 
		/// to predict its Cell will activate in 2 time steps (t+2) etc.  By definition if 
		/// a segment is a sequence segment it has a value of 1 for prediction steps.
		///</remarks>
		public int NumberPredictionSteps
		{
			get
			{
				return this._numberPredictionSteps;
			}
			set
			{
				this._numberPredictionSteps = Math.Min(Math.Max(1, value), MaxTimeSteps);
				this.IsSequence = (this._numberPredictionSteps == 1);
			}
		}
		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Segment"/> class.
		/// </summary>
		internal DistalSegment( Cell cell)
		{
			this.parentCell = cell;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create a new synapse for this segment attached to the specified lateral cell.
		/// </summary>
		/// <param name="lateralCell">the lateral cell of the synapse to create.</param>
		/// <param name="initialPermanence">the initial permanence of the synapse.</param>
		internal void CreateSynapse(Cell lateralCell, float initialPermanence)
		{
			var newSynapse = new DistalSynapse(this, lateralCell, initialPermanence);
			this.Synapses.Add(newSynapse);
		}

		///<summary>
		/// Update (increase or decrease based on whether the synapse is active)
		/// all permanence values of each of the synapses in the specified set.
		///</summary>s
		public void UpdatePermanences(List<Synapse> activeSynapses)
		{
			//decrease all synapses first...
			foreach (var synapse in this.Synapses)
			{
				synapse.DecreasePermanence();
			}

			//then for each active synapse, undo its decrement and add an increment
			foreach (var activeSynapse in activeSynapses)
			{
				activeSynapse.IncreasePermanence(Synapse.PermanenceDecrement + Synapse.PermanenceIncrement);
			}
		}

		///<summary>
		/// Decrease the permanences of each of the synapses in the set of
		/// active synapses that happen to be on this segment.
		///</summary>
		public void DecreasePermanences(List<Synapse> activeSynapses)
		{
			foreach (var activeSynapse in activeSynapses)
			{
				activeSynapse.DecreasePermanence();
			}
		}

		/// <summary>
		/// Returns true if the number of connected synapses on this 
		/// <see cref="Segment"/> that were active due to learning states at time t-1 is 
		/// greater than activationThreshold. 
		/// </summary>  
		public bool WasActiveFromLearning()
		{
			int numberSynapsesWasActive = 0;
			foreach (var synapse in this.Synapses)
			{
				if (synapse.IsActiveFromLearning(Global.T - 1))
				{
					numberSynapsesWasActive += 1;
				}
			}
			return numberSynapsesWasActive >= ActivationThreshold;
		}


		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public override string ID ()
		{
			string str = "";
			str = String.Format ( "DistalSeg[{0}]", parentCell.ID () );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public override void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			base.AddWatchTable ( ref dataSet, tableName );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public override void AddWatchData ( ref DataTable dt )
		{
			AddColumns ( ref dt );
			AddDataRow ( ref dt );

			base.AddWatchData ( ref dt );
		}

		/// <summary>
		/// Convert object specific data to DataTable
		/// </summary>
		/// <returns>DataTable representing object.</returns>
		public override DataTable DataTable ( string tableName = "" )
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
		public override void AddColumns ( ref DataTable dt )
		{
			//add Columns
			dt.Columns.Add ( "MaxTimeSteps", typeof ( int ) );
			dt.Columns.Add ( "NumberPredictionSteps", typeof ( int ) );
			dt.Columns.Add ( "IsSequence", typeof ( int ) );
			dt.Columns.Add ( "NumberPredictionSteps", typeof ( int ) );

		}

		/// <summary>
		/// Add DataRow with object data to DataTable. 
		/// Note: DataRow schema must match the object. (by prior call to AddColumns() )
		/// </summary>
		/// <returns></returns>
		public override void AddDataRow ( ref DataTable dt )
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
		public override void AddRowData ( ref DataRow dr )
		{
			//add data
			dr["MaxTimeSteps"] = MaxTimeSteps;
			dr["NumberPredictionSteps"] = NumberPredictionSteps;
			dr["IsSequence"] = IsSequence;
			dr["NumberPredictionSteps"] = NumberPredictionSteps;
		}

		#endregion
		


#if DEBUG
		public override string ToString()
		{
			return "Nsteps=" + this.NumberPredictionSteps + " Nsyns=" + this.Synapses.Count +
				   (this.ActiveState[Global.T] ? " A" : " ") +
				   (this.IsSequence ? "S" : "") + (this.ActiveState[Global.T - 1] ? "Wa" : "") +
				   (this.WasActiveFromLearning() ? "Wl" : "");
		}
#endif

		#endregion
	}
}
