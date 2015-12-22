using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace OpenHTM.IDE
{
	public partial class NetConfigFileSensorForm : Form
	{
		#region Fields

		/// <summary>
		/// Private singleton instance.
		/// </summary>
		private static NetConfigFileSensorForm _instance;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static NetConfigFileSensorForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NetConfigFileSensorForm();
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
		/// Initializes a new instance of the <see cref="NetConfigFileSensorForm"/> class.
		/// </summary>
		private NetConfigFileSensorForm()
		{
			this.InitializeComponent();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Set controls values from a class instance.
		/// </summary>
		public void SetControlsValues()
		{
			// Set controls value with sensor params
			var nodeParams = (NetConfig.FileSensorParams) NetControllerForm.Instance.HighlightedNode.Params;
			this.spinnerSensorWidth.Text = Convert.ToString(nodeParams.Size.Width);
			this.spinnerSensorHeight.Text = Convert.ToString(nodeParams.Size.Height);
			this.textBoxFile.Text = nodeParams.FileName;
		}

		#endregion

		#region Events

		/// <summary>
		/// Check if values changed and save the,.
		/// </summary>
		private void buttonOk_Click(object sender, EventArgs e)
		{
			if (this.textBoxFile.Text == string.Empty)
			{
				MessageBox.Show("Input stream file was not found or specified.",
				                "Warning", MessageBoxButtons.OK);
				return;
			}

			int width = Convert.ToInt32(this.spinnerSensorWidth.Text);
			int height = Convert.ToInt32(this.spinnerSensorHeight.Text);

			// If anything has changed
			var nodeParams = (NetConfig.FileSensorParams) NetControllerForm.Instance.HighlightedNode.Params;
			if (nodeParams.Size.Width != width ||
			    nodeParams.Size.Height != height ||
			    nodeParams.FileName != this.textBoxFile.Text)
			{
				// Set region params with controls values
				nodeParams.Size = new Size(width, height);
				nodeParams.FileName = this.textBoxFile.Text;
				this.DialogResult = DialogResult.OK;
			}

			this.Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void buttonBrowseFile_Click(object sender, EventArgs e)
		{
			// Ask user for an existing file
			var openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Application.StartupPath +
			                                  Path.DirectorySeparatorChar + "Data";
			openFileDialog.Filter = "OpenHTM Input files (*.txt)|*.txt";
			openFileDialog.CheckFileExists = true;

			// If file exists, set data source file
			DialogResult result = openFileDialog.ShowDialog();
			if (result == DialogResult.OK)
			{
				// Set file
				this.textBoxFile.Text = openFileDialog.FileName;
			}
		}

		#endregion
	}
}
