using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;
using OpenHTM.CLA;
using OpenHTM.IDE.UIControls;
using WeifenLuo.WinFormsUI.Docking;
using Region = OpenHTM.CLA.Region;

namespace OpenHTM.IDE
{
	public partial class NetControllerForm : DockContent
	{
		#region Fields

		// Private singleton instance
		private static NetControllerForm _instance;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static NetControllerForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NetControllerForm();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Top node of the tree (it cannot be deleted).
		/// </summary>
		public TreeNode TopNode;

		/// <summary>
		/// Node that is selected for visualization of its details.
		/// </summary>
		public TreeNode SelectedNode;

		/// <summary>
		/// Node that is highlighted due to mouse is on it.
		/// </summary>
		public TreeNode HighlightedNode;

		#endregion

		#region Nested classes

		/// <summary>
		/// Tree node that represents region/sensors and their params.
		/// When user select any node in tree, he can change its params or then visualize its details.
		/// </summary>
		public class TreeNode
		{
			#region Fields

			// Node content
			public NetConfig.NodeParams Params;
			public Region Region;
			public FileSensor FileSensor;

			// Parent node
			public TreeNode ParentNode;

			// Child nodes in the tree
			public List<TreeNode> Children = new List<TreeNode>();

			// Indicate if this node is a region and is selected by the user.
			public bool IsSelected;

			// Space to skip horizontally between siblings
			// and vertically between generations
			private const float _offsetHorizontal = 15;
			private const float _offsetVertical = 30;

			// The node's center after arranging
			private PointF _center;

			// Drawing properties
			private Font _font = new Font("Arial", 10);
			private Pen _pen = Pens.Black;
			private Brush _foregroundBrush = Brushes.Black;
			private Brush _backgroundBrush = Brushes.White;

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the <see cref="TreeNode"/> class.
			/// </summary>
			public TreeNode(TreeNode parentNode, NetConfig.NodeParams nodeParams)
			{
				// Set fields
				this.ParentNode = parentNode;
				this.Params = nodeParams;

				// If this region is parent of lower regions/sensors in the hierarchy then create and add nodes to them.
				foreach (var childNodeParams in this.Params.Children)
				{
					var childTreeNode = new TreeNode(this, childNodeParams);
					this.Children.Add(childTreeNode);
				}
			}

			#endregion

			#region Methods

			/// <summary>
			/// Add a new child to this node.
			/// </summary>
			/// <param name="type"></param>
			/// <param name="name"></param>
			public void AddNewChild(NetConfig.Type type, string name)
			{
				TreeNode treeNode = null;
				switch (type)
				{
					case NetConfig.Type.Region:
						var newRegionParams = new NetConfig.RegionParams((NetConfig.RegionParams) this.Params, name);
						treeNode = new TreeNode(this, newRegionParams);
						break;
					case NetConfig.Type.FileSensor:
						var newFileSensorParams = new NetConfig.FileSensorParams((NetConfig.RegionParams) this.Params, name);
						treeNode = new TreeNode(this, newFileSensorParams);
						break;
					case NetConfig.Type.DatabaseSensor:
						var newDatabaseSensorParams = new NetConfig.DatabaseSensorParams((NetConfig.RegionParams) this.Params, name);
						treeNode = new TreeNode(this, newDatabaseSensorParams);
						break;
				}
				this.Children.Add(treeNode);
			}

			/// <summary>
			/// Delete this node and its child nodes from tree.
			/// </summary>
			public void Delete()
			{
				// If this region is parent of lower regions/sensors in the hierarchy then delete their nodes.
				for (int i = 0; i < this.Children.Count; i++)
				{
					this.Children[i].Delete();
				}

				// Delete this node from parent's children.
				this.ParentNode.Children.Remove(this);
			}

			// Return the size of the string plus a 10 pixel margin.
			public SizeF GetSize(Graphics g, Font font)
			{
				return g.MeasureString(this.Params.Name, font) + new SizeF(10, 10);
			}

			// Draw the nodes for the subtree rooted at this node.
			public void DrawTreeNode(Graphics g)
			{
				// Recursively make the child draw its subtree nodes.
				foreach (var child in this.Children)
				{
					// Draw the link between this node this child.
					g.DrawLine(this._pen, this._center, child._center);

					// Recursively make the child draw its subtree nodes.
					child.DrawTreeNode(g);
				}

				// Draw this node.
				Brush brush = this.IsSelected ? Brushes.LightBlue : this._backgroundBrush;
				this.Draw(this._center.X, this._center.Y, g, this._pen, brush, this._foregroundBrush, this._font);
			}

