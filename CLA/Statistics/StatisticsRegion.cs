using System;
using System.Data;

namespace OpenHTM.CLA.Statistics
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



		//////////////////////////////////////

		/// <summary>
		/// Convert to DataTable. Combines base class and this class data into a single table.
		/// </summary>
		/// <returns>DataTable representing contents.</returns>
		public DataTable ToDataTableCombined ()
		{
			DataTable dt = new DataTable ( "StatisticsRegion" );


			//transfer columns from base table
			DataTable st = base.DataTable ();
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
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			dataSet.Tables.Add ( base.DataTable ( tableName ) );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public override void AddWatchData ( ref DataTable dt )
		{
			AddColumns ( ref dt );
			AddDataRow ( ref dt );

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
			dt.Columns.Add ( "ColumnActivationAccuracy", typeof ( Single ) );
			dt.Columns.Add ( "ColumnPredictionAccuracy", typeof ( Single ) );
			dt.Columns.Add ( "NumberActiveColumns", typeof ( int ) );
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
			dr["ColumnActivationAccuracy"] = ColumnActivationAccuracy;
			dr["ColumnPredictionAccuracy"] = ColumnPredictionAccuracy;
			dr["NumberActiveColumns"] = NumberActiveColumns;
		}

		#endregion
		

	}
}
