namespace OpenHTM.IDE
{
    partial class NetConfigFileSensorForm
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
			this.buttonBrowseFile = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.textBoxFile = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.spinnerSensorHeight = new System.Windows.Forms.NumericUpDown();
			this.spinnerSensorWidth = new System.Windows.Forms.NumericUpDown();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSensorHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSensorWidth)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(329, 103);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(248, 103);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label14);
			this.groupBox2.Controls.Add(this.label15);
			this.groupBox2.Controls.Add(this.spinnerSensorHeight);
			this.groupBox2.Controls.Add(this.spinnerSensorWidth);
			this.groupBox2.Controls.Add(this.buttonBrowseFile);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.textBoxFile);
			this.groupBox2.Location = new System.Drawing.Point(12, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(392, 85);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			// 
			// buttonBrowseFile
			// 
			this.buttonBrowseFile.Location = new System.Drawing.Point(317, 19);
			this.buttonBrowseFile.Name = "buttonBrowseFile";
			this.buttonBrowseFile.Size = new System.Drawing.Size(59, 23);
			this.buttonBrowseFile.TabIndex = 1;
			this.buttonBrowseFile.Text = "Browse...";
			this.buttonBrowseFile.UseVisualStyleBackColor = true;
			this.buttonBrowseFile.Click += new System.EventHandler(this.buttonBrowseFile_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(15, 19);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 20);
			this.label1.TabIndex = 5;
			this.label1.Text = "File:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textBoxFile
			// 
			this.textBoxFile.Enabled = false;
			this.textBoxFile.Location = new System.Drawing.Point(54, 19);
			this.textBoxFile.Name = "textBoxFile";
			this.textBoxFile.Size = new System.Drawing.Size(257, 20);
			this.textBoxFile.TabIndex = 0;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(220, 48);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 20);
			this.label14.TabIndex = 20;
			this.label14.Text = "Sensor Height";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(14, 48);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(117, 20);
			this.label15.TabIndex = 19;
			this.label15.Text = "Sensor Width";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// spinnerSensorHeight
			// 
			this.spinnerSensorHeight.Location = new System.Drawing.Point(330, 48);
			this.spinnerSensorHeight.Name = "spinnerSensorHeight";
			this.spinnerSensorHeight.Size = new System.Drawing.Size(46, 20);
			this.spinnerSensorHeight.TabIndex = 3;
			this.spinnerSensorHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerSensorHeight, "Number of output bits in the Y direction for this sensor.");
			// 
			// spinnerSensorWidth
			// 
			this.spinnerSensorWidth.Location = new System.Drawing.Point(137, 48);
			this.spinnerSensorWidth.Name = "spinnerSensorWidth";
			this.spinnerSensorWidth.Size = new System.Drawing.Size(46, 20);
			this.spinnerSensorWidth.TabIndex = 2;
			this.spinnerSensorWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerSensorWidth, "Number of output bits in the X direction for this sensor.");
			// 
			// NetConfigFileSensorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(420, 139);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NetConfigFileSensorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "File Sensor Properties";
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSensorHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSensorWidth)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Button buttonBrowseFile;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBoxFile;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.NumericUpDown spinnerSensorHeight;
		private System.Windows.Forms.NumericUpDown spinnerSensorWidth;
    }
}