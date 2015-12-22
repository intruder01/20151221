namespace OpenHTM.IDE
{
    partial class Simulation3DForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Simulation3DForm));
			this.pictureBoxSurface = new System.Windows.Forms.PictureBox();
			this.toolBar = new System.Windows.Forms.ToolStrip();
			this.menuShow = new System.Windows.Forms.ToolStripDropDownButton();
			this.menuShowSpatialLearning = new System.Windows.Forms.ToolStripMenuItem();
			this.menuShowTemporalLearning = new System.Windows.Forms.ToolStripMenuItem();
			this.menuShowCoordinateSystem = new System.Windows.Forms.ToolStripMenuItem();
			this.menuActiveColumnGrid = new System.Windows.Forms.ToolStripMenuItem();
			this.menuShowPredictedGrid = new System.Windows.Forms.ToolStripMenuItem();
			this.showCorrectButton = new System.Windows.Forms.ToolStripButton();
			this.showSeqPredictingButton = new System.Windows.Forms.ToolStripButton();
			this.showPredictingButton = new System.Windows.Forms.ToolStripButton();
			this.showLearningButton = new System.Windows.Forms.ToolStripButton();
			this.showActiveButton = new System.Windows.Forms.ToolStripButton();
			this.showFalsePredictedButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btnResetCamera = new System.Windows.Forms.ToolStripButton();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurface)).BeginInit();
			this.toolBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBoxSurface
			// 
			this.pictureBoxSurface.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxSurface.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxSurface.Name = "pictureBoxSurface";
			this.pictureBoxSurface.Size = new System.Drawing.Size(884, 691);
			this.pictureBoxSurface.TabIndex = 1;
			this.pictureBoxSurface.TabStop = false;
			this.pictureBoxSurface.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSurface_MouseClick);
			this.pictureBoxSurface.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSurface_MouseDoubleClick);
			this.pictureBoxSurface.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSurface_MouseMove);
			this.pictureBoxSurface.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSurface_MouseWheel);
			this.pictureBoxSurface.Resize += new System.EventHandler(this.pictureBoxSurface_Resize);
			// 
			// toolBar
			// 
			this.toolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuShow,
            this.showCorrectButton,
            this.showSeqPredictingButton,
            this.showPredictingButton,
            this.showLearningButton,
            this.showActiveButton,
            this.showFalsePredictedButton,
            this.toolStripSeparator1,
            this.btnResetCamera});
			this.toolBar.Location = new System.Drawing.Point(0, 0);
			this.toolBar.Name = "toolBar";
			this.toolBar.Size = new System.Drawing.Size(884, 25);
			this.toolBar.TabIndex = 0;
			this.toolBar.Text = "toolStrip1";
			// 
			// menuShow
			// 
			this.menuShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.menuShow.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuShowSpatialLearning,
            this.menuShowTemporalLearning,
            this.menuShowCoordinateSystem,
            this.menuActiveColumnGrid,
            this.menuShowPredictedGrid});
			this.menuShow.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.menuShow.Name = "menuShow";
			this.menuShow.Size = new System.Drawing.Size(49, 22);
			this.menuShow.Text = "Show";
			// 
			// menuShowSpatialLearning
			// 
			this.menuShowSpatialLearning.CheckOnClick = true;
			this.menuShowSpatialLearning.Name = "menuShowSpatialLearning";
			this.menuShowSpatialLearning.Size = new System.Drawing.Size(198, 22);
			this.menuShowSpatialLearning.Text = "Spatial Learning";
			this.menuShowSpatialLearning.Click += new System.EventHandler(this.menuShowSpatialLearning_Click);
			// 
			// menuShowTemporalLearning
			// 
			this.menuShowTemporalLearning.CheckOnClick = true;
			this.menuShowTemporalLearning.Name = "menuShowTemporalLearning";
			this.menuShowTemporalLearning.Size = new System.Drawing.Size(198, 22);
			this.menuShowTemporalLearning.Text = "Temporal Learning";
			this.menuShowTemporalLearning.Click += new System.EventHandler(this.menuShowTemporalLearning_Click);
			// 
			// menuShowCoordinateSystem
			// 
			this.menuShowCoordinateSystem.CheckOnClick = true;
			this.menuShowCoordinateSystem.Name = "menuShowCoordinateSystem";
			this.menuShowCoordinateSystem.Size = new System.Drawing.Size(198, 22);
			this.menuShowCoordinateSystem.Text = "Coordinate System";
			this.menuShowCoordinateSystem.Click += new System.EventHandler(this.menuShowCoordinateSystem_Click);
			// 
			// menuActiveColumnGrid
			// 
			this.menuActiveColumnGrid.CheckOnClick = true;
			this.menuActiveColumnGrid.Name = "menuActiveColumnGrid";
			this.menuActiveColumnGrid.Size = new System.Drawing.Size(198, 22);
			this.menuActiveColumnGrid.Text = "Active Column Grid";
			this.menuActiveColumnGrid.Click += new System.EventHandler(this.menuActiveColumnGrid_Click);
			// 
			// menuShowPredictedGrid
			// 
			this.menuShowPredictedGrid.CheckOnClick = true;
			this.menuShowPredictedGrid.Name = "menuShowPredictedGrid";
			this.menuShowPredictedGrid.Size = new System.Drawing.Size(198, 22);
			this.menuShowPredictedGrid.Text = "Region Predictions Grid";
			this.menuShowPredictedGrid.Click += new System.EventHandler(this.menuShowPredictedGrid_Click);
			// 
			// showCorrectButton
			// 
			this.showCorrectButton.BackColor = System.Drawing.Color.Green;
			this.showCorrectButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.showCorrectButton.Image = ((System.Drawing.Image)(resources.GetObject("showCorrectButton.Image")));
			this.showCorrectButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showCorrectButton.Name = "showCorrectButton";
			this.showCorrectButton.Size = new System.Drawing.Size(23, 22);
			this.showCorrectButton.Click += new System.EventHandler(this.showCorrectButton_Click);
			// 
			// showSeqPredictingButton
			// 
			this.showSeqPredictingButton.BackColor = System.Drawing.Color.Magenta;
			this.showSeqPredictingButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.showSeqPredictingButton.Image = ((System.Drawing.Image)(resources.GetObject("showSeqPredictingButton.Image")));
			this.showSeqPredictingButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showSeqPredictingButton.Name = "showSeqPredictingButton";
			this.showSeqPredictingButton.Size = new System.Drawing.Size(23, 22);
			this.showSeqPredictingButton.Click += new System.EventHandler(this.showSeqPredictingButton_Click);
			// 
			// showPredictingButton
			// 
			this.showPredictingButton.BackColor = System.Drawing.Color.Chocolate;
			this.showPredictingButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.showPredictingButton.Image = ((System.Drawing.Image)(resources.GetObject("showPredictingButton.Image")));
			this.showPredictingButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showPredictingButton.Name = "showPredictingButton";
			this.showPredictingButton.Size = new System.Drawing.Size(23, 22);
			this.showPredictingButton.Click += new System.EventHandler(this.showPredictingButton_Click);
			// 
			// showLearningButton
			// 
			this.showLearningButton.BackColor = System.Drawing.Color.Gray;
			this.showLearningButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.showLearningButton.Image = ((System.Drawing.Image)(resources.GetObject("showLearningButton.Image")));
			this.showLearningButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showLearningButton.Name = "showLearningButton";
			this.showLearningButton.Size = new System.Drawing.Size(23, 22);
			this.showLearningButton.Click += new System.EventHandler(this.showLearningButton_Click);
			// 
			// showActiveButton
			// 
			this.showActiveButton.BackColor = System.Drawing.Color.Black;
			this.showActiveButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.showActiveButton.ForeColor = System.Drawing.Color.White;
			this.showActiveButton.Image = ((System.Drawing.Image)(resources.GetObject("showActiveButton.Image")));
			this.showActiveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showActiveButton.Name = "showActiveButton";
			this.showActiveButton.Size = new System.Drawing.Size(23, 22);
			this.showActiveButton.Click += new System.EventHandler(this.showActiveButton_Click);
			// 
			// showFalsePredictedButton
			// 
			this.showFalsePredictedButton.BackColor = System.Drawing.Color.Red;
			this.showFalsePredictedButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.showFalsePredictedButton.Image = ((System.Drawing.Image)(resources.GetObject("showFalsePredictedButton.Image")));
			this.showFalsePredictedButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.showFalsePredictedButton.Name = "showFalsePredictedButton";
			this.showFalsePredictedButton.Size = new System.Drawing.Size(23, 22);
			this.showFalsePredictedButton.Click += new System.EventHandler(this.showFalsePredictedButton_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btnResetCamera
			// 
			this.btnResetCamera.BackColor = System.Drawing.SystemColors.ControlLight;
			this.btnResetCamera.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.btnResetCamera.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnResetCamera.Name = "btnResetCamera";
			this.btnResetCamera.Size = new System.Drawing.Size(67, 22);
			this.btnResetCamera.Text = "Reset Cam";
			this.btnResetCamera.Click += new System.EventHandler(this.btnResetCamera_Click);
			// 
			// Simulation3DForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(884, 691);
			this.Controls.Add(this.toolBar);
			this.Controls.Add(this.pictureBoxSurface);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.MinimumSize = new System.Drawing.Size(400, 400);
			this.Name = "Simulation3DForm";
			this.Text = "3D Neural Network Simulation";
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxSurface)).EndInit();
			this.toolBar.ResumeLayout(false);
			this.toolBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		#endregion

        public System.Windows.Forms.PictureBox pictureBoxSurface;
        private System.Windows.Forms.ToolStrip toolBar;
        private System.Windows.Forms.ToolStripDropDownButton menuShow;
        private System.Windows.Forms.ToolStripMenuItem menuShowTemporalLearning;
        private System.Windows.Forms.ToolStripMenuItem menuShowSpatialLearning;
        private System.Windows.Forms.ToolStripMenuItem menuShowCoordinateSystem;
		private System.Windows.Forms.ToolStripMenuItem menuActiveColumnGrid;
		private System.Windows.Forms.ToolStripMenuItem menuShowPredictedGrid;
		private System.Windows.Forms.ToolStripButton showActiveButton;
		private System.Windows.Forms.ToolStripButton showFalsePredictedButton;
		private System.Windows.Forms.ToolStripButton showLearningButton;
		private System.Windows.Forms.ToolStripButton showPredictingButton;
		private System.Windows.Forms.ToolStripButton showSeqPredictingButton;
		private System.Windows.Forms.ToolStripButton showCorrectButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btnResetCamera;
    }
}

