using System;
using System.Globalization;
using System.Windows.Forms;

namespace OpenHTM.IDE
{
	public partial class ProjectPropertiesForm : Form
	{
		#region Fields

		// Private singleton instance
		private static ProjectPropertiesForm _instance;

		// If this form is called from start form
		// then it returns Ok indepent if there're changes or not.
		public bool CalledFromStartForm;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static ProjectPropertiesForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ProjectPropertiesForm();
				}
				return _instance;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectPropertiesForm"/> class.
		/// </summary>
		private ProjectPropertiesForm()
		{
			this.InitializeComponent();

			// Set UI properties
			this.spinnerInitialPermanence.DecimalPlaces = 2;
			this.spinnerInitialPermanence.Increment = .01M;
			this.spinnerConnectedPermanence.DecimalPlaces = 2;
			this.spinnerConnectedPermanence.Increment = .01M;
			this.spinnerIncreasePermanence.DecimalPlaces = 3;
			this.spinnerIncreasePermanence.Increment = .001M;
			this.spinnerDecreasePermanence.DecimalPlaces = 3;
			this.spinnerDecreasePermanence.Increment = .001M;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Set controls values from a class instance.
		/// </summary>
		public void SetControlsValues()
		{
			// Set controls values with project properties
			ProjectProperties projectProperties = ProjectProperties.Instance;
			this.checkBoxSpatialLearning.Checked = projectProperties.SpatialLearning;
			this.checkBoxTemporalLearning.Checked = projectProperties.TemporalLearning;

			// Set controls value with synapse params
			NetConfig.SynapseParam synapseParams = NetConfig.Instance.SynapseParams;
			this.spinnerInitialPermanence.Text = Convert.ToString(synapseParams.InitialPermanence);
			this.spinnerConnectedPermanence.Text = Convert.ToString(synapseParams.ConnectedPermanence);
			this.spinnerIncreasePermanence.Text = Convert.ToString(synapseParams.PermanenceIncrease);
			this.spinnerDecreasePermanence.Text = Convert.ToString(synapseParams.PermanenceDecrease);
		}

		#endregion

		#region Event Handlers

		private void buttonOk_Click(object sender, EventArgs e)
		{
			float initialPermanence =
				Single.Parse(this.spinnerInitialPermanence.Text, NumberStyles.AllowDecimalPoint);
			float connectedPermanence =
				Single.Parse(this.spinnerConnectedPermanence.Text, NumberStyles.AllowDecimalPoint);
			float permanenceIncrease =
				Single.Parse(this.spinnerIncreasePermanence.Text, NumberStyles.AllowDecimalPoint);
			float permanenceDecrease =
				Single.Parse(this.spinnerDecreasePermanence.Text, NumberStyles.AllowDecimalPoint);

			// If anything has changed
			ProjectProperties projectProperties = ProjectProperties.Instance;
			NetConfig.SynapseParam synapseParams = NetConfig.Instance.SynapseParams;
			if (this.CalledFromStartForm ||
			    projectProperties.SpatialLearning != this.checkBoxSpatialLearning.Checked ||
			    projectProperties.TemporalLearning != this.checkBoxTemporalLearning.Checked ||
			    synapseParams.InitialPermanence != initialPermanence ||
			    synapseParams.ConnectedPermanence != connectedPermanence ||
			    synapseParams.PermanenceIncrease != permanenceIncrease ||
			    synapseParams.PermanenceDecrease != permanenceDecrease)
			{
				// Set project properties with controls values
				projectProperties.SpatialLearning = this.checkBoxSpatialLearning.Checked;
				projectProperties.TemporalLearning = this.checkBoxTemporalLearning.Checked;

				// Set synapse params with controls values
				synapseParams.InitialPermanence = initialPermanence;
				synapseParams.ConnectedPermanence = connectedPermanence;
				synapseParams.PermanenceIncrease = permanenceIncrease;
				synapseParams.PermanenceDecrease = permanenceDecrease;

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
