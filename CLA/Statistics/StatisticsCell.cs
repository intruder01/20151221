using System;
using System.Data;


namespace OpenHTM.CLA.Statistics
{
	/// <summary>
	/// Statistics for Cells
	/// </summary>
	public class StatisticsCell : Statistics
	{
		/// <summary>
		/// Increments every time this Column's Cell is learning.
		/// </summary>
		public float LearningCounter { get; set; }

		public float NumberSegments { get; set; }

		public float MaxNumberSynapses { get; set; }

		///////////////
		/// <summary>
		/// Convert to DataTable. Combines base class and this class data into a single table.
		/// </summary>
		/// <returns>DataTable representing contents.</returns>
		public DataTable ToDataTableCombined ()
		{
			DataTable dt = new DataTable ( "StatisticsCell" );


			//transfer columns from base table
			DataTable st = base.DataTable ();
			foreach (DataColumn c in st.Columns)
			{
				dt.Columns.Add ( c.ColumnName, c.DataType );
			}
			//add elements
			dt.Columns.Add ( "LearningCounter", typeof ( Single ) );
			dt.Columns.Add ( "NumberSegments", typeof ( Single ) );
			dt.Columns.Add ( "MaxNumberSynapses", typeof ( Single ) );

			DataRow dr = st.NewRow ();

			//transfer data from base table
			foreach (DataColumn c in st.Columns)
			{
				dr[c.ColumnName] = st.Rows[0][c.ColumnName];
			}

			//add data
			dr["LearningCounter"] = LearningCounter;
			dr["NumberSegments"] = NumberSegments;
			dr["MaxNumberSynapses"] = MaxNumberSynapses;
			dt.Rows.Add ( dr );

			return dt;
		}




		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public new string ID ()
		{
			string str = "";
			str = String.Format ( "StatisticsCell" );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public new void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			dataSet.Tables.Add ( base.DataTable ( tableName ) );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public new void AddWatchData ( ref DataTable dt )
		{
			AddColumns ( ref dt );
			AddDataRow ( ref dt );

			//base.AddWatchData ( ref dt );
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
			dt.Columns.Add ( "LearningCounter", typeof ( Single ) );
			dt.Columns.Add ( "NumberSegments", typeof ( Single ) );
			dt.Columns.Add ( "MaxNumberSynapses", typeof ( Single ) );
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
			dr["LearningCounter"] = LearningCounter;
			dr["NumberSegments"] = NumberSegments;
			dr["MaxNumberSynapses"] = MaxNumberSynapses;
		}

		#endregion
		
	}



}
