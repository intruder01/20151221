namespace OpenHTM.IDE
{
	using OpenHTM.IDE.UIControls;

	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
			WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin2 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient4 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient8 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient9 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient5 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient10 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient11 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient12 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient6 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient13 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient14 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
			this.menuMain = new System.Windows.Forms.MenuStrip();
			this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
			this.menuFileNew = new System.Windows.Forms.ToolStripMenuItem();
			this.menuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.menuFileSave = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.menuFileExit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuView = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewStateInformation = new System.Windows.Forms.ToolStripMenuItem();
			this.menuViewStatistics = new System.Windows.Forms.ToolStripMenuItem();
			this.menuView3DSimulation = new System.Windows.Forms.ToolStripMenuItem();
			this.menuEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.menuProject = new System.Windows.Forms.ToolStripMenuItem();
			this.menuProjectProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.menuTools = new System.Windows.Forms.ToolStripMenuItem();
			this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
			this.menuUserWiki = new System.Windows.Forms.ToolStripMenuItem();
			this.menuGoToWebsite = new System.Windows.Forms.ToolStripMenuItem();
			this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
			this.toolBar = new System.Windows.Forms.ToolStrip();
			this.buttonInitHTM = new System.Windows.Forms.ToolStripButton();
			this.buttonStepHTM = new System.Windows.Forms.ToolStripButton();
			this.buttonFastStepHTM = new System.Windows.Forms.ToolStripButton();
			this.buttonPauseHTM = new System.Windows.Forms.ToolStripButton();
			this.buttonStopHTM = new System.Windows.Forms.ToolStripButton();
			this.textBoxNumberSteps = new System.Windows.Forms.ToolStripTextBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.errorNumSteps = new System.Windows.Forms.ErrorProvider(this.components);
			this.dockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
			this.btnTest1 = new System.Windows.Forms.Button();
			this.btnTest2 = new System.Windows.Forms.Button();
			this.menuMain.SuspendLayout();
			this.toolBar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorNumSteps)).BeginInit();
			this.SuspendLayout();
			// 
			// menuMain
			// 
			this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuView,
            this.menuEdit,
            this.menuProject,
            this.menuTools,
            this.menuHelp});
			this.menuMain.Location = new System.Drawing.Point(0, 0);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(1067, 24);
			this.menuMain.TabIndex = 0;
			this.menuMain.Text = "menuStrip1";
			// 
			// menuFile
			// 
			this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFileNew,
            this.menuFileOpen,
            this.menuFileSave,
            this.toolStripSeparator2,
            this.menuFileExit});
			this.menuFile.Name = "menuFile";
			this.menuFile.Size = new System.Drawing.Size(37, 20);
			this.menuFile.Text = "&File";
			// 
			// menuFileNew
			// 
			this.menuFileNew.Name = "menuFileNew";
			this.menuFileNew.Size = new System.Drawing.Size(146, 22);
			this.menuFileNew.Text = "&New Project";
			this.menuFileNew.Click += new System.EventHandler(this.menuFileNew_Click);
			// 
			// menuFileOpen
			// 
			this.menuFileOpen.Name = "menuFileOpen";
			this.menuFileOpen.Size = new System.Drawing.Size(146, 22);
			this.menuFileOpen.Text = "&Open  Project";
			this.menuFileOpen.Click += new System.EventHandler(this.menuFileOpen_Click);
			// 
			// menuFileSave
			// 
			this.menuFileSave.Name = "menuFileSave";
			this.menuFileSave.Size = new System.Drawing.Size(146, 22);
			this.menuFileSave.Text = "&Save Project";
			this.menuFileSave.Click += new System.EventHandler(this.menuFileSave_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(143, 6);
			// 
			// menuFileExit
			// 
			this.menuFileExit.Name = "menuFileExit";
			this.menuFileExit.Size = new System.Drawing.Size(146, 22);
			this.menuFileExit.Text = "&Exit";
			this.menuFileExit.Click += new System.EventHandler(this.menuFileExit_Click);
			// 
			// menuView
			// 
			this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuViewStateInformation,
            this.menuViewStatistics,
            this.menuView3DSimulation});
			this.menuView.Name = "menuView";
			this.menuView.Size = new System.Drawing.Size(44, 20);
			this.menuView.Text = "&View";
			// 
			// menuViewStateInformation
			// 
			this.menuViewStateInformation.Name = "menuViewStateInformation";
			this.menuViewStateInformation.Size = new System.Drawing.Size(194, 22);
			this.menuViewStateInformation.Text = "View State Information";
			this.menuViewStateInformation.Click += new System.EventHandler(this.menuViewStateInformation_Click);
			// 
			// menuViewStatistics
			// 
			this.menuViewStatistics.Name = "menuViewStatistics";
			this.menuViewStatistics.Size = new System.Drawing.Size(194, 22);
			this.menuViewStatistics.Text = "View &Statistics";
			this.menuViewStatistics.Click += new System.EventHandler(this.menuViewStatistics_Click);
			// 
			// menuView3DSimulation
			// 
			this.menuView3DSimulation.Name = "menuView3DSimulation";
			this.menuView3DSimulation.Size = new System.Drawing.Size(194, 22);
			this.menuView3DSimulation.Text = "View &3D Simulation";
			this.menuView3DSimulation.Click += new System.EventHandler(this.menuView3DSimulation_Click);
			// 
			// menuEdit
			// 
			this.menuEdit.Name = "menuEdit";
			this.menuEdit.Size = new System.Drawing.Size(39, 20);
			this.menuEdit.Text = "&Edit";
			this.menuEdit.Visible = false;
			// 
			// menuProject
			// 
			this.menuProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProjectProperties});
			this.menuProject.Name = "menuProject";
			this.menuProject.Size = new System.Drawing.Size(56, 20);
			this.menuProject.Text = "&Project";
			// 
			// menuProjectProperties
			// 
			this.menuProjectProperties.Name = "menuProjectProperties";
			this.menuProjectProperties.Size = new System.Drawing.Size(136, 22);
			this.menuProjectProperties.Text = "Properties...";
			this.menuProjectProperties.Click += new System.EventHandler(this.menuProjectProperties_Click);
			// 
			// menuTools
			// 
			this.menuTools.Name = "menuTools";
			this.menuTools.Size = new System.Drawing.Size(48, 20);
			this.menuTools.Text = "&Tools";
			this.menuTools.Visible = false;
			// 
			// menuHelp
			// 
			this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuUserWiki,
            this.menuGoToWebsite,
            this.menuAbout});
			this.menuHelp.Name = "menuHelp";
			this.menuHelp.Size = new System.Drawing.Size(44, 20);
			this.menuHelp.Text = "&Help";
			// 
			// menuUserWiki
			// 
			this.menuUserWiki.Name = "menuUserWiki";
			this.menuUserWiki.Size = new System.Drawing.Size(166, 22);
			this.menuUserWiki.Text = "User Wiki";
			this.menuUserWiki.Click += new System.EventHandler(this.menuUserWiki_Click);
			// 
			// menuGoToWebsite
			// 
			this.menuGoToWebsite.Name = "menuGoToWebsite";
			this.menuGoToWebsite.Size = new System.Drawing.Size(166, 22);
			this.menuGoToWebsite.Text = "OpenHTM Home";
			this.menuGoToWebsite.Click += new System.EventHandler(this.menuGoToWebsite_Click);
			// 
			// menuAbout
			// 
			this.menuAbout.Name = "menuAbout";
			this.menuAbout.Size = new System.Drawing.Size(166, 22);
			this.menuAbout.Text = "About";
			this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
			// 
			// toolBar
			// 
			this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonInitHTM,
            this.buttonStepHTM,
            this.buttonFastStepHTM,
            this.buttonPauseHTM,
            this.buttonStopHTM,
            this.textBoxNumberSteps});
			this.toolBar.Location = new System.Drawing.Point(0, 24);
			this.toolBar.Name = "toolBar";
			this.toolBar.Size = new System.Drawing.Size(1067, 25);
			this.toolBar.TabIndex = 1;
			this.toolBar.Text = "toolStrip1";
			// 
			// buttonInitHTM
			// 
			this.buttonInitHTM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonInitHTM.Image = global::OpenHTM.IDE.Properties.Resources.buttonInitializeHTM;
			this.buttonInitHTM.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonInitHTM.Name = "buttonInitHTM";
			this.buttonInitHTM.Size = new System.Drawing.Size(23, 22);
			this.buttonInitHTM.Text = "toolStripButton1";
			this.buttonInitHTM.ToolTipText = "Initialize HTM-Network with single image transformation";
			this.buttonInitHTM.Click += new System.EventHandler(this.buttonInitHTM_Click);
			// 
			// buttonStepHTM
			// 
			this.buttonStepHTM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonStepHTM.Enabled = false;
			this.buttonStepHTM.Image = global::OpenHTM.IDE.Properties.Resources.buttonStepHTM;
			this.buttonStepHTM.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonStepHTM.Name = "buttonStepHTM";
			this.buttonStepHTM.Size = new System.Drawing.Size(23, 22);
			this.buttonStepHTM.Text = "toolStripButton1";
			this.buttonStepHTM.ToolTipText = "Step ";
			this.buttonStepHTM.Click += new System.EventHandler(this.buttonStepHTM_Click);
			// 
			// buttonFastStepHTM
			// 
			this.buttonFastStepHTM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonFastStepHTM.Enabled = false;
			this.buttonFastStepHTM.Image = global::OpenHTM.IDE.Properties.Resources.buttonStepFastHTM;
			this.buttonFastStepHTM.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonFastStepHTM.Name = "buttonFastStepHTM";
			this.buttonFastStepHTM.Size = new System.Drawing.Size(23, 22);
			this.buttonFastStepHTM.Text = "toolStripButton1";
			this.buttonFastStepHTM.ToolTipText = "Run All Steps";
			this.buttonFastStepHTM.Click += new System.EventHandler(this.buttonFastStepHTM_Click);
			// 
			// buttonPauseHTM
			// 
			this.buttonPauseHTM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonPauseHTM.Enabled = false;
			this.buttonPauseHTM.Image = global::OpenHTM.IDE.Properties.Resources.buttonPauseHTM;
			this.buttonPauseHTM.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonPauseHTM.Name = "buttonPauseHTM";
			this.buttonPauseHTM.Size = new System.Drawing.Size(23, 22);
			this.buttonPauseHTM.Text = "Pause";
			this.buttonPauseHTM.Click += new System.EventHandler(this.buttonPauseHTM_Click);
			// 
			// buttonStopHTM
			// 
			this.buttonStopHTM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.buttonStopHTM.Enabled = false;
			this.buttonStopHTM.Image = global::OpenHTM.IDE.Properties.Resources.buttonStopHTM;
			this.buttonStopHTM.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.buttonStopHTM.Name = "buttonStopHTM";
			this.buttonStopHTM.Size = new System.Drawing.Size(23, 22);
			this.buttonStopHTM.Text = "toolStripButton1";
			this.buttonStopHTM.ToolTipText = "Stop";
			this.buttonStopHTM.Click += new System.EventHandler(this.buttonStopHTM_Click);
			// 
			// textBoxNumberSteps
			// 
			this.textBoxNumberSteps.AutoSize = false;
			this.textBoxNumberSteps.Enabled = false;
			this.textBoxNumberSteps.Name = "textBoxNumberSteps";
			this.textBoxNumberSteps.Size = new System.Drawing.Size(50, 25);
			this.textBoxNumberSteps.Validated += new System.EventHandler(this.textBoxNumberSteps_Validated);
			// 
			// errorNumSteps
			// 
			this.errorNumSteps.ContainerControl = this;
			// 
			// dockPanel
			// 
			this.dockPanel.ActiveAutoHideContent = null;
			this.dockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockPanel.DockBackColor = System.Drawing.SystemColors.Control;
			this.dockPanel.Location = new System.Drawing.Point(0, 49);
			this.dockPanel.Name = "dockPanel";
			this.dockPanel.Size = new System.Drawing.Size(1067, 512);
			dockPanelGradient4.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient4.StartColor = System.Drawing.SystemColors.ControlLight;
			autoHideStripSkin2.DockStripGradient = dockPanelGradient4;
			tabGradient8.EndColor = System.Drawing.SystemColors.Control;
			tabGradient8.StartColor = System.Drawing.SystemColors.Control;
			tabGradient8.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			autoHideStripSkin2.TabGradient = tabGradient8;
			dockPanelSkin2.AutoHideStripSkin = autoHideStripSkin2;
			tabGradient9.EndColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient9.StartColor = System.Drawing.SystemColors.ControlLightLight;
			tabGradient9.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient2.ActiveTabGradient = tabGradient9;
			dockPanelGradient5.EndColor = System.Drawing.SystemColors.Control;
			dockPanelGradient5.StartColor = System.Drawing.SystemColors.Control;
			dockPaneStripGradient2.DockStripGradient = dockPanelGradient5;
			tabGradient10.EndColor = System.Drawing.SystemColors.ControlLight;
			tabGradient10.StartColor = System.Drawing.SystemColors.ControlLight;
			tabGradient10.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripGradient2.InactiveTabGradient = tabGradient10;
			dockPaneStripSkin2.DocumentGradient = dockPaneStripGradient2;
			tabGradient11.EndColor = System.Drawing.SystemColors.ActiveCaption;
			tabGradient11.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient11.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
			tabGradient11.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
			dockPaneStripToolWindowGradient2.ActiveCaptionGradient = tabGradient11;
			tabGradient12.EndColor = System.Drawing.SystemColors.Control;
			tabGradient12.StartColor = System.Drawing.SystemColors.Control;
			tabGradient12.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient2.ActiveTabGradient = tabGradient12;
			dockPanelGradient6.EndColor = System.Drawing.SystemColors.ControlLight;
			dockPanelGradient6.StartColor = System.Drawing.SystemColors.ControlLight;
			dockPaneStripToolWindowGradient2.DockStripGradient = dockPanelGradient6;
			tabGradient13.EndColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient13.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			tabGradient13.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
			tabGradient13.TextColor = System.Drawing.SystemColors.ControlText;
			dockPaneStripToolWindowGradient2.InactiveCaptionGradient = tabGradient13;
			tabGradient14.EndColor = System.Drawing.Color.Transparent;
			tabGradient14.StartColor = System.Drawing.Color.Transparent;
			tabGradient14.TextColor = System.Drawing.SystemColors.ControlDarkDark;
			dockPaneStripToolWindowGradient2.InactiveTabGradient = tabGradient14;
			dockPaneStripSkin2.ToolWindowGradient = dockPaneStripToolWindowGradient2;
			dockPanelSkin2.DockPaneStripSkin = dockPaneStripSkin2;
			this.dockPanel.Skin = dockPanelSkin2;
			this.dockPanel.TabIndex = 3;
			// 
			// btnTest1
			// 
			this.btnTest1.Location = new System.Drawing.Point(236, 28);
			this.btnTest1.Name = "btnTest1";
			this.btnTest1.Size = new System.Drawing.Size(46, 23);
			this.btnTest1.TabIndex = 6;
			this.btnTest1.Text = "Test1";
			this.btnTest1.UseVisualStyleBackColor = true;
			this.btnTest1.Click += new System.EventHandler(this.btnTest1_Click);
			// 
			// btnTest2
			// 
			this.btnTest2.Location = new System.Drawing.Point(299, 28);
			this.btnTest2.Name = "btnTest2";
			this.btnTest2.Size = new System.Drawing.Size(46, 23);
			this.btnTest2.TabIndex = 9;
			this.btnTest2.Text = "Test2";
			this.btnTest2.UseVisualStyleBackColor = true;
			this.btnTest2.Click += new System.EventHandler(this.btnTest2_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1067, 561);
			this.Controls.Add(this.btnTest2);
			this.Controls.Add(this.btnTest1);
			this.Controls.Add(this.dockPanel);
			this.Controls.Add(this.toolBar);
			this.Controls.Add(this.menuMain);
			this.IsMdiContainer = true;
			this.MainMenuStrip = this.menuMain;
			this.MinimumSize = new System.Drawing.Size(300, 300);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "OpenHTM";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_FormClosed);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Shown += new System.EventHandler(this.Form_Shown);
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.toolBar.ResumeLayout(false);
			this.toolBar.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorNumSteps)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
		private System.Windows.Forms.ToolStripMenuItem menuProjectProperties;
        private System.Windows.Forms.ToolStripMenuItem menuFileExit;
        private System.Windows.Forms.ToolStripMenuItem menuEdit;
        private System.Windows.Forms.ToolStripMenuItem menuView;
		private System.Windows.Forms.ToolStripMenuItem menuView3DSimulation;
		private System.Windows.Forms.ToolStripMenuItem menuViewStatistics;
		private System.Windows.Forms.ToolStripMenuItem menuViewStateInformation;
		private System.Windows.Forms.ToolStripMenuItem menuProject;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
		private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolTip toolTip1;
        public System.Windows.Forms.ToolStripButton buttonInitHTM;
        private System.Windows.Forms.ToolStripButton buttonStepHTM;
        private System.Windows.Forms.ToolStripButton buttonFastStepHTM;
        private System.Windows.Forms.ToolStripButton buttonStopHTM;
        private System.Windows.Forms.ToolStripMenuItem menuUserWiki;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.ToolStripMenuItem menuGoToWebsite;
        private System.Windows.Forms.ToolStripMenuItem menuFileNew;
        private System.Windows.Forms.ToolStripMenuItem menuFileOpen;
		private System.Windows.Forms.ToolStripMenuItem menuFileSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem menuTools;
		private System.Windows.Forms.ToolStripButton buttonPauseHTM;
		private System.Windows.Forms.ToolStripTextBox textBoxNumberSteps;
		private System.Windows.Forms.ErrorProvider errorNumSteps;
		private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel;
		private System.Windows.Forms.Button btnTest1;
		private System.Windows.Forms.Button btnTest2;
    }
}