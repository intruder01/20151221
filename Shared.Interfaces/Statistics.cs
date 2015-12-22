using System;
using System.Data;

namespace OpenHTM.Shared.Interfaces
{
	/// <summary>
	/// Basic Statistics for Cells, Columns, Regions
	/// </summary>
	public class Statistics
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



		/// <summary>
		/// Add Watch informatin to passed DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public void AddWatch ( ref DataSet dataSet )
		{
			DataTable dt = ToDataTable ();
			dataSet.Tables.Add ( dt );
		}

		/// <summary>
		/// Convert to DataTable
		/// </summary>
		/// <returns>DataTable representing contents.</returns>
		public DataTable ToDataTable ()
		{
			DataTable dt = null;

			//Statistics
			dt = new DataTable ( "Statistics" );
			dt.Columns.Add ( "ActivityCounter", typeof ( Single ) );
			dt.Columns.Add ( "ActivityPrecision", typeof ( Single ) );
			dt.Columns.Add ( "ActivityRate", typeof ( Single ) );
			dt.Columns.Add ( "CorrectSegmentPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "CorrectPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "PredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "PredictPrecision", typeof ( Single ) );
			dt.Columns.Add ( "SegmentPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "StepCounter", typeof ( Single ) );

			DataRow dr = dt.NewRow ();
			dr["ActivityCounter"] = ActivityCounter;
			dr["ActivityPrecision"] = ActivityPrecision;
			dr["ActivityRate"] = ActivityRate;
			dr["CorrectSegmentPredictionCounter"] = CorrectSegmentPredictionCounter;
			dr["CorrectPredictionCounter"] = CorrectPredictionCounter;
			dr["PredictionCounter"] = PredictionCounter;
			dr["PredictPrecision"] = PredictPrecision;
			dr["SegmentPredictionCounter"] = SegmentPredictionCounter;
			dr["StepCounter"] = StepCounter;

			return dt;
		}

		
	}






	
}
