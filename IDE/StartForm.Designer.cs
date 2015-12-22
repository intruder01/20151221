namespace OpenHTM.IDE
{
    partial class StartForm
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
			this.buttonNew = new System.Windows.Forms.Button();
			this.buttonOpen = new System.Windows.Forms.Button();
			this.buttonClose = new System.Windows.Forms.Button();
			this.buttonOpenPrevious = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// buttonNew
			// 
			this.buttonNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonNew.Location = new System.Drawing.Point(12, 12);
			this.buttonNew.Name = "buttonNew";
			this.buttonNew.Size = new System.Drawing.Size(110, 53);
			this.buttonNew.TabIndex = 0;
			this.buttonNew.Text = "New Project";
			this.buttonNew.UseVisualStyleBackColor = true;
			this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
			// 
			// buttonOpen
			// 
			this.buttonOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonOpen.Location = new System.Drawing.Point(128, 12);
			this.buttonOpen.Name = "buttonOpen";
			this.buttonOpen.Size = new System.Drawing.Size(110, 53);
			this.buttonOpen.TabIndex = 1;
			this.buttonOpen.Text = "Open Project";
			this.buttonOpen.UseVisualStyleBackColor = true;
			this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
			// 
			// buttonClose
			// 
			this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonClose.Location = new System.Drawing.Point(244, 12);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(110, 53);
			this.buttonClose.TabIndex = 2;
			this.buttonClose.Text = "Close";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// buttonOpenPrevious
			// 
			this.buttonOpenPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonOpenPrevious.Location = new System.Drawing.Point(128, 83);
			this.buttonOpenPrevious.Name = "buttonOpenPrevious";
			this.buttonOpenPrevious.Size = new System.Drawing.Size(110, 53);
			this.buttonOpenPrevious.TabIndex = 3;
			this.buttonOpenPrevious.Text = "Open Previous";
			this.buttonOpenPrevious.UseVisualStyleBackColor = true;
			this.buttonOpenPrevious.Click += new System.EventHandler(this.buttonOpenPrevious_Click);
			// 
			// StartForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Honeydew;
			this.ClientSize = new System.Drawing.Size(374, 155);
			this.Controls.Add(this.buttonOpenPrevious);
			this.Controls.Add(this.buttonClose);
			this.Controls.Add(this.buttonOpen);
			this.Controls.Add(this.buttonNew);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "StartForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Synapse Options";
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.Button buttonNew;
		private System.Windows.Forms.Button buttonOpen;
		private System.Windows.Forms.Button buttonClose;
		private System.Windows.Forms.Button buttonOpenPrevious;

	}
}