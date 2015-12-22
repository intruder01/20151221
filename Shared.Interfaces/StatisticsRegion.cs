using System;
using System.Data;

namespace OpenHTM.Shared.Interfaces
{
	/// <summary>
	/// Statistics for Region
	/// </summary>
	public class StatisticsRegion : Statistics
	{
		/// <summary>
		/// This is the number of correctly predicted active columns
		/// out of the total active columns from the most recently run time step.  
		/// </summary>
		public float ColumnActivationAccuracy { get; set; }

		/// <summary>
		/// This is the number of correctly predicted active columns out of the 
		/// total sequence-segment predicted columns from the most recently run time step.
		/// </summary>
		public float ColumnPredictionAccuracy { get; set; }

		/// <summary>
		/// The total number of active columns in the Region as calculated from
		/// the most recently run time step.  For hardcoded Regions this number will match
		/// the number of 1 values from the input data.  For spatial pooling Regions this
		/// number will represent the number of winning columns after inhibition.
		/// </summary>
		public int NumberActiveColumns { get; set; }


		/// <summary>
		/// Add Watch information to passed DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public void AddWatch ( ref DataSet dataSet )
		{
			dataSet.Tables.Add ( ToDataTable () );
			dataSet.Tables.Add ( base.ToDataTable () );
		}



		/// <summary>
		/// Convert to DataTable. Combines base class and this class data into a single table.
		/// </summary>
		/// <returns>DataTable representing contents.</returns>
		public new DataTable ToDataTable ()
		{
			DataTable dt = new DataTable ( "StatisticsRegion" );

			//add elements
			dt.Columns.Add ( "ColumnActivationAccuracy", typeof ( Single ) );
			dt.Columns.Add ( "ColumnPredictionAccuracy", typeof ( Single ) );
			dt.Columns.Add ( "NumberActiveColumns", typeof ( int ) );

			DataRow dr = dt.NewRow ();

			//add data
			dr["ColumnActivationAccuracy"] = ColumnActivationAccuracy;
			dr["ColumnPredictionAccuracy"] = ColumnPredictionAccuracy;
			dr["NumberActiveColumns"] = NumberActiveColumns;
			dt.Rows.Add ( dr );

			return dt;
		}
		/// <summary>
		/// Convert to DataTable. Combines base class and this class data into a single table.
		/// </summary>
		/// <returns>DataTable representing contents.</returns>
		public DataTable ToDataTableCombined ()
		{
			DataTable dt = new DataTable ( "StatisticsRegion" );


			//transfer columns from base table
			DataTable st = base.ToDataTable ();
			foreach (DataColumn c in st.Columns)
			{
				dt.Columns.Add ( c.ColumnName, c.DataType );
			}
			//add elements
			dt.Columns.Add ( "ColumnActivationAccuracy", typeof ( Single ) );
			dt.Columns.Add ( "ColumnPredictionAccuracy", typeof ( Single ) );
			dt.Columns.Add ( "NumberActiveColumns", typeof ( int ) );

			DataRow dr = st.NewRow ();

			//transfer data from base table
			foreach (DataColumn c in st.Columns)
			{
				dr[c.ColumnName] = st.Rows[0][c.ColumnName];
			}

			//add data
			dr["ColumnActivationAccuracy"] = ColumnActivationAccuracy;
			dr["ColumnPredictionAccuracy"] = ColumnPredictionAccuracy;
			dr["NumberActiveColumns"] = NumberActiveColumns;
			dt.Rows.Add ( dr );

			return dt;
		}


		//////////////////////


		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public override string ID ()
		{
			string str = "";
			str = String.Format ( "StatisticsRegion" );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public override void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( DataTable ( tableName ) );
			base.AddWatchTable ( ref dataSet, tableName );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public override void AddWatchData ( ref DataTable dt )
		{
			this.AddColumns ( ref dt );
			this.AddDataRow ( ref dt );

			//base.AddWatchData ( ref dt );
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
		public override DataRow AddDataRow ( ref DataTable dt )
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
		public override void AddRowData ( ref DataRow dr )
		{
			//add data
			dr["Column"] = Column.PositionInRegion;
			dr["Index"] = Index;
			dr["IsSegmentPredicting"] = IsSegmentPredicting;
			dr["NumberPredictionSteps"] = NumberPredictionSteps;
			dr["PrevNumberPredictionSteps"] = PrevNumberPredictionSteps;
		}

		#endregion
		

	}
}
