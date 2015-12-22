using System;
using System.Collections.Generic;
using System.Data;

namespace OpenHTM.CLA
{
	/// <summary>
	/// This data structure holds three pieces of information required to update a given 
	/// segment: 
	/// a) segment reference (None if it's a new segment), 
	/// b) a list of existing active synapses, and 
	/// c) a flag indicating whether this segment should be marked as a sequence
	/// segment (defaults to false).
	/// The structure also determines which learning cells (at this time step) are available 
	/// to connect (add synapses to) should the segment get updated. If there is a locality 
	/// radius set on the Region, the pool of learning cells is restricted to those with 
	/// the radius.
	/// </summary>
	public class SegmentUpdate : IWatchItem
	{
		#region Fields

		private static Random _random = new Random(4242);

		#endregion

		#region Properties

		/// <summary>
		/// Cell which owns the segment.
		/// </summary>
		public Cell Cell { get; set; }

		/// <summary>
		/// Segment (null if it's a new segment).
		/// </summary>
		public DistalSegment DistalSegment { get; set; }

		/// <summary>
		/// List of active synapses where the originating cells have their ActiveState 
		/// output = true at time step t.
		/// </summary>
		public List<Synapse> ActiveDistalSynapses { get; set; }

		/// <summary>
		/// Indicates whether is to add new synapses.
		/// </summary>
		public bool AddNewSynapses { get; set; }

		/// <summary>
		/// Cells which a segment will be created from a cell to them.
		/// </summary>
		public List<Cell> CellsToConnect { get; set; }

		public int NumberPredictionSteps { get; set; }

		public int CreationStep { get; set; }

		#endregion

		#region Constructor


		/// <summary>
		/// Parameterless constructor for serialization.
		/// </summary>
		public SegmentUpdate ()
		{}
		#endregion

		#region Methods

		/// <summary>
		/// Randomly sample values (numberRandomSamples) from the Cell array of length n (numberRandomSamples less than n).
		/// Runs in O(2 numberRandomSamples) worst case time.
		/// Result is written to the result array of length m containing the randomly chosen cells.
		/// </summary>
		/// <param name="cells">input Cells to randomly choose from.</param>
		/// <param name="numberRandomSamples">the number of random samples to take 
		///   (numberRandomSamples less than equal to resultCells.Length)</param>
		private List<Cell> ChooseRandomCells(List<Cell> cells, int numberRandomSamples)
		{
			var resultCells = new Cell[numberRandomSamples];

			int numberCells = cells.Count;
			int currentLenght = 0;
			for (int i = numberCells - numberRandomSamples; i < numberCells; i++)
			{
				int position = _random.Next(i + 1);
				Cell cell = cells[position];

				// If (subset contains item already) then use item[i] instead
				bool contains = false;
				for (int j = 0; j < currentLenght; ++j)
				{
					Cell initialResultCell = resultCells[j];
					if (initialResultCell == cell)
					{
						contains = true;
						break;
					}
				}
				if (contains)
				{
					resultCells[currentLenght] = cells[i];
				}
				else
				{
					resultCells[currentLenght] = cell;
				}

				currentLenght++;
			}

			return new List<Cell>(resultCells);
		}