			// Draw the object centered at (x, y).
			private void Draw(float x, float y, Graphics g, Pen pen, Brush backgroundBrush, Brush foregroundBrush, Font font)
			{
				// Fill and draw a polygon at our location.
				SizeF size = this.GetSize(g, font);
				var polygon = new List<Point>();
				switch (this.Params.Type)
				{
					case NetConfig.Type.Region:
						polygon.Add(new Point((int) (x - size.Width / 2) + 10, (int) (y - size.Height / 2)));
						polygon.Add(new Point((int) (x + size.Width / 2) - 10, (int) (y - size.Height / 2)));
						break;
					case NetConfig.Type.DatabaseSensor:
					case NetConfig.Type.FileSensor:
						polygon.Add(new Point((int) (x - size.Width / 2), (int) (y - size.Height / 2)));
						polygon.Add(new Point((int) (x + size.Width / 2), (int) (y - size.Height / 2)));
						break;
				}
				polygon.Add(new Point((int) (x + size.Width / 2), (int) (y + size.Height / 2)));
				polygon.Add(new Point((int) (x - size.Width / 2), (int) (y + size.Height / 2)));
				g.FillPolygon(backgroundBrush, polygon.ToArray());
				g.DrawPolygon(pen, polygon.ToArray());

				// Draw the text.
				using (var stringFormat = new StringFormat())
				{
					stringFormat.Alignment = StringAlignment.Center;
					stringFormat.LineAlignment = StringAlignment.Center;
					g.DrawString(this.Params.Name, font, foregroundBrush, x, y, stringFormat);
				}
			}

			// Arrange the node and its children in the allowed area.
			// Set minX to indicate the right edge of our subtree.
			// Set minY to indicate the bottom edge of our subtree.
			public void Arrange(Graphics g, ref float minX, ref float minY)
			{
				// See how big this node is.
				SizeF size = this.GetSize(g, this._font);

				// Recursively arrange our children,
				// allowing room for this node.
				float x = minX;
				float biggestMinY = minY + size.Height;
				float subtreeMinY = minY + size.Height + _offsetVertical;
				foreach (var child in this.Children)
				{
					// Arrange this child's subtree.
					float childMinY = subtreeMinY;
					child.Arrange(g, ref x, ref childMinY);

					// See if this increases the biggest ymin value.
					if (biggestMinY < childMinY)
					{
						biggestMinY = childMinY;
					}

					// Allow room before the next sibling.
					x += _offsetHorizontal;
				}

				// Remove the spacing after the last child.
				if (this.Children.Count > 0)
				{
					x -= _offsetHorizontal;
				}

				// See if this node is wider than the subtree under it.
				float subtreeWidth = x - minX;
				if (size.Width > subtreeWidth)
				{
					// Center the subtree under this node.
					// Make the children rearrange themselves
					// moved to center their subtrees.
					x = minX + (size.Width - subtreeWidth) / 2;
					foreach (var child in this.Children)
					{
						// Arrange this child's subtree.
						child.Arrange(g, ref x, ref subtreeMinY);

						// Allow room before the next sibling.
						x += _offsetHorizontal;
					}

					// The subtree's width is this node's width.
					subtreeWidth = size.Width;
				}

				// Set this node's center position.
				this._center = new PointF(
					minX + subtreeWidth / 2,
					minY + size.Height / 2);

				// Increase xmin to allow room for
				// the subtree before returning.
				minX += subtreeWidth;

				// Set the return value for ymin.
				minY = biggestMinY;
			}

			// Return the node at this point (or null if there isn't one there).
			public TreeNode NodeAtPoint(Graphics g, PointF targetPoint)
			{
				// See if the point is under this node.
				if (this.IsAtPoint(g, this._font, this._center, targetPoint))
				{
					return this;
				}

				// See if the point is under a node in the subtree.
				foreach (var child in this.Children)
				{
					TreeNode hitNode = child.NodeAtPoint(g, targetPoint);
					if (hitNode != null)
					{
						return hitNode;
					}
				}

				return null;
			}

			// Return true if the node is above this point.
			// Note: The equation for an ellipse with half
			// width w and half height h centered at the origin is:
			//      x*x/w/w + y*y/h/h <= 1.
			private bool IsAtPoint(Graphics g, Font font, PointF centerPoint, PointF targetPoint)
			{
				// Get our size.
				SizeF size = this.GetSize(g, font);

				// translate so we can assume the
				// ellipse is centered at the origin.
				targetPoint.X -= centerPoint.X;
				targetPoint.Y -= centerPoint.Y;

				// Determine whether the target point is under our ellipse.
				float w = size.Width / 2;
				float h = size.Height / 2;
				return
					targetPoint.X * targetPoint.X / w / w +
					targetPoint.Y * targetPoint.Y / h / h
					<= 1;
			}

