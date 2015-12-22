namespace OpenHTM.IDE
{
	partial class StateInformationForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.menuMode = new System.Windows.Forms.ToolStripMenuItem();
			this.menuModeRegular = new System.Windows.Forms.ToolStripMenuItem();
			this.menuModeInput = new System.Windows.Forms.ToolStripMenuItem();
			this.menuModeInputReconstruction = new System.Windows.Forms.ToolStripMenuItem();
			this.menuModePredictionReconstruction = new System.Windows.Forms.ToolStripMenuItem();
			this.menuModeBoosting = new System.Windows.Forms.ToolStripMenuItem();
			this.menuModeColumnsSideview = new System.Windows.Forms.ToolStripMenuItem();
			this.menuView = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewActiveCells = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewPredictedCells = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewWasPredictedAndActiveCells = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewWasPredictedAndNonActiveCells = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewSegmentUpdates = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewRegionalIncreasedPermanenceSynapses = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewRegionalDecreasedPermanenceSynapses = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewRegionalNewSynapses = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewRegionalRemovedSynapses = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.menuViewSelectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewDeselectAll = new System.Windows.Forms.ToolStripMenuItem();
			this.menuDefaultView = new System.Windows.Forms.ToolStripMenuItem();
			this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuHelpLegend = new System.Windows.Forms.ToolStripMenuItem();
			this._viewer = new OpenHTM.IDE.StateInformationPanel();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuMode,
            this.menuView,
            this.menuHelp});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(637, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// menuMode
			// 
			this.menuMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuModeRegular,
            this.menuModeInput,
            this.menuModeInputReconstruction,
            this.menuModePredictionReconstruction,
            this.menuModeBoosting,
            this.menuModeColumnsSideview});
			this.menuMode.Name = "menuMode";
			this.menuMode.Size = new System.Drawing.Size(50, 20);
			this.menuMode.Text = "&Mode";
			// 
			// menuModeRegular
			// 
			this.menuModeRegular.Checked = true;
			this.menuModeRegular.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuModeRegular.Name = "menuModeRegular";
			this.menuModeRegular.Size = new System.Drawing.Size(251, 22);
			this.menuModeRegular.Text = "Regular mode";
			this.menuModeRegular.Click += new System.EventHandler(this.menuModeRegular_Click);
			// 
			// menuModeInput
			// 
			this.menuModeInput.Name = "menuModeInput";
			this.menuModeInput.Size = new System.Drawing.Size(251, 22);
			this.menuModeInput.Text = "Feedforward Input";
			this.menuModeInput.Click += new System.EventHandler(this.menuModeInput_Click);
			// 
			// menuModeInputReconstruction
			// 
			this.menuModeInputReconstruction.Name = "menuModeInputReconstruction";
			this.menuModeInputReconstruction.Size = new System.Drawing.Size(251, 22);
			this.menuModeInputReconstruction.Text = "Feedforward Input reconstruction";
			this.menuModeInputReconstruction.Click += new System.EventHandler(this.menuModeInputReconstruction_Click);
			// 
			// menuModePredictionReconstruction
			// 
			this.menuModePredictionReconstruction.Name = "menuModePredictionReconstruction";
			this.menuModePredictionReconstruction.Size = new System.Drawing.Size(251, 22);
			this.menuModePredictionReconstruction.Text = "Prediction reconstruction";
			this.menuModePredictionReconstruction.Click += new System.EventHandler(this.menuModePredictionReconstruction_Click);
			// 
			// menuModeBoosting
			// 
			this.menuModeBoosting.Name = "menuModeBoosting";
			this.menuModeBoosting.Size = new System.Drawing.Size(251, 22);
			this.menuModeBoosting.Text = "Boosting";
			this.menuModeBoosting.Click += new System.EventHandler(this.menuModeBoosting_Click);
			// 
			// menuModeColumnsSideview
			// 
			this.menuModeColumnsSideview.Name = "menuModeColumnsSideview";
			this.menuModeColumnsSideview.Size = new System.Drawing.Size(251, 22);
			this.menuModeColumnsSideview.Text = "Columns sideview";
			this.menuModeColumnsSideview.Click += new System.EventHandler(this.menuModeColumnsSideview_Click);
			// 
			// menuView
			// 
			this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewActiveCells,
            this.menuViewPredictedCells,
            this.menuViewWasPredictedAndActiveCells,
            this.menuViewWasPredictedAndNonActiveCells,
            this.menuViewSegmentUpdates,
            this.menuViewRegionalIncreasedPermanenceSynapses,
            this.menuViewRegionalDecreasedPermanenceSynapses,
            this.menuViewRegionalNewSynapses,
            this.menuViewRegionalRemovedSynapses,
            this.toolStripSeparator1,
            this.menuViewSelectAll,
            this.menuViewDeselectAll,
            this.menuDefaultView});
			this.menuView.Name = "menuView";
			this.menuView.Size = new System.Drawing.Size(44, 20);
			this.menuView.Text = "&View";
			// 
			// menuViewActiveCells
			// 
			this.menuViewActiveCells.Checked = true;
			this.menuViewActiveCells.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuViewActiveCells.Name = "menuViewActiveCells";
			this.menuViewActiveCells.Size = new System.Drawing.Size(341, 22);
			this.menuViewActiveCells.Text = "Show active cells (1)";
			this.menuViewActiveCells.Click += new System.EventHandler(this.menuViewActiveCells_Click);
			// 
			// menuViewPredictedCells
			// 
			this.menuViewPredictedCells.Checked = true;
			this.menuViewPredictedCells.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuViewPredictedCells.Name = "menuViewPredictedCells";
			this.menuViewPredictedCells.Size = new System.Drawing.Size(341, 22);
			this.menuViewPredictedCells.Text = "Show predicting cells (2)";
			this.menuViewPredictedCells.Click += new System.EventHandler(this.menuViewPredictedCells_Click);
			// 
			// menuViewWasPredictedAndActiveCells
			// 
			this.menuViewWasPredictedAndActiveCells.Checked = true;
			this.menuViewWasPredictedAndActiveCells.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuViewWasPredictedAndActiveCells.Name = "menuViewWasPredictedAndActiveCells";
			this.menuViewWasPredictedAndActiveCells.Size = new System.Drawing.Size(341, 22);
			this.menuViewWasPredictedAndActiveCells.Text = "Show was predicted and active cells (3)";
			this.menuViewWasPredictedAndActiveCells.Click += new System.EventHandler(this.menuViewWasPredictedAndActiveCells_Click);
			// 
			// menuViewWasPredictedAndNonActiveCells
			// 
			this.menuViewWasPredictedAndNonActiveCells.Checked = true;
			this.menuViewWasPredictedAndNonActiveCells.CheckState = System.Windows.Forms.CheckState.Checked;
			this.menuViewWasPredictedAndNonActiveCells.Name = "menuViewWasPredictedAndNonActiveCells";
			this.menuViewWasPredictedAndNonActiveCells.Size = new System.Drawing.Size(341, 22);
			this.menuViewWasPredictedAndNonActiveCells.Text = "Show was predicted and non-active cells (4)";
			this.menuViewWasPredictedAndNonActiveCells.Click += new System.EventHandler(this.menuViewWasPredictedAndNonActiveCells_Click);
			// 
			// menuViewSegmentUpdates
			// 
			this.menuViewSegmentUpdates.Name = "menuViewSegmentUpdates";
			this.menuViewSegmentUpdates.Size = new System.Drawing.Size(341, 22);
			this.menuViewSegmentUpdates.Text = "Show segment updates for selected cell (5)";
			this.menuViewSegmentUpdates.Click += new System.EventHandler(this.menuViewSegmentUpdates_Click);
			// 
			// menuViewRegionalIncreasedPermanenceSynapses
			// 
			this.menuViewRegionalIncreasedPermanenceSynapses.Name = "menuViewRegionalIncreasedPermanenceSynapses";
			this.menuViewRegionalIncreasedPermanenceSynapses.Size = new System.Drawing.Size(341, 22);
			this.menuViewRegionalIncreasedPermanenceSynapses.Text = "Show regional increased permanence synapses (6)";
			this.menuViewRegionalIncreasedPermanenceSynapses.Click += new System.EventHandler(this.menuViewRegionalIncreasedPermanenceSynapses_Click);
			// 
			// menuViewRegionalDecreasedPermanenceSynapses
			// 
			this.menuViewRegionalDecreasedPermanenceSynapses.Name = "menuViewRegionalDecreasedPermanenceSynapses";
			this.menuViewRegionalDecreasedPermanenceSynapses.Size = new System.Drawing.Size(341, 22);
			this.menuViewRegionalDecreasedPermanenceSynapses.Text = "Show regional decreased permanence synapses (7)";
			this.menuViewRegionalDecreasedPermanenceSynapses.Click += new System.EventHandler(this.menuViewRegionalDecreasedPermanenceSynapses_Click);
			// 
			// menuViewRegionalNewSynapses
			// 
			this.menuViewRegionalNewSynapses.Name = "menuViewRegionalNewSynapses";
			this.menuViewRegionalNewSynapses.Size = new System.Drawing.Size(341, 22);
			this.menuViewRegionalNewSynapses.Text = "Show regional new synapses (8)";
			this.menuViewRegionalNewSynapses.Click += new System.EventHandler(this.menuViewRegionalNewSynapses_Click);
			// 
			// menuViewRegionalRemovedSynapses
			// 
			this.menuViewRegionalRemovedSynapses.Name = "menuViewRegionalRemovedSynapses";
			this.menuViewRegionalRemovedSynapses.Size = new System.Drawing.Size(341, 22);
			this.menuViewRegionalRemovedSynapses.Text = "Show regional removed synapses (9)";
			this.menuViewRegionalRemovedSynapses.Click += new System.EventHandler(this.menuViewRegionalRemovedSynapses_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(338, 6);
			// 
			// menuViewSelectAll
			// 
			this.menuViewSelectAll.Name = "menuViewSelectAll";
			this.menuViewSelectAll.Size = new System.Drawing.Size(341, 22);
			this.menuViewSelectAll.Text = "Select all";
			this.menuViewSelectAll.Click += new System.EventHandler(this.menuViewSelectAll_Click);
			// 
			// menuViewDeselectAll
			// 
			this.menuViewDeselectAll.Name = "menuViewDeselectAll";
			this.menuViewDeselectAll.Size = new System.Drawing.Size(341, 22);
			this.menuViewDeselectAll.Text = "Deselect all (Backspace)";
			this.menuViewDeselectAll.Click += new System.EventHandler(this.menuViewDeselectAll_Click);
			// 
			// menuDefaultView
			// 
			this.menuDefaultView.Name = "menuDefaultView";
			this.menuDefaultView.Size = new System.Drawing.Size(341, 22);
			this.menuDefaultView.Text = "Default view";
			this.menuDefaultView.Click += new System.EventHandler(this.menuDefaultView_Click);
			// 
			// menuHelp
			// 
			this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHelpLegend});
			this.menuHelp.Name = "menuHelp";
			this.menuHelp.Size = new System.Drawing.Size(44, 20);
			this.menuHelp.Text = "&Help";
			// 
			// menuHelpLegend
			// 
			this.menuHelpLegend.Name = "menuHelpLegend";
			this.menuHelpLegend.Size = new System.Drawing.Size(113, 22);
			this.menuHelpLegend.Text = "Legend";
			this.menuHelpLegend.Click += new System.EventHandler(this.menuHelpLegend_Click);
			// 
			// _viewer
			// 
			this._viewer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._viewer.Location = new System.Drawing.Point(0, 24);
			this._viewer.Name = "_viewer";
			this._viewer.Size = new System.Drawing.Size(637, 320);
			this._viewer.TabIndex = 0;
			// 
			// StateInformationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(637, 344);
			this.Controls.Add(this._viewer);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "StateInformationForm";
			this.Text = "State Information";
			this.SizeChanged += new System.EventHandler(this.Form_SizeChanged);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem menuMode;
		private System.Windows.Forms.ToolStripMenuItem menuView;
		private System.Windows.Forms.ToolStripMenuItem menuModeRegular;
		private System.Windows.Forms.ToolStripMenuItem menuModeInput;
		private System.Windows.Forms.ToolStripMenuItem menuModeInputReconstruction;
		private System.Windows.Forms.ToolStripMenuItem menuModePredictionReconstruction;
		private System.Windows.Forms.ToolStripMenuItem menuModeBoosting;
		private System.Windows.Forms.ToolStripMenuItem menuModeColumnsSideview;
		private System.Windows.Forms.ToolStripMenuItem menuHelp;
		private System.Windows.Forms.ToolStripMenuItem menuViewActiveCells;
		private System.Windows.Forms.ToolStripMenuItem menuViewPredictedCells;
		private System.Windows.Forms.ToolStripMenuItem menuViewWasPredictedAndActiveCells;
		private System.Windows.Forms.ToolStripMenuItem menuViewWasPredictedAndNonActiveCells;
		private System.Windows.Forms.ToolStripMenuItem menuViewSegmentUpdates;
		private System.Windows.Forms.ToolStripMenuItem menuViewRegionalIncreasedPermanenceSynapses;
		private System.Windows.Forms.ToolStripMenuItem menuViewRegionalDecreasedPermanenceSynapses;
		private System.Windows.Forms.ToolStripMenuItem menuViewRegionalNewSynapses;
		private System.Windows.Forms.ToolStripMenuItem menuViewRegionalRemovedSynapses;
		private System.Windows.Forms.ToolStripMenuItem menuViewDeselectAll;
		private System.Windows.Forms.ToolStripMenuItem menuDefaultView;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem menuViewSelectAll;
		private StateInformationPanel stateInformationPanel;
		private System.Windows.Forms.ToolStripMenuItem menuHelpLegend;
	}
}