namespace OpenHTM.IDE
{
	using OpenHTM.IDE.UIControls;

	partial class StatisticsForm
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
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.tabControlMain = new System.Windows.Forms.TabControl();
			this.tabPageRegions = new System.Windows.Forms.TabPage();
			this.textBoxRegionCorrectPredictionCounter = new System.Windows.Forms.TextBox();
			this.textBoxRegionPredictionCounter = new System.Windows.Forms.TextBox();
			this.textBoxRegionPredictionPrecision = new System.Windows.Forms.TextBox();
			this.textBoxRegionActivityRate = new System.Windows.Forms.TextBox();
			this.textBoxRegionStepCounter = new System.Windows.Forms.TextBox();
			this.labelRegionStepCounter = new System.Windows.Forms.Label();
			this.labelRegionActivityRate = new System.Windows.Forms.Label();
			this.labelRegionPredictionPrecision = new System.Windows.Forms.Label();
			this.labelRegionPredictionCounter = new System.Windows.Forms.Label();
			this.labelRegionCorrectPredictionCounter = new System.Windows.Forms.Label();
			this.tabPageColumns = new System.Windows.Forms.TabPage();
			this.dataGridColumns = new System.Windows.Forms.DataGridView();
			this.dataColumnColumnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataColumnColumnInputPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataColumnColumnActivityRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataColumnColumnPredictionPrecision = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabPageCells = new System.Windows.Forms.TabPage();
			this.dataGridCells = new System.Windows.Forms.DataGridView();
			this.errorNumSteps = new System.Windows.Forms.ErrorProvider(this.components);
			this.dataColumnCellIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataColumnCellActivityRate = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dataColumnCellPredictPrecision = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tabControlMain.SuspendLayout();
			this.tabPageRegions.SuspendLayout();
			this.tabPageColumns.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridColumns)).BeginInit();
			this.tabPageCells.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridCells)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errorNumSteps)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControlMain
			// 
			this.tabControlMain.Controls.Add(this.tabPageRegions);
			this.tabControlMain.Controls.Add(this.tabPageColumns);
			this.tabControlMain.Controls.Add(this.tabPageCells);
			this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControlMain.Location = new System.Drawing.Point(0, 0);
			this.tabControlMain.Name = "tabControlMain";
			this.tabControlMain.SelectedIndex = 0;
			this.tabControlMain.Size = new System.Drawing.Size(1067, 171);
			this.tabControlMain.TabIndex = 2;
			// 
			// tabPageRegions
			// 
			this.tabPageRegions.Controls.Add(this.textBoxRegionCorrectPredictionCounter);
			this.tabPageRegions.Controls.Add(this.textBoxRegionPredictionCounter);
			this.tabPageRegions.Controls.Add(this.textBoxRegionPredictionPrecision);
			this.tabPageRegions.Controls.Add(this.textBoxRegionActivityRate);
			this.tabPageRegions.Controls.Add(this.textBoxRegionStepCounter);
			this.tabPageRegions.Controls.Add(this.labelRegionStepCounter);
			this.tabPageRegions.Controls.Add(this.labelRegionActivityRate);
			this.tabPageRegions.Controls.Add(this.labelRegionPredictionPrecision);
			this.tabPageRegions.Controls.Add(this.labelRegionPredictionCounter);
			this.tabPageRegions.Controls.Add(this.labelRegionCorrectPredictionCounter);
			this.tabPageRegions.Location = new System.Drawing.Point(4, 22);
			this.tabPageRegions.Name = "tabPageRegions";
			this.tabPageRegions.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageRegions.Size = new System.Drawing.Size(1059, 145);
			this.tabPageRegions.TabIndex = 0;
			this.tabPageRegions.Text = "Regions";
			this.tabPageRegions.UseVisualStyleBackColor = true;
			// 
			// textBoxRegionCorrectPredictionCounter
			// 
			this.textBoxRegionCorrectPredictionCounter.Enabled = false;
			this.textBoxRegionCorrectPredictionCounter.Location = new System.Drawing.Point(114, 102);
			this.textBoxRegionCorrectPredictionCounter.Name = "textBoxRegionCorrectPredictionCounter";
			this.textBoxRegionCorrectPredictionCounter.ReadOnly = true;
			this.textBoxRegionCorrectPredictionCounter.Size = new System.Drawing.Size(80, 20);
			this.textBoxRegionCorrectPredictionCounter.TabIndex = 11;
			this.textBoxRegionCorrectPredictionCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxRegionPredictionCounter
			// 
			this.textBoxRegionPredictionCounter.Enabled = false;
			this.textBoxRegionPredictionCounter.Location = new System.Drawing.Point(114, 79);
			this.textBoxRegionPredictionCounter.Name = "textBoxRegionPredictionCounter";
			this.textBoxRegionPredictionCounter.ReadOnly = true;
			this.textBoxRegionPredictionCounter.Size = new System.Drawing.Size(80, 20);
			this.textBoxRegionPredictionCounter.TabIndex = 10;
			this.textBoxRegionPredictionCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxRegionPredictionPrecision
			// 
			this.textBoxRegionPredictionPrecision.Enabled = false;
			this.textBoxRegionPredictionPrecision.Location = new System.Drawing.Point(114, 56);
			this.textBoxRegionPredictionPrecision.Name = "textBoxRegionPredictionPrecision";
			this.textBoxRegionPredictionPrecision.ReadOnly = true;
			this.textBoxRegionPredictionPrecision.Size = new System.Drawing.Size(80, 20);
			this.textBoxRegionPredictionPrecision.TabIndex = 9;
			this.textBoxRegionPredictionPrecision.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxRegionActivityRate
			// 
			this.textBoxRegionActivityRate.Enabled = false;
			this.textBoxRegionActivityRate.Location = new System.Drawing.Point(114, 33);
			this.textBoxRegionActivityRate.Name = "textBoxRegionActivityRate";
			this.textBoxRegionActivityRate.ReadOnly = true;
			this.textBoxRegionActivityRate.Size = new System.Drawing.Size(80, 20);
			this.textBoxRegionActivityRate.TabIndex = 8;
			this.textBoxRegionActivityRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// textBoxRegionStepCounter
			// 
			this.textBoxRegionStepCounter.Enabled = false;
			this.textBoxRegionStepCounter.Location = new System.Drawing.Point(114, 10);
			this.textBoxRegionStepCounter.Name = "textBoxRegionStepCounter";
			this.textBoxRegionStepCounter.ReadOnly = true;
			this.textBoxRegionStepCounter.Size = new System.Drawing.Size(80, 20);
			this.textBoxRegionStepCounter.TabIndex = 7;
			this.textBoxRegionStepCounter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// labelRegionStepCounter
			// 
			this.labelRegionStepCounter.Location = new System.Drawing.Point(8, 13);
			this.labelRegionStepCounter.Name = "labelRegionStepCounter";
			this.labelRegionStepCounter.Size = new System.Drawing.Size(100, 20);
			this.labelRegionStepCounter.TabIndex = 0;
			this.labelRegionStepCounter.Text = "Steps";
			this.labelRegionStepCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelRegionActivityRate
			// 
			this.labelRegionActivityRate.Location = new System.Drawing.Point(8, 36);
			this.labelRegionActivityRate.Name = "labelRegionActivityRate";
			this.labelRegionActivityRate.Size = new System.Drawing.Size(100, 20);
			this.labelRegionActivityRate.TabIndex = 1;
			this.labelRegionActivityRate.Text = "Activity Rate";
			this.labelRegionActivityRate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelRegionPredictionPrecision
			// 
			this.labelRegionPredictionPrecision.Location = new System.Drawing.Point(8, 59);
			this.labelRegionPredictionPrecision.Name = "labelRegionPredictionPrecision";
			this.labelRegionPredictionPrecision.Size = new System.Drawing.Size(100, 20);
			this.labelRegionPredictionPrecision.TabIndex = 2;
			this.labelRegionPredictionPrecision.Text = "Prediction Precision";
			this.labelRegionPredictionPrecision.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelRegionPredictionCounter
			// 
			this.labelRegionPredictionCounter.Location = new System.Drawing.Point(8, 82);
			this.labelRegionPredictionCounter.Name = "labelRegionPredictionCounter";
			this.labelRegionPredictionCounter.Size = new System.Drawing.Size(100, 20);
			this.labelRegionPredictionCounter.TabIndex = 3;
			this.labelRegionPredictionCounter.Text = "Predictions";
			this.labelRegionPredictionCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// labelRegionCorrectPredictionCounter
			// 
			this.labelRegionCorrectPredictionCounter.Location = new System.Drawing.Point(8, 105);
			this.labelRegionCorrectPredictionCounter.Name = "labelRegionCorrectPredictionCounter";
			this.labelRegionCorrectPredictionCounter.Size = new System.Drawing.Size(100, 20);
			this.labelRegionCorrectPredictionCounter.TabIndex = 4;
			this.labelRegionCorrectPredictionCounter.Text = "Correct Predictions";
			this.labelRegionCorrectPredictionCounter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// tabPageColumns
			// 
			this.tabPageColumns.Controls.Add(this.dataGridColumns);
			this.tabPageColumns.Location = new System.Drawing.Point(4, 22);
			this.tabPageColumns.Name = "tabPageColumns";
			this.tabPageColumns.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageColumns.Size = new System.Drawing.Size(1059, 145);
			this.tabPageColumns.TabIndex = 1;
			this.tabPageColumns.Text = "Columns";
			this.tabPageColumns.UseVisualStyleBackColor = true;
			// 
			// dataGridColumns
			// 
			this.dataGridColumns.AllowUserToAddRows = false;
			this.dataGridColumns.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridColumns.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataColumnColumnPosition,
            this.dataColumnColumnInputPosition,
            this.dataColumnColumnActivityRate,
            this.dataColumnColumnPredictionPrecision});
			this.dataGridColumns.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridColumns.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridColumns.Location = new System.Drawing.Point(3, 3);
			this.dataGridColumns.MultiSelect = false;
			this.dataGridColumns.Name = "dataGridColumns";
			this.dataGridColumns.RowTemplate.Height = 18;
			this.dataGridColumns.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridColumns.Size = new System.Drawing.Size(1053, 139);
			this.dataGridColumns.TabIndex = 4;
			this.dataGridColumns.SelectionChanged += new System.EventHandler(this.dataGridColumns_SelectionChanged);
			// 
			// dataColumnColumnPosition
			// 
			this.dataColumnColumnPosition.HeaderText = "Column Position";
			this.dataColumnColumnPosition.Name = "dataColumnColumnPosition";
			// 
			// dataColumnColumnInputPosition
			// 
			this.dataColumnColumnInputPosition.HeaderText = "Input Position";
			this.dataColumnColumnInputPosition.Name = "dataColumnColumnInputPosition";
			// 
			// dataColumnColumnActivityRate
			// 
			this.dataColumnColumnActivityRate.HeaderText = "Activity Rate";
			this.dataColumnColumnActivityRate.Name = "dataColumnColumnActivityRate";
			// 
			// dataColumnColumnPredictionPrecision
			// 
			this.dataColumnColumnPredictionPrecision.HeaderText = "Prediction Precision";
			this.dataColumnColumnPredictionPrecision.Name = "dataColumnColumnPredictionPrecision";
			// 
			// tabPageCells
			// 
			this.tabPageCells.Controls.Add(this.dataGridCells);
			this.tabPageCells.Location = new System.Drawing.Point(4, 22);
			this.tabPageCells.Name = "tabPageCells";
			this.tabPageCells.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageCells.Size = new System.Drawing.Size(1059, 145);
			this.tabPageCells.TabIndex = 2;
			this.tabPageCells.Text = "Cells";
			this.tabPageCells.UseVisualStyleBackColor = true;
			// 
			// dataGridCells
			// 
			this.dataGridCells.AllowUserToAddRows = false;
			this.dataGridCells.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridCells.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridCells.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataColumnCellIndex,
            this.dataColumnCellActivityRate,
            this.dataColumnCellPredictPrecision});
			this.dataGridCells.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridCells.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.dataGridCells.Location = new System.Drawing.Point(3, 3);
			this.dataGridCells.MultiSelect = false;
			this.dataGridCells.Name = "dataGridCells";
			this.dataGridCells.RowTemplate.Height = 18;
			this.dataGridCells.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridCells.Size = new System.Drawing.Size(1053, 139);
			this.dataGridCells.TabIndex = 6;
			this.dataGridCells.SelectionChanged += new System.EventHandler(this.dataGridCells_SelectionChanged);
			// 
			// errorNumSteps
			// 
			this.errorNumSteps.ContainerControl = this;
			// 
			// dataColumnCellIndex
			// 
			this.dataColumnCellIndex.HeaderText = "Index";
			this.dataColumnCellIndex.Name = "dataColumnCellIndex";
			// 
			// dataColumnCellActivityRate
			// 
			this.dataColumnCellActivityRate.HeaderText = "Activity Rate";
			this.dataColumnCellActivityRate.Name = "dataColumnCellActivityRate";
			// 
			// dataColumnCellPredictPrecision
			// 
			this.dataColumnCellPredictPrecision.HeaderText = "Prediction Precision";
			this.dataColumnCellPredictPrecision.Name = "dataColumnCellPredictPrecision";
			// 
			// StatisticsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1067, 171);
			this.Controls.Add(this.tabControlMain);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "StatisticsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Statistics";
			this.tabControlMain.ResumeLayout(false);
			this.tabPageRegions.ResumeLayout(false);
			this.tabPageRegions.PerformLayout();
			this.tabPageColumns.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridColumns)).EndInit();
			this.tabPageCells.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridCells)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errorNumSteps)).EndInit();
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabControlMain;
		private System.Windows.Forms.TabPage tabPageRegions;
		private System.Windows.Forms.Label labelRegionStepCounter;
		private System.Windows.Forms.Label labelRegionActivityRate;
		private System.Windows.Forms.Label labelRegionPredictionPrecision;
		private System.Windows.Forms.Label labelRegionPredictionCounter;
		private System.Windows.Forms.Label labelRegionCorrectPredictionCounter;
		private System.Windows.Forms.ErrorProvider errorNumSteps;
		private System.Windows.Forms.TabPage tabPageColumns;
		private System.Windows.Forms.TabPage tabPageCells;
		private System.Windows.Forms.DataGridView dataGridColumns;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnColumnPosition;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnColumnInputPosition;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnColumnActivityRate;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnColumnPredictionPrecision;
		private System.Windows.Forms.DataGridView dataGridCells;
		private System.Windows.Forms.TextBox textBoxRegionStepCounter;
		private System.Windows.Forms.TextBox textBoxRegionCorrectPredictionCounter;
		private System.Windows.Forms.TextBox textBoxRegionPredictionCounter;
		private System.Windows.Forms.TextBox textBoxRegionPredictionPrecision;
		private System.Windows.Forms.TextBox textBoxRegionActivityRate;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnCellIndex;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnCellActivityRate;
		private System.Windows.Forms.DataGridViewTextBoxColumn dataColumnCellPredictPrecision;
    }
}