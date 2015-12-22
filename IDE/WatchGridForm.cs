using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data;
using OpenHTM.CLA;
using OpenHTM.IDE.UIControls;
using WeifenLuo.WinFormsUI.Docking;
using Region = OpenHTM.CLA.Region;

namespace OpenHTM.IDE
{
	public partial class WatchGridForm : DockContent
	{
		#region Fields

		// Private singleton instance
		private static WatchGridForm _instance;

		private List<Cell> WatchListCells;
		private List<DistalSynapse> WatchListDistal;
		private List<ProximalSynapse> WatchListProximal;
		private DataSet ds;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static WatchGridForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new WatchGridForm ();
					_instance.ds = new DataSet ();
				}
				return _instance;
			}
		}


		#endregion

		#region Nested classes



		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WatchGridForm"/> class.
		/// </summary>
		public WatchGridForm ()
		{
			this.InitializeComponent ();

			// Set UI properties
			this.MdiParent = MainForm.Instance;

			WatchListCells = new List<Cell> ();
			WatchListDistal = new List<DistalSynapse> ();
			WatchListProximal = new List<ProximalSynapse> ();
			ds = new DataSet ();
		}

		#endregion

		#region Methods

		public void InitializeParams ()
		{
			
		}
		#endregion

		#region Events



		#endregion

		#region Custom Events


		//wait for Engine created
		public void Handler_SimEngineStarted ( object sender, EventArgs e )
		{
			Simulation3DEngine engine = (Simulation3DEngine)sender;
			//subscribe to Engine objectSelectionChanged event 
			engine.SelectionChangedEvent += Handler_SimSelectionChanged;
		}

		//
		public void Handler_SimSelectionChanged ( object sender, EventArgs e )
		{
			List<Selectable3DObject> WatchList = (List<Selectable3DObject>) sender;

			Instance.RebuildWatchLists ( WatchList );
			ds = Instance.WatchListCellsToDataSet ();

			//this.dataGridView1.AutoGenerateColumns = true;
			//this.dataGridView1.DataSource = ds;
			//dataGridView1.DataMember = ds.Tables[0].TableName;
			SetWatchGridDataSource ( ds );
			//dataGridView1.Refresh ();
			
		}



		/// <summary>
		/// Rebuilds specific element Watch lists (Cell, Distal, Proximal) 
		/// from generic list of Selectable3DObjects from Engine.
		/// </summary>
		/// <param name="watchList"></param>
		public void RebuildWatchLists ( List<Selectable3DObject> watchList ) 
		{
			bool add = true;
			Cell cell = null;
			DistalSynapse distal = null;
			ProximalSynapse proximal = null;

			WatchListCells.Clear ();
			WatchListDistal.Clear ();
			WatchListProximal.Clear ();

			foreach (var obj in watchList)
			{
				if (obj != null)
				{
					switch (obj.SelectablelType)
					{
						case SelectableObjectType.Cell:
							cell = (Cell)obj;
							if (add == true)
								WatchListCells.Add ( cell );
							else
								WatchListCells.Remove ( cell );
							break;
						case SelectableObjectType.DistalSynapse:
							distal = (DistalSynapse)obj;
							if (add == true)
								WatchListDistal.Add ( distal );
							else
								WatchListDistal.Remove ( distal );
							break;
						case SelectableObjectType.ProximalSynapse:
							proximal = (ProximalSynapse)obj;
							if (add == true)
								WatchListProximal.Add ( proximal );
							else
								WatchListProximal.Remove ( proximal );
							break;
						case SelectableObjectType.None:
						default:
							break;

					}
				}
			}
		}
		#endregion

		/// <summary>
		/// Create DataSet representing objects in the list.
		/// Used as WatchGrid source.
		/// </summary>
		/// <returns></returns>
		public DataSet WatchListCellsToDataSet()
		{
			ds = new DataSet ();

			foreach (Cell cell in WatchListCells)
			{
				cell.AddWatchTable ( ref ds );
			}

			return ds;
		}


		//Thread safe way of setting the datasource
		//in WatchGrid
		delegate void SetDatasource ( DataSet ds ); 

		private void SetWatchGridDataSource ( DataSet ds )
		{
			// InvokeRequired required compares the thread ID of the 
			// calling thread to the thread ID of the creating thread. 
			// If these threads are different, it returns true. 
			if (this.dataGridView1.InvokeRequired)
			{
				SetDatasource d = new SetDatasource ( SetWatchGridDataSource );
				this.Invoke ( d, new object[] { ds } );
			}
			else
			{
				this.dataGridView1.AutoGenerateColumns = true;
				this.dataGridView1.DataSource = ds;
				this.dataGridView1.DataMember = ds.Tables[0].TableName;
				this.dataGridView1.Refresh ();
			}
		}



	}
}
