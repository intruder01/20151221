using System;
using System.Data;
using System.Linq;



namespace OpenHTM.CLA
{
	interface IWatchItem
	{
		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		string ID ();

		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		void AddWatchTable ( ref DataSet dataSet, string tableName = "" );
		
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		void AddWatchData ( ref DataTable dt );


		/// <summary>
		/// Convert object specific data to DataTable
		/// </summary>
		/// <returns>DataTable representing object.</returns>
		DataTable DataTable (string tableName="");


		/// <summary>
		/// Add object Watch columns to DataTable
		/// </summary>
		/// <returns></returns>
		void AddColumns ( ref DataTable dt );
		
		/// <summary>
		/// Add DataRow with object data to DataTable. 
		/// Note: DataRow schema must match the object. (by prior call to AddColumns() )
		/// </summary>
		/// <returns></returns>
		void AddDataRow ( ref DataTable dt );

		/// <summary>
		/// Fill DataRow with object data. 
		/// Note: DataRow schema must match the object. (by prior call to AddColumns() )
		/// </summary>
		/// <returns></returns>
		void AddRowData ( ref DataRow dr );


	}
}
