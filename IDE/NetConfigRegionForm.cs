using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenHTM.IDE
{
	public partial class NetConfigRegionForm : Form
	{
		#region Fields

		/// <summary>
		/// Private singleton instance.
		/// </summary>
		private static NetConfigRegionForm _instance;

		/// <summary>
		/// Choosen region zero-based index.
		/// </summary>
		public static int ChoosenRegionIndex;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static NetConfigRegionForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NetConfigRegionForm();
				}
				return _instance;
			}
			set
			{
				_instance = value;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NetConfigRegionForm"/> class.
		/// </summary>
		private NetConfigRegionForm()
		{
			this.InitializeComponent();

			// TODO: Refine values
			this.spinnerCellsPerColumn.Minimum = 1;
			this.spinnerRegionWidth.Minimum = 1;
			this.spinnerRegionHeight.Minimum = 1;
			this.spinnerInputPerColumn.Minimum = 1;
			this.spinnerInputPerColumn.Maximum = 100;
			this.spinnerLocalityRadius.Maximum = 100;
			this.spinnerNewNumberSynapses.Maximum = 1000;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Set controls values from a class instance.
		/// </summary>
		public void SetControlsValues()
		{
			// Set controls value with region params
			var nodeParams = (NetConfig.RegionParams) NetControllerForm.Instance.HighlightedNode.Params;
			this.spinnerInputWidth.Text = Convert.ToString(nodeParams.InputSize.Width);
			this.spinnerInputHeight.Text = Convert.ToString(nodeParams.InputSize.Height);
			this.spinnerInputPerColumn.Text = Convert.ToString(nodeParams.PercentageInputCol);
			this.spinnerLocalityRadius.Text = Convert.ToString(nodeParams.LocalityRadius);
			this.spinnerSegmentThreshold.Text = Convert.ToString(nodeParams.SegmentActivateThreshold);
			this.spinnerRegionWidth.Text = Convert.ToString(nodeParams.Size.Width);
			this.spinnerRegionHeight.Text = Convert.ToString(nodeParams.Size.Height);
			this.spinnerMinOverlap.Text = Convert.ToString(nodeParams.PercentageMinOverlap);
			this.spinnerLocalActivity.Text = Convert.ToString(nodeParams.PercentageLocalActivity);
			this.spinnerNewNumberSynapses.Text = Convert.ToString(nodeParams.NewNumberSynapses);
			this.spinnerCellsPerColumn.Text = Convert.ToString(nodeParams.CellsPerColumn);
		}

		#endregion

		#region Events

		/// <summary>
		/// Check if values changed and save the,.
		/// </summary>
		private void buttonOk_Click(object sender, EventArgs e)
		{
			int width = Convert.ToInt32(this.spinnerRegionWidth.Text);
			int height = Convert.ToInt32(this.spinnerRegionHeight.Text);
			int inputWidth = Convert.ToInt32(this.spinnerInputWidth.Text);
			int inputHeight = Convert.ToInt32(this.spinnerInputHeight.Text);
			int cellsPerColumn = Convert.ToInt32(this.spinnerCellsPerColumn.Text);
			int localityRadius = Convert.ToInt32(this.spinnerLocalityRadius.Text);
			int newNumberSynapses = Convert.ToInt32(this.spinnerNewNumberSynapses.Text);
			int percentageInputCol = Convert.ToInt32(this.spinnerInputPerColumn.Text);
			int percentageMinOverlap = Convert.ToInt32(this.spinnerMinOverlap.Text);
			int percentageLocalActivity = Convert.ToInt32(this.spinnerLocalActivity.Text);
			int segmentActivateThreshold = Convert.ToInt32(this.spinnerSegmentThreshold.Text);

			// If anything has changed
			var nodeParams = (NetConfig.RegionParams) NetControllerForm.Instance.HighlightedNode.Params;
			if (nodeParams.Size.Width != width ||
			    nodeParams.Size.Height != height ||
			    nodeParams.InputSize.Width != inputWidth ||
			    nodeParams.InputSize.Height != inputHeight ||
			    nodeParams.CellsPerColumn != cellsPerColumn ||
			    nodeParams.LocalityRadius != localityRadius ||
			    nodeParams.NewNumberSynapses != newNumberSynapses ||
			    nodeParams.PercentageInputCol != percentageInputCol ||
			    nodeParams.PercentageMinOverlap != percentageMinOverlap ||
			    nodeParams.PercentageLocalActivity != percentageLocalActivity ||
			    nodeParams.SegmentActivateThreshold != segmentActivateThreshold)
			{
				// Set region params with controls values
				nodeParams.Size = new Size(width, height);
				nodeParams.InputSize = new Size(inputWidth, inputHeight);
				nodeParams.CellsPerColumn = cellsPerColumn;
				nodeParams.LocalityRadius = localityRadius;
				nodeParams.NewNumberSynapses = newNumberSynapses;
				nodeParams.PercentageInputCol = percentageInputCol;
				nodeParams.PercentageMinOverlap = percentageMinOverlap;
				nodeParams.PercentageLocalActivity = percentageLocalActivity;
				nodeParams.SegmentActivateThreshold = segmentActivateThreshold;
				this.DialogResult = DialogResult.OK;
			}

			this.Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion
	}
}
