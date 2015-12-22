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
	public partial class WatchForm : DockContent
	{
		#region Fields

		// Private singleton instance
		private static WatchForm _instance;

		private List<Cell> WatchListCells;
		private List<DistalSynapse> WatchListDistal;
		private List<ProximalSynapse> WatchListProximal;
		//private DataSet ds;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static WatchForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new WatchForm ();
				}
				return _instance;
			}
		}


		#endregion

		#region Nested classes



		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WatchForm"/> class.
		/// </summary>
		public WatchForm ()
		{
			this.InitializeComponent ();

			// Set UI properties
			this.MdiParent = MainForm.Instance;

			WatchListCells = new List<Cell> ();
			WatchListDistal = new List<DistalSynapse> ();
			WatchListProximal = new List<ProximalSynapse> ();
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
		/// <summary>
		/// Event handler. Handles Watch selection change notifications.
		/// </summary>
		/// <param name="sender">List of Selectable3DObject - WatchList from Simulation3DEngine
		/// TEST: entire network passed by NetControllerForm.Instance.TopNode.Region</param>
		/// <param name="e"></param>
		public void Handler_SimSelectionChanged ( object sender, EventArgs e )
		{
			//List<Selectable3DObject> WatchList = (List<Selectable3DObject>) sender;
			//Instance.RebuildWatchLists ( WatchList );
			//ds = Instance.WatchListCellsToDataSet ();
			SetPropertyGridDataSource ( (Region)sender );
		}


		public void Handler_StateInfoPanelSelectionChanged ( object sender, EventArgs e )
		{
			//List<Selectable3DObject> WatchList = (List<Selectable3DObject>) sender;
			//Instance.RebuildWatchLists ( WatchList );
			//ds = Instance.WatchListCellsToDataSet ();
			SetPropertyGridDataSource ( (Region)sender );

		}
		/// <summary>
		/// Rebuilds current Watch structure for watchPropertyGrid
		/// from generic list of Selectable3DObjects from 3DEngine.
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



		//Thread safe way of setting the datasource
		//in WatchGrid
		delegate void SetPropertyGridDatasource ( Region region ); 

		private void SetPropertyGridDataSource ( Region region )
		{
			// InvokeRequired required compares the thread ID of the 
			// calling thread to the thread ID of the creating thread. 
			// If these threads are different, it returns true. 
			if (this.watchPropertyGrid.InvokeRequired)
			{
				SetPropertyGridDatasource d = new SetPropertyGridDatasource ( SetPropertyGridDataSource );
				this.Invoke ( d, new object[] { region } );
			}
			else
			{
				this.watchPropertyGrid.SelectObject ( region, false, 500 );
				this.watchPropertyGrid.Refresh ();
			}
		}



	}
}
