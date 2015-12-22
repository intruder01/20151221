using System;
using System.Data;
using System.Collections.Generic;

namespace OpenHTM.CLA
{
	/// <summary>
	/// Represents a single dendrite segment that forms synapses (connections) to other 
	/// Cells.
	/// </summary>
	/// <remarks>
	/// Each Segment also maintains a boolean flag indicating 
	/// whether the Segment predicts feed-forward input on the next time step.
	/// Segments can be either proximal or distal (for spatial pooling or temporal pooling 
	/// respectively) however the class object itself does not need to know which
	/// it ultimately is as they behave identically.  Segments are considered 'active' 
	/// if enough of its existing synapses are connected and individually active.
	/// </remarks>
	public class Segment : Selectable3DObject
	{
		#region Properties


		/// <summary>
		/// Activation threshold for a segment. If the number of active connected
		/// synapses in a segment is greater than ActivationThreshold, the segment
		/// is said to be active.
		/// Original name: activationThreshold
		/// </summary>
		internal static float ActivationThreshold { get; set; }

		/// <summary>
		/// The synapses list.
		/// </summary>
		public List<Synapse> Synapses { get; private set; }

		/// <summary>
		/// Returns true if the number of connected synapses on this 
		/// <see cref="Segment"/> that are active due to active states at time t is 
		/// greater than ActivationThreshold.
		/// </summary>
		public List<bool> ActiveState = new List<bool>();

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Segment"/> class.
		/// </summary>
		internal Segment()
		{
			this.Synapses = new List<Synapse>();

			// Initialize state vectors with fixed lenght = T
			for (int t = 0; t <= Global.T; t++)
			{
				this.ActiveState.Add(false);
			}
		}

		#endregion

		#region Methods

		///<summary>
		/// Advance this segment to the next time step.
		///</summary>
		///<remarks>
		/// The current state of this segment (active, number of synapes) will be  
		/// the previous state and the current state will be reset to no cell activity by 
		/// default until it can be determined.
		///</remarks>
		public void NextTimeStep()
		{
			// Remove the element in first position in order to the state vector always have lenght = T
			// and add a new element to represent the state at current time step
			this.ActiveState.RemoveAt(0);
			this.ActiveState.Add(false);

			//TODO: possibly cache "was connected" for all synapses here
			//foreach (Synapse synapse in Synapses)
			//{
			//	synapse.nextTimeStep();
			//}
		}

		///<summary>
		/// Process this segment for the current time step.
		///</summary>
		///<remarks>
		/// Processing will determine the set of active synapses on this segment for this time 
		/// step.  From there we will determine if this segment is active if enough active 
		/// synapses are present.  This information is then cached for the remainder of the
		/// Region's processing for the time step.  When a new time step occurs, the
		/// Region will call NextTimeStep() on all cells/segments to cache the 
		/// information as what was previously active.
		///</remarks>
		public void ProcessSegment()
		{
			this.ActiveState[Global.T] = this.GetActiveConnectedSynapses(Global.T).Count >= ActivationThreshold;
		}

		///<summary>
		/// Gets the the list of all active synapses as computed as of the most recently processed
		/// time step for this segment.
		///</summary>
		public List<Synapse> GetActiveSynapses(int t)
		{
			var activeSynapses = new List<Synapse>();
			foreach (var synapse in this.Synapses)
			{
				if (synapse.IsActive(t))
				{
					activeSynapses.Add(synapse);
				}
			}

			return activeSynapses;
		}

		/// <summary>
		/// Return a list of all the synapses that are currently connected (those with a
		/// permanence value above the threshold).
		/// </summary>
		/// <returns></returns>
		public List<Synapse> GetConnectedSynapses()
		{
			var connectedSynapses = new List<Synapse>();
			foreach (var synapse in this.Synapses)
			{
				if (synapse.IsConnected())
				{
					connectedSynapses.Add(synapse);
				}
			}
			return connectedSynapses;
		}

		///<summary>
		/// Gets the active and connected (with permanence > ConnectedPermanence)
		/// synapses in the given time step.
		///</summary>
		public List<Synapse> GetActiveConnectedSynapses(int t)
		{
			var activeConnectedSynapses = new List<Synapse>();
			foreach (var activeSynapse in this.GetActiveSynapses(t))
			{
				if (activeSynapse.IsConnected())
				{
					activeConnectedSynapses.Add(activeSynapse);
				}
			}

			return activeConnectedSynapses;
		}

		#endregion


		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public override string ID ()
		{
			string str = "";
			//str = String.Format ( "Selectable3DObject [" + Parent.ID() + "]" );
			str = String.Format ( "Segment" );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public override void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			dataSet.Tables.Add ( base.DataTable ( tableName ) );

			//add separate table for Synapses List
			DataTable dt = new DataTable ();
			if (Synapses.Count > 0)
			{
				Synapses[0].AddColumns ( ref dt );

				foreach (var syn in Synapses)
				{
					syn.AddDataRow ( ref dt );
				}
			}
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public override void AddWatchData ( ref DataTable dt )
		{
			AddWatchData ( ref dt );
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
			dt.Columns.Add ( "ActivationThreshold", typeof ( Single ) );
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
			dr["ActivationThreshold"] = ActivationThreshold;
		}

		

		#endregion
	}
}
