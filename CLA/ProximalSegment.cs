using System;
using System.Data;

namespace OpenHTM.CLA
{

	



	/// <summary>
	/// Represents a single dendrite segment that forms synapses (connections) to other 
	/// cells in lower regions.
	/// </summary>
	/// <remarks>
	/// Each Segment also maintains a boolean flag indicating 
	/// whether the Segment predicts feed-forward input on the next time step.
	/// Proximal segments are used in spatial pooling.  Segments are considered 'active' 
	/// if enough of its existing synapses are connected and individually active.
	/// </remarks>
	public class ProximalSegment : Segment
	{

		#region Fields

		/// <summary>
		/// Column which this segment belongs to.
		/// </summary>
		public Column parentColumn { get; private set; }


		#endregion



		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="Segment"/> class.
		/// </summary>
		internal ProximalSegment(Column column)
			: base()
		{
			this.parentColumn = column;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Create a new synapse for this segment attached to the specified input cell.
		/// </summary>
		/// <param name="inputSource">the input source of the synapse to create.</param>
		/// <param name="initialPermanence">the initial permanence of the synapse.</param>
		internal void CreateSynapse(InputCell inputSource, double initialPermanence)
		{
			var newSynapse = new ProximalSynapse(this, inputSource, (float)initialPermanence);
			this.Synapses.Add(newSynapse);
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
			str = String.Format ( "ProximalSeg[{0}]", parentColumn.ID () );
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
			//dt.Columns.Add ( "MaxTimeSteps", typeof ( int ) );
			//dt.Columns.Add ( "NumberPredictionSteps", typeof ( int ) );
			//dt.Columns.Add ( "IsSequence", typeof ( int ) );
			//dt.Columns.Add ( "NumberPredictionSteps", typeof ( int ) );

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
			//dr["MaxTimeSteps"] = MaxTimeSteps;
			//dr["NumberPredictionSteps"] = NumberPredictionSteps;
			//dr["IsSequence"] = IsSequence;
			//dr["NumberPredictionSteps"] = NumberPredictionSteps;
		}

		#endregion
		


#if DEBUG
		public override string ToString()
		{
			return " Nsyns=" + this.Synapses.Count +
				   (this.ActiveState[Global.T] ? " A" : " ") +
				   (this.ActiveState[Global.T - 1] ? "Wa" : "");
		}
#endif

		#endregion
	}
}
