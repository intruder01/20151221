namespace OpenHTM.IDE
{
	partial class NetControllerForm
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
			this.menuNode = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.menuNodeProperties = new System.Windows.Forms.ToolStripMenuItem();
			this.menuNodeAddChildRegion = new System.Windows.Forms.ToolStripMenuItem();
			this.menuNodeAddChildFileSensor = new System.Windows.Forms.ToolStripMenuItem();
			this.menuNodeDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.pictureBoxTree = new System.Windows.Forms.PictureBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.menuNode.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxTree)).BeginInit();
			this.SuspendLayout();
			// 
			// menuNode
			// 
			this.menuNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNodeProperties,
            this.menuNodeAddChildRegion,
            this.menuNodeAddChildFileSensor,
            this.menuNodeDelete});
			this.menuNode.Name = "menuNode";
			this.menuNode.Size = new System.Drawing.Size(242, 92);
			// 
			// menuNodeProperties
			// 
			this.menuNodeProperties.Name = "menuNodeProperties";
			this.menuNodeProperties.Size = new System.Drawing.Size(241, 22);
			this.menuNodeProperties.Text = "&Properties";
			this.menuNodeProperties.Click += new System.EventHandler(this.menuNodeProperties_Click);
			// 
			// menuNodeAddChildRegion
			// 
			this.menuNodeAddChildRegion.Name = "menuNodeAddChildRegion";
			this.menuNodeAddChildRegion.Size = new System.Drawing.Size(241, 22);
			this.menuNodeAddChildRegion.Text = "&Add new child region here...";
			this.menuNodeAddChildRegion.Click += new System.EventHandler(this.menuNodeAddChildRegion_Click);
			// 
			// menuNodeAddChildFileSensor
			// 
			this.menuNodeAddChildFileSensor.Name = "menuNodeAddChildFileSensor";
			this.menuNodeAddChildFileSensor.Size = new System.Drawing.Size(241, 22);
			this.menuNodeAddChildFileSensor.Text = "&Add new child file sensor here...";
			this.menuNodeAddChildFileSensor.Click += new System.EventHandler(this.menuNodeAddChildFileSensor_Click);
			// 
			// menuNodeDelete
			// 
			this.menuNodeDelete.Name = "menuNodeDelete";
			this.menuNodeDelete.Size = new System.Drawing.Size(241, 22);
			this.menuNodeDelete.Text = "&Delete this child...";
			this.menuNodeDelete.Click += new System.EventHandler(this.menuNodeDelete_Click);
			// 
			// pictureBoxTree
			// 
			this.pictureBoxTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxTree.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxTree.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxTree.Name = "pictureBoxTree";
			this.pictureBoxTree.Size = new System.Drawing.Size(349, 429);
			this.pictureBoxTree.TabIndex = 2;
			this.pictureBoxTree.TabStop = false;
			this.toolTip1.SetToolTip(this.pictureBoxTree, "Left-Click: Select region.\r\nRigh-Click: Show options for region or sensor.");
			this.pictureBoxTree.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxTree_Paint);
			this.pictureBoxTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTree_MouseDown);
			this.pictureBoxTree.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxTree_MouseMove);
			this.pictureBoxTree.Resize += new System.EventHandler(this.pictureBoxTree_Resize);
			// 
			// NetControllerForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(349, 429);
			this.Controls.Add(this.pictureBoxTree);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "NetControllerForm";
			this.Text = "Regions Selector";
			this.menuNode.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxTree)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip menuNode;
		private System.Windows.Forms.ToolStripMenuItem menuNodeProperties;
		private System.Windows.Forms.ToolStripMenuItem menuNodeAddChildRegion;
		private System.Windows.Forms.ToolStripMenuItem menuNodeAddChildFileSensor;
		private System.Windows.Forms.ToolStripMenuItem menuNodeDelete;
		private System.Windows.Forms.PictureBox pictureBoxTree;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}