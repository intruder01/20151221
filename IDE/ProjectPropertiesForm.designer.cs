namespace OpenHTM.IDE
{
    partial class ProjectPropertiesForm
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
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonOk = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.checkBoxTemporalLearning = new System.Windows.Forms.CheckBox();
			this.checkBoxSpatialLearning = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.spinnerIncreasePermanence = new System.Windows.Forms.NumericUpDown();
			this.spinnerInitialPermanence = new System.Windows.Forms.NumericUpDown();
			this.spinnerDecreasePermanence = new System.Windows.Forms.NumericUpDown();
			this.spinnerConnectedPermanence = new System.Windows.Forms.NumericUpDown();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinnerIncreasePermanence)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerInitialPermanence)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerDecreasePermanence)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerConnectedPermanence)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(338, 142);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(257, 142);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.checkBoxTemporalLearning);
			this.groupBox2.Controls.Add(this.checkBoxSpatialLearning);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(401, 38);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			// 
			// checkBoxTemporalLearning
			// 
			this.checkBoxTemporalLearning.AutoSize = true;
			this.checkBoxTemporalLearning.Location = new System.Drawing.Point(216, 15);
			this.checkBoxTemporalLearning.Name = "checkBoxTemporalLearning";
			this.checkBoxTemporalLearning.Size = new System.Drawing.Size(114, 17);
			this.checkBoxTemporalLearning.TabIndex = 2;
			this.checkBoxTemporalLearning.Text = "Temporal Learning";
			this.checkBoxTemporalLearning.UseVisualStyleBackColor = true;
			// 
			// checkBoxSpatialLearning
			// 
			this.checkBoxSpatialLearning.AutoSize = true;
			this.checkBoxSpatialLearning.Location = new System.Drawing.Point(12, 15);
			this.checkBoxSpatialLearning.Name = "checkBoxSpatialLearning";
			this.checkBoxSpatialLearning.Size = new System.Drawing.Size(102, 17);
			this.checkBoxSpatialLearning.TabIndex = 1;
			this.checkBoxSpatialLearning.Text = "Spatial Learning";
			this.checkBoxSpatialLearning.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.spinnerIncreasePermanence);
			this.groupBox1.Controls.Add(this.spinnerInitialPermanence);
			this.groupBox1.Controls.Add(this.spinnerDecreasePermanence);
			this.groupBox1.Controls.Add(this.spinnerConnectedPermanence);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(12, 56);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(401, 80);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// spinnerIncreasePermanence
			// 
			this.spinnerIncreasePermanence.Location = new System.Drawing.Point(336, 19);
			this.spinnerIncreasePermanence.Name = "spinnerIncreasePermanence";
			this.spinnerIncreasePermanence.Size = new System.Drawing.Size(52, 20);
			this.spinnerIncreasePermanence.TabIndex = 1;
			this.spinnerIncreasePermanence.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// spinnerInitialPermanence
			// 
			this.spinnerInitialPermanence.Location = new System.Drawing.Point(142, 45);
			this.spinnerInitialPermanence.Name = "spinnerInitialPermanence";
			this.spinnerInitialPermanence.Size = new System.Drawing.Size(52, 20);
			this.spinnerInitialPermanence.TabIndex = 2;
			this.spinnerInitialPermanence.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// spinnerDecreasePermanence
			// 
			this.spinnerDecreasePermanence.Location = new System.Drawing.Point(336, 45);
			this.spinnerDecreasePermanence.Name = "spinnerDecreasePermanence";
			this.spinnerDecreasePermanence.Size = new System.Drawing.Size(52, 20);
			this.spinnerDecreasePermanence.TabIndex = 3;
			this.spinnerDecreasePermanence.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// spinnerConnectedPermanence
			// 
			this.spinnerConnectedPermanence.Location = new System.Drawing.Point(142, 19);
			this.spinnerConnectedPermanence.Name = "spinnerConnectedPermanence";
			this.spinnerConnectedPermanence.Size = new System.Drawing.Size(52, 20);
			this.spinnerConnectedPermanence.TabIndex = 0;
			this.spinnerConnectedPermanence.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 45);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(130, 23);
			this.label3.TabIndex = 2;
			this.label3.Text = "Initial Permanence";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(200, 19);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(130, 23);
			this.label2.TabIndex = 1;
			this.label2.Text = "Permanence Increase";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(6, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(130, 23);
			this.label1.TabIndex = 0;
			this.label1.Text = "Connected Permanence";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(200, 45);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(130, 23);
			this.label4.TabIndex = 3;
			this.label4.Text = "Permanence Decrease";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ProjectPropertiesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.buttonCancel;
			this.ClientSize = new System.Drawing.Size(428, 177);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProjectPropertiesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Project Properties";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.spinnerIncreasePermanence)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerInitialPermanence)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerDecreasePermanence)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerConnectedPermanence)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox checkBoxTemporalLearning;
		private System.Windows.Forms.CheckBox checkBoxSpatialLearning;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.NumericUpDown spinnerIncreasePermanence;
		private System.Windows.Forms.NumericUpDown spinnerInitialPermanence;
		private System.Windows.Forms.NumericUpDown spinnerDecreasePermanence;
		private System.Windows.Forms.NumericUpDown spinnerConnectedPermanence;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label4;
    }
}