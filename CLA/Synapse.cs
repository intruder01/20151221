using System;
using System.Data;


namespace OpenHTM.CLA
{
	/// <summary>
	/// A data structure representing a synapse. Contains a permanence value to
	/// indicate connectivity to a target cell.  
	/// </summary>
	public class Synapse : Selectable3DObject
	{
		#region Properties


		/// <summary>
		/// Synapses with permanences above this value are connected.
		/// </summary>
		/// <remarks>Lays within (0; 1] interval.</remarks>
		public static float ConnectedPermanence { get; set; }

		/// <summary>
		/// Initial permanence value for distal synapses.
		/// </summary>
		/// <remarks>Lays within [0; 1] interval.</remarks>
		public static float InitialPermanence { get; set; }

		/// <summary>
		/// Decrement value for synapses that are decremented in learning.
		/// </summary>
		/// <remarks>Lays within (0; 1) interval.</remarks>
		public static float PermanenceDecrement { get; set; }

		/// <summary>
		/// Increment value for synapses that are incremented in learning.
		/// </summary>
		/// <remarks>Lays within (0; 1) interval.</remarks>
		public static float PermanenceIncrement { get; set; }

		/// <summary>
		/// A value to indicate connectivity to a target cell.  
		/// </summary>
		public float Permanence { get; protected set; }

		#endregion

		/// <summary>
		/// Initializes the <see cref="Synapse"/> class.
		/// </summary>
		public Synapse()
		{
			ConnectedPermanence = 0.2f;
			InitialPermanence = ConnectedPermanence + 0.1f;
			PermanenceDecrement = 0.01f;
			PermanenceIncrement = 0.015f;
		}

		#region Methods

		/// <summary>
		/// When overriden in derived class, returns true if this synapse is active due to the 
		/// current input.
		/// </summary>
		public virtual bool IsActive ( int t )
		{
			return false;
		}

		/// <summary>
		/// When overriden in derived class, returns true if this synapse is active due to the 
		/// input being in a learning state. 
		/// </summary>
		public virtual bool IsActiveFromLearning(int t)
		{
			return false;
		}

		/// <summary>
		/// Returns true if this <see cref="Synapse"/> is currently connecting its source
		/// and destination <see cref="Cell"/>s.
		/// </summary>
		public bool IsConnected()
		{
			return this.Permanence >= ConnectedPermanence;
		}

		/// <summary>
		/// Decrease the permance value of the synapse
		/// </summary>
		public void DecreasePermanence()
		{
			this.Permanence = Math.Max(0.0f, this.Permanence - PermanenceDecrement);
		}

		/// <summary>
		/// Decrease the permance value of the synapse
		/// </summary>
		/// <param name="value">Value to decrease</param>
		public void DecreasePermanence(float value)
		{
			this.Permanence = Math.Max(0.0f, this.Permanence - value);
		}

		/// <summary>
		/// Increase the permanence value of the synapse
		/// </summary>
		public void IncreasePermanence()
		{
			this.Permanence = Math.Min(1.0f, this.Permanence + PermanenceIncrement);
		}

		/// <summary>
		/// Increase the permanence value of the synapse
		/// </summary>
		/// <param name="value">Value to increase</param>
		public void IncreasePermanence(float value)
		{
			this.Permanence = Math.Min(1.0f, this.Permanence + value);
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
			str = String.Format ( "Synapse" );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public new void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			base.AddWatchTable ( ref dataSet, tableName );
		}
		/// <summary>
		/// Add Watch columns and data to existing DataTable
		/// </summary>
		/// <param name="dt"></param>
		public new void AddWatchData ( ref DataTable dt )
		{
			AddColumns ( ref dt );
			AddDataRow ( ref dt );

			base.AddWatchData ( ref dt );
		}

		/// <summary>
		/// Convert object specific data to DataTable
		/// </summary>
		/// <returns>DataTable representing object.</returns>
		public new DataTable DataTable ( string tableName = "" )
		{
			DataTable dt = new DataTable ( this.ID () + " " + tableName );
			AddColumns ( ref dt );
			AddDataRow (ref dt );
			return dt;
		}

		/// <summary>
		/// Add object specific columns to DataTable
		/// </summary>
		/// <returns>DataTable representing contents.</returns>
		public new void AddColumns ( ref DataTable dt )
		{
			//add Columns
			dt.Columns.Add ( "ConnectedPermanence", typeof ( float ) );
			dt.Columns.Add ( "InitialPermanence", typeof ( float ) );
			dt.Columns.Add ( "PermanenceDecrement", typeof ( float ) );
			dt.Columns.Add ( "PermanenceIncrement", typeof ( float ) );
			dt.Columns.Add ( "Permanence", typeof ( float ) );

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
			dr["ConnectedPermanence"] = ConnectedPermanence;
			dr["InitialPermanence"] = InitialPermanence;
			dr["PermanenceDecrement"] = PermanenceDecrement;
			dr["PermanenceIncrement"] = PermanenceIncrement;
			dr["Permanence"] = Permanence;
		}

		#endregion
		
		
	}
}
