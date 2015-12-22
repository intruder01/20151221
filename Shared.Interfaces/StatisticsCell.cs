using System;
using System.Data;


namespace OpenHTM.Shared.Interfaces
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
			DataTable st = base.ToDataTable ();
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




		
	}



}
