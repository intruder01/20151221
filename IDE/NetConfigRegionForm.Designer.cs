namespace OpenHTM.IDE
{
    partial class NetConfigRegionForm
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
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.spinnerInputHeight = new System.Windows.Forms.NumericUpDown();
			this.spinnerInputWidth = new System.Windows.Forms.NumericUpDown();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.labelLocalActivity = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.spinnerMinOverlap = new System.Windows.Forms.NumericUpDown();
			this.labelMinOverlap = new System.Windows.Forms.Label();
			this.labelRegionHeight = new System.Windows.Forms.Label();
			this.spinnerInputPerColumn = new System.Windows.Forms.NumericUpDown();
			this.spinnerCellsPerColumn = new System.Windows.Forms.NumericUpDown();
			this.spinnerLocalityRadius = new System.Windows.Forms.NumericUpDown();
			this.labelRegionWidth = new System.Windows.Forms.Label();
			this.spinnerLocalActivity = new System.Windows.Forms.NumericUpDown();
			this.spinnerNewNumberSynapses = new System.Windows.Forms.NumericUpDown();
			this.labelInputPerColumn = new System.Windows.Forms.Label();
			this.spinnerRegionHeight = new System.Windows.Forms.NumericUpDown();
			this.labelLocalityRadius = new System.Windows.Forms.Label();
			this.spinnerRegionWidth = new System.Windows.Forms.NumericUpDown();
			this.spinnerSegmentThreshold = new System.Windows.Forms.NumericUpDown();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinnerInputHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerInputWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerMinOverlap)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerInputPerColumn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerCellsPerColumn)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerLocalityRadius)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerLocalActivity)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerNewNumberSynapses)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerRegionHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerRegionWidth)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSegmentThreshold)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label14);
			this.groupBox4.Controls.Add(this.label15);
			this.groupBox4.Controls.Add(this.spinnerInputHeight);
			this.groupBox4.Controls.Add(this.spinnerInputWidth);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Controls.Add(this.labelLocalActivity);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.spinnerMinOverlap);
			this.groupBox4.Controls.Add(this.labelMinOverlap);
			this.groupBox4.Controls.Add(this.labelRegionHeight);
			this.groupBox4.Controls.Add(this.spinnerInputPerColumn);
			this.groupBox4.Controls.Add(this.spinnerCellsPerColumn);
			this.groupBox4.Controls.Add(this.spinnerLocalityRadius);
			this.groupBox4.Controls.Add(this.labelRegionWidth);
			this.groupBox4.Controls.Add(this.spinnerLocalActivity);
			this.groupBox4.Controls.Add(this.spinnerNewNumberSynapses);
			this.groupBox4.Controls.Add(this.labelInputPerColumn);
			this.groupBox4.Controls.Add(this.spinnerRegionHeight);
			this.groupBox4.Controls.Add(this.labelLocalityRadius);
			this.groupBox4.Controls.Add(this.spinnerRegionWidth);
			this.groupBox4.Controls.Add(this.spinnerSegmentThreshold);
			this.groupBox4.Location = new System.Drawing.Point(12, 12);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(361, 184);
			this.groupBox4.TabIndex = 1;
			this.groupBox4.TabStop = false;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(189, 19);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 20);
			this.label14.TabIndex = 16;
			this.label14.Text = "Input Height";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label14, "If the input has a defined column grid of 100x50 then the height is 50.");
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(14, 19);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(117, 20);
			this.label15.TabIndex = 15;
			this.label15.Text = "Input Width";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label15, "If the input has a defined column grid of 100x50 then the width is 100.");
			// 
			// spinnerInputHeight
			// 
			this.spinnerInputHeight.Location = new System.Drawing.Point(299, 19);
			this.spinnerInputHeight.Name = "spinnerInputHeight";
			this.spinnerInputHeight.Size = new System.Drawing.Size(46, 20);
			this.spinnerInputHeight.TabIndex = 1;
			this.spinnerInputHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerInputHeight, "Number of input bits in the Y direction from a region/sensor that feed this regio" +
        "n.");
			// 
			// spinnerInputWidth
			// 
			this.spinnerInputWidth.Location = new System.Drawing.Point(137, 19);
			this.spinnerInputWidth.Name = "spinnerInputWidth";
			this.spinnerInputWidth.Size = new System.Drawing.Size(46, 20);
			this.spinnerInputWidth.TabIndex = 0;
			this.spinnerInputWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerInputWidth, "Number of input bits in the X direction from a region/sensor that feed this regio" +
        "n.");
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(14, 97);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(117, 20);
			this.label9.TabIndex = 14;
			this.label9.Text = "Cells per Column";
			this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label9, "The number of context learning cells per column.");
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(14, 123);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(117, 20);
			this.label8.TabIndex = 13;
			this.label8.Text = "New Synapse Count";
			this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label8, "The number of new distal synapses added to segments if no matching ones are found" +
        " during learning.");
			// 
			// labelLocalActivity
			// 
			this.labelLocalActivity.Location = new System.Drawing.Point(189, 149);
			this.labelLocalActivity.Name = "labelLocalActivity";
			this.labelLocalActivity.Size = new System.Drawing.Size(104, 20);
			this.labelLocalActivity.TabIndex = 12;
			this.labelLocalActivity.Text = "Local Activity (%)";
			this.labelLocalActivity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.labelLocalActivity, "Approximate percent of columns within average receptive field radius to be winner" +
        "s after spatial pooling inhibition.");
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(189, 123);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(104, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "Segment Threshold";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.label3, "A threshold number of active synapses between active and non-active segment state" +
        ".");
			// 
			// spinnerMinOverlap
			// 
			this.spinnerMinOverlap.Location = new System.Drawing.Point(299, 97);
			this.spinnerMinOverlap.Name = "spinnerMinOverlap";
			this.spinnerMinOverlap.Size = new System.Drawing.Size(46, 20);
			this.spinnerMinOverlap.TabIndex = 6;
			this.spinnerMinOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerMinOverlap, "Minimum percent of column\'s proximal synapses that must be active for the column " +
        "to be considered by the spatial pooler.");
			// 
			// labelMinOverlap
			// 
			this.labelMinOverlap.Location = new System.Drawing.Point(189, 97);
			this.labelMinOverlap.Name = "labelMinOverlap";
			this.labelMinOverlap.Size = new System.Drawing.Size(104, 20);
			this.labelMinOverlap.TabIndex = 11;
			this.labelMinOverlap.Text = "Min Overlap (%)";
			this.labelMinOverlap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.labelMinOverlap, "Minimum percent of column\'s proximal synapses that must be active for column to b" +
        "e considered during spatial pooling inhibition.");
			// 
			// labelRegionHeight
			// 
			this.labelRegionHeight.Location = new System.Drawing.Point(189, 71);
			this.labelRegionHeight.Name = "labelRegionHeight";
			this.labelRegionHeight.Size = new System.Drawing.Size(104, 20);
			this.labelRegionHeight.TabIndex = 4;
			this.labelRegionHeight.Text = "Region Height";
			this.labelRegionHeight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.labelRegionHeight, "If the region has a defined column grid of 100x50 then the height is 50.");
			// 
			// spinnerInputPerColumn
			// 
			this.spinnerInputPerColumn.Location = new System.Drawing.Point(299, 45);
			this.spinnerInputPerColumn.Name = "spinnerInputPerColumn";
			this.spinnerInputPerColumn.Size = new System.Drawing.Size(46, 20);
			this.spinnerInputPerColumn.TabIndex = 2;
			this.spinnerInputPerColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerInputPerColumn, "Percent of input bits within locality radius each column has potential (proximal)" +
        " synapses for.");
			// 
			// spinnerCellsPerColumn
			// 
			this.spinnerCellsPerColumn.Location = new System.Drawing.Point(136, 97);
			this.spinnerCellsPerColumn.Name = "spinnerCellsPerColumn";
			this.spinnerCellsPerColumn.Size = new System.Drawing.Size(46, 20);
			this.spinnerCellsPerColumn.TabIndex = 5;
			this.spinnerCellsPerColumn.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerCellsPerColumn, "Number of (temporal context) cells to use for each column.");
			// 
			// spinnerLocalityRadius
			// 
			this.spinnerLocalityRadius.Location = new System.Drawing.Point(137, 149);
			this.spinnerLocalityRadius.Name = "spinnerLocalityRadius";
			this.spinnerLocalityRadius.Size = new System.Drawing.Size(46, 20);
			this.spinnerLocalityRadius.TabIndex = 9;
			this.spinnerLocalityRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerLocalityRadius, "Furthest number of columns away to allow distal synapse connections (0 means no r" +
        "estriction).");
			// 
			// labelRegionWidth
			// 
			this.labelRegionWidth.Location = new System.Drawing.Point(14, 71);
			this.labelRegionWidth.Name = "labelRegionWidth";
			this.labelRegionWidth.Size = new System.Drawing.Size(117, 20);
			this.labelRegionWidth.TabIndex = 3;
			this.labelRegionWidth.Text = "Region Width";
			this.labelRegionWidth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.labelRegionWidth, "If the region has a defined column grid of 100x50 then the width is 100.");
			// 
			// spinnerLocalActivity
			// 
			this.spinnerLocalActivity.Location = new System.Drawing.Point(299, 149);
			this.spinnerLocalActivity.Name = "spinnerLocalActivity";
			this.spinnerLocalActivity.Size = new System.Drawing.Size(46, 20);
			this.spinnerLocalActivity.TabIndex = 10;
			this.spinnerLocalActivity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerLocalActivity, "Approximate percent of columns within locality radius to be winners after inhibit" +
        "ion.");
			// 
			// spinnerNewNumberSynapses
			// 
			this.spinnerNewNumberSynapses.Location = new System.Drawing.Point(137, 123);
			this.spinnerNewNumberSynapses.Name = "spinnerNewNumberSynapses";
			this.spinnerNewNumberSynapses.Size = new System.Drawing.Size(46, 20);
			this.spinnerNewNumberSynapses.TabIndex = 7;
			this.spinnerNewNumberSynapses.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerNewNumberSynapses, "Number of \'new\' synapses to add to a segment if none activated during learning.");
			// 
			// labelInputPerColumn
			// 
			this.labelInputPerColumn.Location = new System.Drawing.Point(189, 45);
			this.labelInputPerColumn.Name = "labelInputPerColumn";
			this.labelInputPerColumn.Size = new System.Drawing.Size(104, 20);
			this.labelInputPerColumn.TabIndex = 0;
			this.labelInputPerColumn.Text = "Input per Column (%)";
			this.labelInputPerColumn.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.labelInputPerColumn, "Percentage of input bits each Column will have potential proximal (spatial) synap" +
        "ses for.");
			// 
			// spinnerRegionHeight
			// 
			this.spinnerRegionHeight.Location = new System.Drawing.Point(299, 71);
			this.spinnerRegionHeight.Name = "spinnerRegionHeight";
			this.spinnerRegionHeight.Size = new System.Drawing.Size(46, 20);
			this.spinnerRegionHeight.TabIndex = 4;
			this.spinnerRegionHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerRegionHeight, "Number of columns in the Y direction for this region.");
			// 
			// labelLocalityRadius
			// 
			this.labelLocalityRadius.Location = new System.Drawing.Point(14, 149);
			this.labelLocalityRadius.Name = "labelLocalityRadius";
			this.labelLocalityRadius.Size = new System.Drawing.Size(117, 20);
			this.labelLocalityRadius.TabIndex = 1;
			this.labelLocalityRadius.Text = "Locality Radius";
			this.labelLocalityRadius.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolTip1.SetToolTip(this.labelLocalityRadius, "Furthest number of columns away (in column grid space) to allow new distal synaps" +
        "e connections. If set to 0 then there is no restriction and connections can form" +
        " between any two columns in the region.");
			// 
			// spinnerRegionWidth
			// 
			this.spinnerRegionWidth.Location = new System.Drawing.Point(137, 71);
			this.spinnerRegionWidth.Name = "spinnerRegionWidth";
			this.spinnerRegionWidth.Size = new System.Drawing.Size(46, 20);
			this.spinnerRegionWidth.TabIndex = 3;
			this.spinnerRegionWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerRegionWidth, "Number of columns in the X direction for this region.");
			// 
			// spinnerSegmentThreshold
			// 
			this.spinnerSegmentThreshold.Location = new System.Drawing.Point(299, 123);
			this.spinnerSegmentThreshold.Name = "spinnerSegmentThreshold";
			this.spinnerSegmentThreshold.Size = new System.Drawing.Size(46, 20);
			this.spinnerSegmentThreshold.TabIndex = 8;
			this.spinnerSegmentThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerSegmentThreshold, "Minimum number of active synapses to activate a segment.");
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(298, 202);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(217, 202);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 3;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// NetConfigRegionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(388, 236);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.Controls.Add(this.groupBox4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NetConfigRegionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Region Properties";
			this.groupBox4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.spinnerInputHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerInputWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerMinOverlap)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerInputPerColumn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerCellsPerColumn)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerLocalityRadius)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerLocalActivity)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerNewNumberSynapses)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerRegionHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerRegionWidth)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSegmentThreshold)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown spinnerInputPerColumn;
        private System.Windows.Forms.NumericUpDown spinnerCellsPerColumn;
        private System.Windows.Forms.NumericUpDown spinnerNewNumberSynapses;
        private System.Windows.Forms.NumericUpDown spinnerLocalActivity;
        private System.Windows.Forms.NumericUpDown spinnerMinOverlap;
        private System.Windows.Forms.NumericUpDown spinnerRegionHeight;
        private System.Windows.Forms.NumericUpDown spinnerRegionWidth;
        private System.Windows.Forms.NumericUpDown spinnerSegmentThreshold;
        private System.Windows.Forms.NumericUpDown spinnerLocalityRadius;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelLocalActivity;
        private System.Windows.Forms.Label labelMinOverlap;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelRegionHeight;
        private System.Windows.Forms.Label labelInputPerColumn;
        private System.Windows.Forms.Label labelRegionWidth;
		private System.Windows.Forms.Label labelLocalityRadius;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.NumericUpDown spinnerInputHeight;
		private System.Windows.Forms.NumericUpDown spinnerInputWidth;
    }
}