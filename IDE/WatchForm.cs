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
	// when selection changes, broadcast new selection to 
	// StateInformationPanel and Simulation3D so that current selection is
	// always synchronised
	public delegate void WatchFormSelectionSynch_Event (object sender, EventArgs e);

	public partial class WatchForm : DockContent
	{
		public static event WatchFormSelectionSynch_Event WatchFormSelectionSynch = delegate { };

		#region Fields

		// Private singleton instance
		private static WatchForm _instance;


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
			//subscribe to Engine SelectionChanged event 
			engine.SelectionChangedEvent += Handler_SimSelectionChanged;
		}
		//wait for Engine shutdown
		public void Handler_SimEngineShutdown ( object sender, EventArgs e )
		{
			Simulation3DEngine engine = (Simulation3DEngine)sender;
			//un-subscribe from Engine SelectionChanged event 
			engine.SelectionChangedEvent -= Handler_SimSelectionChanged;
		}

		/// <summary>
		/// Event handler. Handles Watch selection change notifications.
		/// </summary>
		/// <param name="sender">List of Selectable3DObject - WatchList from Simulation3DEngine
		/// TEST: entire network passed by NetControllerForm.Instance.TopNode.Region</param>
		/// <param name="e"></param>
		public void Handler_SimSelectionChanged ( object sender, EventArgs e, object region )
		{
			SetPropertyGridDataSource ( region );
		}


		public void Handler_StateInfoPanelSelectionChanged ( object sender, EventArgs e, object selectedEntities )
		{
			SetPropertyGridDataSource ( selectedEntities );
		}
	
	
		#endregion



		//Thread safe way of setting the datasource
		//in WatchGrid
		delegate void SetPropertyGridDatasource ( object region ); 

		private void SetPropertyGridDataSource ( object region )
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
