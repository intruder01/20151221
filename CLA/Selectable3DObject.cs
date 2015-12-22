using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;


namespace OpenHTM.CLA
{
	/// <summary>
	/// Enum of Seelctable object types: Cell, Distal, Prox etc.
	/// </summary>
	public enum SelectableObjectType
	{
		None			=0,
		Cell			=1,
		DistalSynapse	=2,
		ProximalSynapse	=3
	}
	
	
	/// <summary>
	/// Faciliates control of object selection on 3DSimulator screen.
	/// </summary>
	public class Selectable3DObject : IWatchItem
	{
		/// <summary>
		/// True if object has been selected with mouse.
		/// </summary>
		public bool mouseSelected { get; set; }
		/// <summary>
		/// True if mouse cursor is over object.
		/// </summary>
		public bool mouseOver { get; set; }
		/// <summary>
		/// True if object is visible (not used).
		/// </summary>
		public bool isVisible { get; set; }
		/// <summary>
		/// Specifies object type: Cell, DistalSynapse etc. Used in casting Selectable3DObject back to real entity.
		/// </summary>
		public SelectableObjectType SelectablelType;


		/// <summary>
		/// Constructor
		/// </summary>
		public Selectable3DObject ()
		{
			mouseSelected = false;
			mouseOver = false;
			isVisible = false;
			SelectablelType = SelectableObjectType.None;
		}

		


		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public virtual string ID ()
		{
			string str = "";
			//str = String.Format ( "Selectable3DObject [" + Parent.ID() + "]" );
			str = String.Format ( "Sel3DObj" );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public virtual void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
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
			dt.Columns.Add ( "mouseSelected", typeof ( bool ) );
			dt.Columns.Add ( "mouseOver", typeof ( bool ) );
			dt.Columns.Add ( "isVisible", typeof ( bool ) );
			dt.Columns.Add ( "SelectablelType", typeof ( string ) );
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
			dr["mouseSelected"] = mouseSelected;
			dr["mouseOver"] = mouseOver;
			dr["isVisible"] = isVisible;
			dr["SelectablelType"] = SelectablelType.ToString ();
		}

		#endregion
		





	}
}