			#endregion
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NetControllerForm"/> class.
		/// </summary>
		public NetControllerForm()
		{
			this.InitializeComponent();

			// Set UI properties
			this.MdiParent = MainForm.Instance;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initalize tree nodes with base in parameters of the region/sensors
		/// starting from top region.
		/// </summary>
		public void InitializeParams()
		{
			this.TopNode = new TreeNode(null, NetConfig.Instance.TopRegionParams);
			this.SelectedNode = this.TopNode;
			this.TopNode.IsSelected = true;
			this.ArrangeTree();
		}

		/// <summary>
		/// Initialize the network starting from top region.
		/// </summary>
		public void InitializeNetwork()
		{
			// Initialize project parameters
			Global.T = 5;
			Global.SpatialLearning = ProjectProperties.Instance.SpatialLearning;
			Global.TemporalLearning = ProjectProperties.Instance.TemporalLearning;

			// Initalize synapses parameters
			Synapse.ConnectedPermanence = NetConfig.Instance.SynapseParams.ConnectedPermanence;
			Synapse.InitialPermanence = NetConfig.Instance.SynapseParams.InitialPermanence;
			Synapse.PermanenceDecrement = NetConfig.Instance.SynapseParams.PermanenceDecrease;
			Synapse.PermanenceIncrement = NetConfig.Instance.SynapseParams.PermanenceIncrease;

			// Initalize nodes including region/sensors and their parameters
			this.InitializeRegion(null, this.TopNode);
		}

		/// <summary>
		/// Initialize a region/sensor and its lower regions.
		/// </summary>
		public void InitializeRegion(TreeNode parentNode, TreeNode node)
		{
			switch (node.Params.Type)
			{
				case NetConfig.Type.Region:
					{
						var regionParams = (NetConfig.RegionParams) node.Params;

						// Percentage calculations:
						var percentageInput = (float) (regionParams.PercentageInputCol * 0.01);
						var percentageLocalActivity = (float) (regionParams.PercentageLocalActivity * 0.01);
						var percentageMinOverlap = (float) (regionParams.PercentageMinOverlap * 0.01);

						// Initalize region with params
						Region parentRegion = null;
						if (parentNode != null)
						{
							parentRegion = parentNode.Region;
						}
						var newRegion = new Region(0, parentRegion, regionParams.Size, percentageInput, percentageMinOverlap, regionParams.LocalityRadius, percentageLocalActivity, regionParams.CellsPerColumn, regionParams.SegmentActivateThreshold, regionParams.NewNumberSynapses);
						node.Region = newRegion;

						// If this region is parent of lower regions/sensors in the hierarchy then initialize them too.
						foreach (var childNode in node.Children)
						{
							this.InitializeRegion(node, childNode);
						}
					}
					break;
				case NetConfig.Type.FileSensor:
					{
						var fileSensorParams = (NetConfig.FileSensorParams) node.Params;

						// Initalize file sensor with params
						var newFileSensor = new FileSensor(parentNode.Region, fileSensorParams.GetInputFilePath());
						node.FileSensor = newFileSensor;
					}
					break;
			}
		}

		/// <summary>
		/// Rearrange the tree in order to accomodate changes like resize, new/deleted nodes, etc.
		/// </summary>
		private void ArrangeTree()
		{
			using (Graphics g = this.pictureBoxTree.CreateGraphics())
			{
				// Arrange the tree once to see how big it is.
				float minX = 0, minY = 0;
				this.TopNode.Arrange(g, ref minX, ref minY);

				// Arrange the tree again to center it horizontally.
				minX = (this.ClientSize.Width - minX) / 2;
				minY = 10;
				this.TopNode.Arrange(g, ref minX, ref minY);
			}

			this.Refresh();
		}

		// Set HighlightedNode to the node under the mouse.
		private void FindNodeUnderMouse(PointF point)
		{
			using (Graphics g = this.pictureBoxTree.CreateGraphics())
			{
				this.HighlightedNode = this.TopNode.NodeAtPoint(g, point);
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Draw the tree.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxTree_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
			this.TopNode.DrawTreeNode(e.Graphics);
		}

		/// <summary>
		/// Center the tree on the form.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxTree_Resize(object sender, EventArgs e)
		{
			this.ArrangeTree();
		}

		/// <summary>
		/// Display the text of the node under the mouse.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxTree_MouseMove(object sender, MouseEventArgs e)
		{
			// Find the node under the mouse.
			this.FindNodeUnderMouse(e.Location);
		}

		/// <summary>
		/// If this is a right button down and the
		/// mouse is over a node, display a context menu.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pictureBoxTree_MouseDown(object sender, MouseEventArgs e)
		{
			switch (e.Button)
			{
				case MouseButtons.Left:
					if (this.HighlightedNode != null && this.HighlightedNode.Params.Type == NetConfig.Type.Region)
					{
						// Select the node and updates any related information.
						this.SelectedNode.IsSelected = false;
						this.SelectedNode = this.HighlightedNode;
						this.SelectedNode.IsSelected = true;

						// Rearrange the tree to show the new node.
						this.ArrangeTree();

						// Refresh dependents tools
						StatisticsForm.Instance.RefreshControls();
						StateInformationForm.Instance.RefreshControls();
					}
					break;
				case MouseButtons.Right:
					this.FindNodeUnderMouse(e.Location);
					if (this.HighlightedNode != null)
					{
						// Don't let the user delete the top node.
						this.menuNodeAddChildRegion.Enabled = (!MainForm.Instance.SimulationInitialized && this.HighlightedNode.Params.Type != NetConfig.Type.FileSensor);
						this.menuNodeAddChildFileSensor.Enabled = (!MainForm.Instance.SimulationInitialized && this.HighlightedNode.Params.Type != NetConfig.Type.FileSensor);
						this.menuNodeDelete.Enabled = (!MainForm.Instance.SimulationInitialized && this.HighlightedNode != this.TopNode);

						// Don't let the user configure a node if network is initialized.
						this.menuNodeProperties.Enabled = !MainForm.Instance.SimulationInitialized;

						// Display the context menu.
						this.menuNode.Show(this, e.Location);
					}
					break;
			}
		}

		/// <summary>
		/// View node propeerties.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuNodeProperties_Click(object sender, EventArgs e)
		{
			switch (this.HighlightedNode.Params.Type)
			{
				case NetConfig.Type.Region:
					{
						NetConfigRegionForm.Instance.SetControlsValues();
						DialogResult dialogResult = NetConfigRegionForm.Instance.ShowDialog();
						if (dialogResult == DialogResult.OK)
						{
							MainForm.Instance.MarkProjectChanges(true);
						}
					}
					break;
				case NetConfig.Type.FileSensor:
					{
						NetConfigFileSensorForm.Instance.SetControlsValues();
						DialogResult dialogResult = NetConfigFileSensorForm.Instance.ShowDialog();
						if (dialogResult == DialogResult.OK)
						{
							MainForm.Instance.MarkProjectChanges(true);
						}
					}
					break;
				case NetConfig.Type.DatabaseSensor:
					{
						NetConfigDatabaseSensorForm.Instance.SetControlsValues();
						DialogResult dialogResult = NetConfigDatabaseSensorForm.Instance.ShowDialog();
						if (dialogResult == DialogResult.OK)
						{
							MainForm.Instance.MarkProjectChanges(true);
						}
					}
					break;
			}
		}

		/// <summary>
		/// Add a child region to the selected region.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuNodeAddChildRegion_Click(object sender, EventArgs e)
		{
			// Ask for region's name
			var inputBox = new InputBox();
			if (inputBox.ShowDialog() == DialogResult.OK)
			{
				MainForm.Instance.MarkProjectChanges(true);

				// Add new region bellow highlighted region
				this.HighlightedNode.AddNewChild(NetConfig.Type.Region, inputBox.textBoxInput.Text);

				// Rearrange the tree to show the new node.
				this.ArrangeTree();
			}
		}

		/// <summary>
		/// Add a child file sensor to the selected region.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuNodeAddChildFileSensor_Click(object sender, EventArgs e)
		{
			// Ask for sensor's name
			var inputBox = new InputBox();
			if (inputBox.ShowDialog() == DialogResult.OK)
			{
				MainForm.Instance.MarkProjectChanges(true);

				// Add new sensor bellow highlighted region
				this.HighlightedNode.AddNewChild(NetConfig.Type.FileSensor, inputBox.textBoxInput.Text);

				// Rearrange the tree to show the new node.
				this.ArrangeTree();
			}
		}

		/// <summary>
		/// Delete this node from the tree.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void menuNodeDelete_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Are you sure you want to delete this node?",
			                    "Delete Node?", MessageBoxButtons.YesNo,
			                    MessageBoxIcon.Question) == DialogResult.Yes)
			{
				MainForm.Instance.MarkProjectChanges(true);

				// Delete the node and its subtree.
				this.HighlightedNode.Delete();

				// Rearrange the tree to show the new structure.
				this.ArrangeTree();
			}
		}

		#endregion
	}
}
