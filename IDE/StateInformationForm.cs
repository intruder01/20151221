using System;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenHTM.IDE
{
	public partial class StateInformationForm : DockContent
	{
		#region Fields

		// Private singleton instance
		private static StateInformationForm _instance;

		private StateInformationPanel _viewer;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static StateInformationForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new StateInformationForm();
				}
				return _instance;
			}
		}

		#endregion

		#region Constructor

		public StateInformationForm()
		{
			this.InitializeComponent();

			// Set UI properties
			this.MdiParent = MainForm.Instance;

			this.SetViewerSize();
			this.ActiveControl = this._viewer;
			this._viewer.Focus();
			this.MouseWheel += this._viewer.Panel_MouseWheel;
		}

		#endregion

		#region Methods

		public void SetViewerSize()
		{
			this._viewer.SetBounds(0, this.menuStrip1.Height, this.Width, this.Height - this.menuStrip1.Height);
		}

		public void RefreshControls()
		{
			this._viewer.RefreshControls();
		}

		private void UncheckAllViewModes()
		{
			foreach (ToolStripMenuItem strip in this.menuMode.DropDownItems)
			{
				strip.Checked = false;
			}
		}

		// A function that the "Viewer" uses in order to change things at this form or to
		// Refresh it for some purpose.
		public void RefreshMenus()
		{
			this.menuViewActiveCells.Checked = this._viewer.ViewActiveCells;
			this.menuViewPredictedCells.Checked = this._viewer.ViewPredictingCells;
			this.menuViewWasPredictedAndActiveCells.Checked = this._viewer.ViewActiveAndPredictedCells;
			this.menuViewWasPredictedAndNonActiveCells.Checked = this._viewer.ViewNonActiveAndPredictedCells;
			this.menuViewSegmentUpdates.Checked = this._viewer.ViewCellSegmentUpdates;
			this.menuViewRegionalIncreasedPermanenceSynapses.Checked = this._viewer.ViewRegionalIncreasedPermanenceSynapses;
			this.menuViewRegionalDecreasedPermanenceSynapses.Checked = this._viewer.ViewRegionalDecreasedPermanenceSynapses;
			this.menuViewRegionalNewSynapses.Checked = this._viewer.ViewRegionalNewSynapses;
			this.menuViewRegionalRemovedSynapses.Checked = this._viewer.ViewRegionalRemovedSynapses;
		}

		#endregion

		#region Events

		#region Form

		private void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Hide();
			this.Parent = null;
			e.Cancel = true;
		}

		private void Form_SizeChanged(object sender, EventArgs e)
		{
			this.SetViewerSize();
			this._viewer.Display();
		}

		#endregion

		#region Menus

		private void menuModeInput_Click(object sender, EventArgs e)
		{
			this.UncheckAllViewModes();
			this.menuModeInput.Checked = true;
			this._viewer._viewerMode = StateInformationPanel.Mode.FeedFowardInput;
			this._viewer.Display();
		}

		private void menuModeRegular_Click(object sender, EventArgs e)
		{
			this.UncheckAllViewModes();
			this.menuModeRegular.Checked = true;
			this._viewer._viewerMode = StateInformationPanel.Mode.Regular;
			this._viewer.Display();
		}

		private void menuModeInputReconstruction_Click(object sender, EventArgs e)
		{
			this.UncheckAllViewModes();
			this.menuModeInputReconstruction.Checked = true;
			this._viewer._viewerMode = StateInformationPanel.Mode.FeedFowardInputReconstruction;
			this._viewer.Display();
		}

		private void menuModePredictionReconstruction_Click(object sender, EventArgs e)
		{
			this.UncheckAllViewModes();
			this.menuModePredictionReconstruction.Checked = true;
			this._viewer._viewerMode = StateInformationPanel.Mode.PredictionReconstruction;
			this._viewer.Display();
		}

		private void menuModeBoosting_Click(object sender, EventArgs e)
		{
			this.UncheckAllViewModes();
			this.menuModeBoosting.Checked = true;
			this._viewer._viewerMode = StateInformationPanel.Mode.Boosting;
			this._viewer.Display();
		}

		private void menuModeColumnsSideview_Click(object sender, EventArgs e)
		{
			this.UncheckAllViewModes();
			this.menuModeColumnsSideview.Checked = true;
			this._viewer._viewerMode = StateInformationPanel.Mode.ColumnsSideView;
			this._viewer.Display();
		}

		private void menuViewActiveCells_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_ActiveCells();
		}

		private void menuViewPredictedCells_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_PredictingCells();
		}

		private void menuViewWasPredictedAndActiveCells_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_ActiveAndPredictedCells();
		}

		private void menuViewWasPredictedAndNonActiveCells_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_NonActiveAndPredictedCells();
		}

		private void menuViewSegmentUpdates_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_CellsSegmentUpdates();
		}

		private void menuViewRegionalIncreasedPermanenceSynapses_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_RegionalIncreasedPermanenceSynapses();
		}

		private void menuViewRegionalDecreasedPermanenceSynapses_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_RegionalDecreasedPermanenceSynapses();
		}

		private void menuViewRegionalNewSynapses_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_RegionalNewSynapses();
		}

		private void menuViewRegionalRemovedSynapses_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_RegionalRemovedSynapses();
		}

		private void menuViewSelectAll_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_SelectedAll();
		}

		private void menuViewDeselectAll_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_DeselectedAll();
		}

		private void menuDefaultView_Click(object sender, EventArgs e)
		{
			this._viewer.ViewSelect_DefaultView();
		}

		#endregion

		private void menuHelpLegend_Click(object sender, EventArgs e)
		{
			MessageBox.Show(
				"Controls:\n\n" + 
				"Drag with the mouse to view. Zoom in/out with the mouse wheel.\n" +
				"Press 'Ctrl + left mouse click' on a column or a cell to select them. Right click to unselect. " +
				"By selecting, you can view the segments and connections.\n" +
				"When viewing connections from far, each connection is marked by a different color for each " +
				"cell in the column. When viewing connection from close, each connection is " +
				"marked by a different color for each segment of the cell.\n\n" +
				"You can see an alternative colors set by holding 'Alt' key to see the connections permanences. \n" +
				"Connections with BLUE color means that the connection has permanence below connected threshold \n" +
				"Connections with RED color means that the connection has permanence above the connected threshold. \n" +
				"The connections colors intensity is dependent on what is the max permanence in the region and" +
				"the permanence of the connection.\n\n" +
				"L = Learning cell,\n" +
				"LP = Learning on previous step cell.\n\n" +
				"Color Legend\n\n" +
				"Regular mode:\n" +
				"Lime Green = predicting cells for the next timestep.\n" +
				"Yellow = active cells.\n" +
				"Aqua = cells that were predicted successfully\n" +
				"Red = cells that were predicted mistakely\n" +
				"Some of the colors are blended or hidden by other colors with more priority assigned\n\n" +
				"Feedforward input:\n" +
				"Green = assigned proximal dendrites\n\n" +
				"Feedforward input reconstruction:\n" +
				"Black = maximal strength feedforward input\n" +
				"Grey = medium strength feedforward input\n" +
				"White = minimal strength feedforward input\n\n" + 
				"Prediction reconstruction (1 step):\n" +
				"Black = maximal strength prediction\n" +
				"Grey = medium strength prediction\n" +
				"White = minimal strength prediction\n\n",
				"Help", MessageBoxButtons.OK);
		}

		#endregion
	}
}
