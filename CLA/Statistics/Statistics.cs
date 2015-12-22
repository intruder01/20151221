using System;
using System.Data;

namespace OpenHTM.CLA.Statistics
{
	/// <summary>
	/// Basic Statistics for Cells, Columns, Regions
	/// </summary>
	public class Statistics : IWatchItem
	{
		/// <summary>
		/// Records active cells.
		/// </summary>
		public float ActivityCounter { get; set; }

		/// <summary>
		/// Quotient of correct Segment predictions to activity counter.
		/// </summary>
		public float ActivityPrecision { get; set; }

		/// <summary>
		/// Quotient of number of steps when cells were active to overall number of steps.
		/// </summary>
		public float ActivityRate { get; set; }

		/// <summary>
		/// Records correct cell Sequence-Segment-Predictions.
		/// </summary>
		public float CorrectSegmentPredictionCounter { get; set; }

		/// <summary>
		/// Records correct cell predictions.
		/// </summary>
		public float CorrectPredictionCounter { get; set; }

		/// <summary>
		/// Records overall cell predictions
		/// </summary>
		public float PredictionCounter { get; set; }

		/// <summary>
		/// Quotient of correct Segment predictions to overall Segment predictions.
		/// </summary>
		public float PredictPrecision { get; set; }

		/// <summary>
		/// Records Sequence-Segment cell predictions, not overall cell predictions
		/// </summary>
		public float SegmentPredictionCounter { get; set; }

		/// <summary>
		/// Increments at every time step.
		/// </summary>
		public float StepCounter { get; set; }



		#region IWatchItem

		/// <summary>
		/// Object's identification string based on it's position within parent.
		/// Used primarly in Watch.
		/// </summary>
		/// <returns></returns>
		public virtual string ID ()
		{
			string str = "";
			str = String.Format ( "Statistics" );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public virtual void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( this.DataTable ( tableName ) );
			//dataSet.Tables.Add ( base.DataTable () );
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
			DataTable dt = new DataTable ( this.ID () + " " + tableName );
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
			dt.Columns.Add ( "ActivityCounter", typeof ( Single ) );
			dt.Columns.Add ( "ActivityPrecision", typeof ( Single ) );
			dt.Columns.Add ( "ActivityRate", typeof ( Single ) );
			dt.Columns.Add ( "CorrectSegmentPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "CorrectPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "PredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "PredictPrecision", typeof ( Single ) );
			dt.Columns.Add ( "SegmentPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "StepCounter", typeof ( Single ) );
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
			dr["ActivityCounter"] = ActivityCounter;
			dr["ActivityPrecision"] = ActivityPrecision;
			dr["ActivityRate"] = ActivityRate;
			dr["CorrectSegmentPredictionCounter"] = CorrectSegmentPredictionCounter;
			dr["CorrectPredictionCounter"] = CorrectPredictionCounter;
			dr["PredictionCounter"] = PredictionCounter;
			dr["PredictPrecision"] = PredictPrecision;
			dr["SegmentPredictionCounter"] = SegmentPredictionCounter;
			dr["StepCounter"] = StepCounter;
		}

		#endregion
		
		
	}






	
}
