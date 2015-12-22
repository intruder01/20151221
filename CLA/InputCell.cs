using System;
using System.Data;

namespace OpenHTM.CLA
{
	/// <summary>
	/// Represent a single input bit from an external source.
	/// </summary>
	public class InputCell : IWatchItem
	{
		#region Fields

		/// <summary>
		/// Region which receives the input.
		/// </summary>
		private Region _region;

		#endregion

		#region Properties

		/// <summary>
		/// The X position of this cell in the dimensional plane of the input
		/// </summary>
		public int X { get; set; }

		/// <summary>
		/// The Y position of this cell in the dimensional plane of the input
		/// </summary>
		public int Y { get; set; }

		#endregion

		#region Constructor

		internal InputCell(Region region, int x, int y)
		{
			// Set fields
			this._region = region;
			this.X = x;
			this.Y = y;
		}

		#endregion

		#region Methods 

		/// <summary>
		/// Returns true if this <see cref="InputCell"/> is active due to the 
		/// current input.
		/// </summary>
		public bool IsActive(int t)
		{
			bool isActive;

			try
			{
				isActive = (this._region.Input[t][this.X, this.Y] == 1);
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new Exception("Input and region sizes don't match!", ex);
			}

			return isActive;
		}

		#endregion



		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public virtual string ID ()
		{
			string str = "";
			//str = String.Format ( "Synapse [" + Parent.ID () + "]" );
			str = String.Format ( "InputCell[" + "X{0} Y{1} R{2}]", X, Y, _region.Index );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public virtual void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			//base.AddWatchTable ( ref dataSet );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public virtual void AddWatchData ( ref DataTable dt )
		{
			AddColumns ( ref dt );
			AddDataRow ( ref dt );

			//base.AddWatchData ( ref dt );
		}

		/// <summary>
		/// Convert object specific data to DataTable
		/// </summary>
		/// <returns>DataTable representing object.</returns>
		public virtual DataTable DataTable ( string tableName = "" )
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
		public virtual void AddColumns ( ref DataTable dt )
		{
			//add Columns
			dt.Columns.Add ( "X", typeof ( int ) );
			dt.Columns.Add ( "Y", typeof ( int ) );
		}

		/// <summary>
		/// Add DataRow with object data to DataTable. 
		/// Note: DataRow schema must match the object. (by prior call to AddColumns() )
		/// </summary>
		/// <returns></returns>
		public virtual void AddDataRow ( ref DataTable dt )
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
		public virtual void AddRowData ( ref DataRow dr )
		{
			//add data
			dr["X"] = X;
			dr["Y"] = Y;
		}

		#endregion
		
	
	
	
	
	
	}
}
