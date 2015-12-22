using System;
using System.Data;

namespace OpenHTM.CLA
{
	/// <summary>
	/// A data structure representing a distal synapse. Contains a permanence value and the 
	/// source input to a lower input cell.  
	/// </summary>
	public class DistalSynapse : Synapse
	{
		#region Properties


		/// <summary>
		/// Distal	Segment  which this synapse belongs to.
		/// </summary>
		public DistalSegment DistalSegment { get; private set; }
		/// <summary>
		/// A single input from a neighbour <see cref="Cell"/>.
		/// </summary>
		public Cell InputSource { get; private set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="DistalSynapse"/> class and 
		/// sets its input source and initial permanance values.
		/// </summary>
		/// <param name="distalSeg">
		/// </param>
		/// <param name="inputSource">
		/// An object providing source of the input to this synapse (either 
		/// a <see cref="Column"/>'s <see cref="Cell"/> or a special 
		/// <see cref="InputCell"/>).
		/// </param>
		/// <param name="permanence">Initial permanence value.</param>
		public DistalSynapse(DistalSegment distalSeg, Cell inputSource, float permanence)
		:this ( inputSource, permanence )
		{
			// Set fields
			this.DistalSegment = distalSeg;
		}

		public DistalSynapse ( Cell inputSource, float permanence )
		{
			// Set fields
			this.InputSource = inputSource;
			this.Permanence = permanence;

			SelectablelType = SelectableObjectType.DistalSynapse;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Returns true if this <see cref="DistalSynapse"/> is active due to the current
		/// input.
		/// </summary>
		public override bool IsActive(int t)
		{
			return this.InputSource.ActiveState[t];
		}

		/// <summary>
		/// Returns true if this <see cref="DistalSynapse"/> was active due to the input
		/// previously being in a learning state. 
		/// </summary>
		public override bool IsActiveFromLearning(int t)
		{
			return this.InputSource.ActiveState[t] && this.InputSource.LearnState[t];
		}


		#endregion



		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public new string ID ()
		{
			string str = "";
			//str = String.Format ( "Synapse [" + Parent.ID () + "]" );
			str = String.Format ( "DistalSynapse[{0}]", DistalSegment.ID() );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public new void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			dataSet.Tables.Add ( DistalSegment.DataTable ( tableName ) );//not sure if necc
			dataSet.Tables.Add ( InputSource.DataTable ( tableName ) );//not sure if necc
			base.AddWatchTable ( ref dataSet, tableName );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public new void AddWatchData ( ref DataTable dt )
		{
			AddWatchData ( ref dt );
			base.AddWatchData ( ref dt );
		}

		/// <summary>
		/// Convert object specific data to DataTable
		/// </summary>
		/// <returns>DataTable representing object.</returns>
		public new DataTable DataTable ( string tableName = "" )
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
		public new void AddColumns ( ref DataTable dt )
		{
			//add Columns
			//dt.Columns.Add ( "Column", typeof ( Point ) );
			//dt.Columns.Add ( "Index", typeof ( Single ) );
			//dt.Columns.Add ( "IsSegmentPredicting", typeof ( bool ) );
			//dt.Columns.Add ( "NumberPredictionSteps", typeof ( int ) );
			//dt.Columns.Add ( "PrevNumberPredictionSteps", typeof ( int ) );
		}

		/// <summary>
		/// Add DataRow with object data to DataTable. 
		/// Note: DataRow schema must match the object. (by prior call to AddColumns() )
		/// </summary>
		/// <returns></returns>
		public new DataRow AddDataRow ( ref DataTable dt )
		{
			//add row
			DataRow dr = dt.NewRow ();
			AddRowData ( ref dr );
			return dr;
		}

		/// <summary>
		/// Fill DataRow with object data. 
		/// Note: DataRow schema must match the object. (by prior call to AddColumns() )
		/// </summary>
		/// <returns></returns>
		public new void AddRowData ( ref DataRow dr )
		{
			//add data
			//dr["Column"] = Column.PositionInRegion;
			//dr["Index"] = Index;
			//dr["IsSegmentPredicting"] = IsSegmentPredicting;
			//dr["NumberPredictionSteps"] = NumberPredictionSteps;
			//dr["PrevNumberPredictionSteps"] = PrevNumberPredictionSteps;
		}

		#endregion
		



		

#if DEBUG
		public override string ToString()
		{
			return "Source=" + this.InputSource.ToString() + (this.IsActive(Global.T) ? " A" : " ") +
			       (this.IsConnected() ? "C" : "") + (this.IsActive(Global.T - 1) ? "Wa" : "") +
			       (this.IsActiveFromLearning(Global.T - 1) ? "Wal" : "") + " P=" + this.Permanence;
		}
#endif
	}
}
