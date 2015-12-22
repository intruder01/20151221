using System;
using System.Data;


namespace OpenHTM.CLA.Statistics
{
	/// <summary>
	/// Statistics for Columns.
	/// Mainly extends StatisticsCells to retrieve max-vals from cells
	/// </summary>
	public class StatisticsColumn : StatisticsCell
	{
		/// <summary>
		/// Records steps of this <see cref="Column"/>s activity.
		/// </summary>
		public float ColumnActivityCounter { get; set; }

		/// <summary>
		/// Records correct cell Sequence-Segment-Predictions. Increments with every 
		/// <see cref="Segment"/>, correctly predicted in this Column.
		/// </summary>
		public new float CorrectSegmentPredictionCounter
		{
			get
			{
				return this._correctSegmentPredictionCounter > 0 ? 1.0f : 0.0f;
			}
			set
			{
				this._correctSegmentPredictionCounter = value;
			}
		}

		private float _correctSegmentPredictionCounter;

		/// <summary>
		/// Records correct cell predictions. Increments with every active cell, correctly 
		/// predicted in this Column.
		/// </summary>
		public new float CorrectPredictionCounter
		{
			get
			{
				return this._correctPredictionCounter > 0 ? 1.0f : 0.0f;
			}
			set
			{
				this._correctPredictionCounter = value;
			}
		}

		private float _correctPredictionCounter;

		/// <summary>
		/// Records overall cell predictions. Increments with every predicting cell in this
		/// Column.
		/// </summary>
		public new float PredictionCounter
		{
			get
			{
				return this._predictionCounter > 0 ? 1.0f : 0.0f;
			}
			set
			{
				this._predictionCounter = value;
			}
		}

		private float _predictionCounter;

		/// <summary>
		/// Records Sequence-Segment cell predictions, not overall cell predictions. 
		/// Increments with every predicting <see cref="Segment"/> in this Column.
		/// </summary>
		public new float SegmentPredictionCounter
		{
			get
			{
				return this._segmentPredictionCounter > 0 ? 1.0f : 0.0f;
			}
			set
			{
				this._segmentPredictionCounter = value;
			}
		}

		private float _segmentPredictionCounter;

		public float MaxNumberSegments { get; set; }

		public float MaxLearningCounter { get; set; }

		public float MaxCellActivityCounter { get; set; }

		public float MaxPredictionCounter { get; set; }

		/// <summary>
		/// Gets or sets the max correct prediction counter.
		/// </summary>
		public float MaxCorrectPredictionCounter { get; set; }





		///////////////////////////////

		
		/// <summary>
		/// Convert to DataTable. Combines base class and this class data into a single table.
		/// </summary>
		/// <returns>DataTable representing contents.</returns>
		public new DataTable ToDataTableCombined ()
		{
			DataTable dt = new DataTable ( "StatisticsColumn" );


			//transfer columns from base table
			DataTable st = base.DataTable ();
			foreach (DataColumn c in st.Columns)
			{
				dt.Columns.Add ( c.ColumnName, c.DataType );
			}
			//add elements
			dt.Columns.Add ( "ColumnActivityCounter", typeof ( Single ) );
			dt.Columns.Add ( "CorrectSegmentPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "CorrectPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "PredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "SegmentPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "MaxNumberSegments", typeof ( Single ) );
			dt.Columns.Add ( "MaxLearningCounter", typeof ( Single ) );
			dt.Columns.Add ( "MaxCellActivityCounter", typeof ( Single ) );
			dt.Columns.Add ( "MaxPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "MaxCorrectPredictionCounter", typeof ( Single ) );


			DataRow dr = st.NewRow ();

			//transfer data from base table
			foreach (DataColumn c in st.Columns)
			{
				dr[c.ColumnName] = st.Rows[0][c.ColumnName];
			}

			//add data
			dr["ColumnActivityCounter"] = ColumnActivityCounter;
			dr["CorrectSegmentPredictionCounter"] = CorrectSegmentPredictionCounter;
			dr["CorrectPredictionCounter"] = CorrectPredictionCounter;
			dr["PredictionCounter"] = PredictionCounter;
			dr["SegmentPredictionCounter"] = PredictionCounter;
			dr["MaxNumberSegments"] = MaxNumberSegments;
			dr["MaxLearningCounter"] = MaxLearningCounter;
			dr["MaxCellActivityCounter"] = MaxCellActivityCounter;
			dr["MaxPredictionCounter"] = MaxPredictionCounter;
			dr["MaxCorrectPredictionCounter"] = MaxCorrectPredictionCounter;

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
			str = String.Format ( "StatisticsColumn" );
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


		//for ancestor classes - base class calls this function and renames the table.

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
			dt.Columns.Add ( "ColumnActivityCounter", typeof ( Single ) );
			dt.Columns.Add ( "CorrectSegmentPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "CorrectPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "PredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "SegmentPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "MaxNumberSegments", typeof ( Single ) );
			dt.Columns.Add ( "MaxLearningCounter", typeof ( Single ) );
			dt.Columns.Add ( "MaxCellActivityCounter", typeof ( Single ) );
			dt.Columns.Add ( "MaxPredictionCounter", typeof ( Single ) );
			dt.Columns.Add ( "MaxCorrectPredictionCounter", typeof ( Single ) );
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
			dr["ColumnActivityCounter"] = ColumnActivityCounter;
			dr["CorrectSegmentPredictionCounter"] = CorrectSegmentPredictionCounter;
			dr["CorrectPredictionCounter"] = CorrectPredictionCounter;
			dr["PredictionCounter"] = PredictionCounter;
			dr["SegmentPredictionCounter"] = PredictionCounter;
			dr["MaxNumberSegments"] = MaxNumberSegments;
			dr["MaxLearningCounter"] = MaxLearningCounter;
			dr["MaxCellActivityCounter"] = MaxCellActivityCounter;
			dr["MaxPredictionCounter"] = MaxPredictionCounter;
			dr["MaxCorrectPredictionCounter"] = MaxCorrectPredictionCounter;
		}

		#endregion
		


	}
}
