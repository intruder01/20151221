using System;
using System.Data;

namespace OpenHTM.CLA
{
	/// <summary>
	/// Represents a synapse that receives feed-forward input from an input cell.
	/// </summary>
	public class ProximalSynapse : Synapse
	{
		#region Properties

		/// <summary>
		/// Segment which this synapse belongs to.
		/// </summary>
		public ProximalSegment ProximalSegment { get; private set; }
		/// <summary>
		/// A single input bit from an external source.
		/// </summary>
		public InputCell InputSource { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ProximalSynapse"/> class and 
		/// sets its input source and initial permanance values.
		/// </summary>
		/// <param name="proximalSeg">
		/// </param>
		/// <param name="inputSource">
		/// An <see cref="InputCell"/> providing source of the input to this synapse.
		/// </param>
		/// <param name="permanence">Initial permanence value.</param>
		internal ProximalSynapse(ProximalSegment proximalSeg, InputCell inputSource, float permanence)
		{
			// Set fields
			this.ProximalSegment = proximalSeg;
			this.InputSource = inputSource;
			this.Permanence = permanence;

			SelectablelType = SelectableObjectType.ProximalSynapse;
		}

		#endregion


		#region Methods

		/// <summary>
		/// Returns true if this <see cref="ProximalSynapse"/> is active due to the 
		/// current input.
		/// InputSource is the correspondent coordinate(x,y) in the input vector which the synapse is connected.
		/// </summary>
		public override bool IsActive(int t)
		{
			return this.InputSource.IsActive(t);
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
			str = String.Format ( "ProximalSynapse[{0}]", ProximalSegment.ID() );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public new void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			dataSet.Tables.Add ( ProximalSegment.DataTable ( tableName ) );//not sure if necc
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
		

	}
}
