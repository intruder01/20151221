using System;
using System.Drawing;
using System.Windows.Forms;

namespace OpenHTM.IDE
{
	public partial class NetConfigDatabaseSensorForm : Form
	{
		#region Fields

		/// <summary>
		/// Private singleton instance.
		/// </summary>
		private static NetConfigDatabaseSensorForm _instance;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static NetConfigDatabaseSensorForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NetConfigDatabaseSensorForm();
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
		/// Initializes a new instance of the <see cref="NetConfigDatabaseSensorForm"/> class.
		/// </summary>
		private NetConfigDatabaseSensorForm()
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
			var nodeParams = (NetConfig.DatabaseSensorParams) NetControllerForm.Instance.HighlightedNode.Params;
			this.spinnerSensorWidth.Text = Convert.ToString(nodeParams.Size.Width);
			this.spinnerSensorHeight.Text = Convert.ToString(nodeParams.Size.Height);
			this.textBoxDatabaseConnectionString.Text = nodeParams.DatabaseConnectionString;
			this.textBoxDatabaseTable.Text = nodeParams.DatabaseTable;
			this.textBoxDatabaseField.Text = nodeParams.DatabaseField;
		}

		#endregion

		#region Events

		/// <summary>
		/// Check if values changed and save the,.
		/// </summary>
		private void buttonOk_Click(object sender, EventArgs e)
		{
			int width = Convert.ToInt32(this.spinnerSensorWidth.Text);
			int height = Convert.ToInt32(this.spinnerSensorHeight.Text);

			// If anything has changed
			var nodeParams = (NetConfig.DatabaseSensorParams) NetControllerForm.Instance.HighlightedNode.Params;
			if (nodeParams.Size.Width != width ||
			    nodeParams.Size.Height != height ||
			    nodeParams.DatabaseConnectionString != this.textBoxDatabaseConnectionString.Text ||
			    nodeParams.DatabaseTable != this.textBoxDatabaseTable.Text ||
			    nodeParams.DatabaseField != this.textBoxDatabaseField.Text)
			{
				// Set region params with controls values
				nodeParams.Size = new Size(width, height);
				nodeParams.DatabaseConnectionString = this.textBoxDatabaseConnectionString.Text;
				nodeParams.DatabaseTable = this.textBoxDatabaseTable.Text;
				nodeParams.DatabaseField = this.textBoxDatabaseField.Text;
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