		///<summary>
		///Create a new SegmentUpdate that is to modify the state of the Region
		///either by adding a new segment to a cell, new synapses to a segemnt,
		///or updating permanences of existing synapses on some segment.
		///</summary>
		///<param name="cell">cell the cell that is to have a segment added or updated.
		///  </param>
		///<param name="distalSegment">the segment that is to be updated (null here means a new
		///  segment is to be created on the parent cell).</param> 
		///<param name="activeDistalSynapses">the set of active synapses on the segment 
		///  that are to have their permanences updated.</param> 
		///<param name="addNewSynapses">set to true if new synapses are to be added to the
		///  segment (or if new segment is being created) or false if no new synapses
		///  should be added instead only existing permanences updated.</param> 
		///
		public SegmentUpdate(Cell cell, DistalSegment distalSegment, List<Synapse> activeDistalSynapses,
		                     bool addNewSynapses = false)
		{
			// Set fields
			this.Cell = cell;
			this.DistalSegment = distalSegment;
			this.ActiveDistalSynapses = new List<Synapse>(activeDistalSynapses);
			this.AddNewSynapses = addNewSynapses;
			this.CellsToConnect = new List<Cell>();
			this.NumberPredictionSteps = 1;
			this.CreationStep = (int) cell.Statistics.StepCounter;

			// Set of cells that have LearnState output = true at time step t.
			var cellsToConnect = new List<Cell>();

			// If adding new synapses, find the current set of learning cells within
			// the Region and select a random subset of them to connect the segment to.
			// Do not add > 1 synapse to the same cell on a given segment
			Region region = this.Cell.Column.Region;

			if (this.AddNewSynapses)
			{
				// Gather all cells from segment in order to avoid use them as learning cells.
				var cellsInSegment = new List<Cell>();
				if (distalSegment != null)
				{
					foreach (var synapse in distalSegment.Synapses)
					{
						if (synapse is DistalSynapse)
						{
							cellsInSegment.Add(((DistalSynapse) synapse).InputSource);
						}
					}
				}

				// Define limits to choose cells to connect.
				int minY, maxY, minX, maxX;
				if (region.LocalityRadius > 0)
				{
					// Only allow connecting to Columns within locality radius
					minX = Math.Max(0, cell.Column.PositionInRegion.X - region.LocalityRadius);
					minY = Math.Max(0, cell.Column.PositionInRegion.Y - region.LocalityRadius);
					maxX = Math.Min(region.Size.Width - 1,
					                cell.Column.PositionInRegion.X + region.LocalityRadius);
					maxY = Math.Min(region.Size.Height - 1,
					                cell.Column.PositionInRegion.Y + region.LocalityRadius);
				}
				else
				{
					// Now I want to allow connections over all the region!
					// If locality radius is 0, it means 'no restriction'
					minX = 0;
					minY = 0;
					maxX = region.Size.Width - 1;
					maxY = region.Size.Height - 1;
				}

				// Set of cells that have LearnState output = true at time step t.
				for (int x = minX; x <= maxX; x++)
				{
					for (int y = minY; y <= maxY; y++)
					{
						Column column = region.GetColumn(x, y);

						// Skip cells in our own column (don't connect to ourself)
						if (column != cell.Column)
						{
							foreach (var learningCell in column.Cells)
							{
								// Skip cells that already are linked to the segment (case it exists).
								if (learningCell.LearnState[Global.T - 1] && !cellsInSegment.Contains(learningCell))
								{
									cellsToConnect.Add(learningCell);
								}
							}
						}
					}
				}
			}

			// Basic allowed number of new Synapses
			int newNumberSynapses = region.NumberNewSynapses;
			if (distalSegment != null)
			{
				newNumberSynapses = Math.Max(0, newNumberSynapses - activeDistalSynapses.Count);
			}

			// Clamp at learn cells
			newNumberSynapses = Math.Min(cellsToConnect.Count, newNumberSynapses);

			// Randomly choose (newNumberSynapses) learning cells to add connections to
			if (cellsToConnect.Count > 0 && newNumberSynapses > 0)
			{
				this.CellsToConnect = this.ChooseRandomCells(cellsToConnect, newNumberSynapses);
			}
		}

		///<summary>
		/// Create a new segment on the update cell using connections from
		/// the set of learning cells for the update info.
		///</summary>
		internal DistalSegment CreateDistalSegment()
		{
			DistalSegment distalSegment = this.Cell.CreateDistalSegment(this.CellsToConnect);
			distalSegment.NumberPredictionSteps = this.NumberPredictionSteps;
			return distalSegment;
		}

		///<summary>
		/// Create new synapse connections to the segment to be updated using
		/// the set of learning cells in this update info.
		///</summary>
		internal void CreateDistalSynapses()
		{
			foreach (var cell in this.CellsToConnect)
			{
				this.DistalSegment.CreateSynapse(cell, Synapse.InitialPermanence);
			}
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
			str = String.Format ( "SegmentUpdate" );
			return str;
		}
		/// <summary>
		/// Add new DataTable to existing DataSet
		/// </summary>
		/// <param name="dataSet"></param>
		public virtual void AddWatchTable ( ref DataSet dataSet, string tableName = "" )
		{
			dataSet.Tables.Add ( DataTable ( tableName ) );
			this.DistalSegment.AddWatchTable ( ref dataSet, tableName );

			//ActiveDistalSynapses
			foreach (Synapse syn in ActiveDistalSynapses)
			{
				syn.AddWatchTable ( ref dataSet, tableName );
			}

			//CellsToConnect - List
			DataTable dt = new DataTable ( "CellsToConnect" );
			dt.Columns.Add ( "CellID", typeof ( string ) );
			foreach (Cell cell in CellsToConnect)
			{
				DataRow dr = dt.NewRow ();
				dr["CellID"] = cell.ID ();
				dt.Rows.Add ( dr );
			}
			dataSet.Tables.Add ( dt );
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
			dt.Columns.Add ( "CellID", typeof ( string ) );
			dt.Columns.Add ( "AddNewSynapses", typeof ( bool ) );
			dt.Columns.Add ( "NumberPredictionSteps", typeof ( int ) );
			dt.Columns.Add ( "CreationStep", typeof ( int ) );
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
			dr["CellID"] = this.Cell.ID ();
			dr["AddNewSynapses"] = AddNewSynapses;
			dr["NumberPredictionSteps"] = NumberPredictionSteps;
			dr["CreationStep"] = CreationStep;
		}

		#endregion
		

#if DEBUG
		/// <summary>
		/// Returns a string that represents the current object.
		/// </summary>
		/// <returns>
		/// A string that represents the current object.
		/// </returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return "Cell=" + this.Cell + " Nsteps=" + this.NumberPredictionSteps +
			       " Sgmnt=" + this.DistalSegment + " NewSyns=" +
			       (this.AddNewSynapses ? "+" : "-");
		}
#endif

		#endregion
	}
}
