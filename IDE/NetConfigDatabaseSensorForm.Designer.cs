namespace OpenHTM.IDE
{
    partial class NetConfigDatabaseSensorForm
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.textBoxDatabaseField = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.textBoxDatabaseTable = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.textBoxDatabaseConnectionString = new System.Windows.Forms.TextBox();
			this.label14 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.spinnerSensorHeight = new System.Windows.Forms.NumericUpDown();
			this.spinnerSensorWidth = new System.Windows.Forms.NumericUpDown();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSensorHeight)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSensorWidth)).BeginInit();
			this.SuspendLayout();
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(329, 155);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonOk
			// 
			this.buttonOk.Location = new System.Drawing.Point(248, 155);
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.Size = new System.Drawing.Size(75, 23);
			this.buttonOk.TabIndex = 2;
			this.buttonOk.Text = "OK";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label14);
			this.groupBox1.Controls.Add(this.label15);
			this.groupBox1.Controls.Add(this.spinnerSensorHeight);
			this.groupBox1.Controls.Add(this.spinnerSensorWidth);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.textBoxDatabaseField);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.textBoxDatabaseTable);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.textBoxDatabaseConnectionString);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(392, 137);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Source";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(20, 74);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(100, 20);
			this.label5.TabIndex = 13;
			this.label5.Text = "Field:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textBoxDatabaseField
			// 
			this.textBoxDatabaseField.Enabled = false;
			this.textBoxDatabaseField.Location = new System.Drawing.Point(126, 74);
			this.textBoxDatabaseField.Name = "textBoxDatabaseField";
			this.textBoxDatabaseField.Size = new System.Drawing.Size(255, 20);
			this.textBoxDatabaseField.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(20, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(100, 20);
			this.label4.TabIndex = 11;
			this.label4.Text = "Table:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textBoxDatabaseTable
			// 
			this.textBoxDatabaseTable.Enabled = false;
			this.textBoxDatabaseTable.Location = new System.Drawing.Point(126, 48);
			this.textBoxDatabaseTable.Name = "textBoxDatabaseTable";
			this.textBoxDatabaseTable.Size = new System.Drawing.Size(255, 20);
			this.textBoxDatabaseTable.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(20, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 20);
			this.label2.TabIndex = 8;
			this.label2.Text = "Connection String:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomRight;
			// 
			// textBoxDatabaseConnectionString
			// 
			this.textBoxDatabaseConnectionString.Enabled = false;
			this.textBoxDatabaseConnectionString.Location = new System.Drawing.Point(126, 22);
			this.textBoxDatabaseConnectionString.Name = "textBoxDatabaseConnectionString";
			this.textBoxDatabaseConnectionString.Size = new System.Drawing.Size(255, 20);
			this.textBoxDatabaseConnectionString.TabIndex = 0;
			// 
			// label14
			// 
			this.label14.Location = new System.Drawing.Point(225, 100);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(104, 20);
			this.label14.TabIndex = 24;
			this.label14.Text = "Sensor Height";
			this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label15
			// 
			this.label15.Location = new System.Drawing.Point(3, 100);
			this.label15.Name = "label15";
			this.label15.Size = new System.Drawing.Size(117, 20);
			this.label15.TabIndex = 23;
			this.label15.Text = "Sensor Width";
			this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// spinnerSensorHeight
			// 
			this.spinnerSensorHeight.Location = new System.Drawing.Point(335, 100);
			this.spinnerSensorHeight.Name = "spinnerSensorHeight";
			this.spinnerSensorHeight.Size = new System.Drawing.Size(46, 20);
			this.spinnerSensorHeight.TabIndex = 4;
			this.spinnerSensorHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerSensorHeight, "Number of output bits in the Y direction for this sensor.");
			// 
			// spinnerSensorWidth
			// 
			this.spinnerSensorWidth.Location = new System.Drawing.Point(126, 100);
			this.spinnerSensorWidth.Name = "spinnerSensorWidth";
			this.spinnerSensorWidth.Size = new System.Drawing.Size(46, 20);
			this.spinnerSensorWidth.TabIndex = 3;
			this.spinnerSensorWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.toolTip1.SetToolTip(this.spinnerSensorWidth, "Number of output bits in the X direction for this sensor.");
			// 
			// NetConfigDatabaseSensorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(420, 189);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NetConfigDatabaseSensorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Database Sensor Properties";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSensorHeight)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spinnerSensorWidth)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

		private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonOk;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox textBoxDatabaseField;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBoxDatabaseTable;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox textBoxDatabaseConnectionString;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.NumericUpDown spinnerSensorHeight;
		private System.Windows.Forms.NumericUpDown spinnerSensorWidth;
    }
}