using System;
using System.Windows.Forms;

namespace OpenHTM.IDE
{
	public partial class StartForm : Form
	{
		public event EventHandler ButtonClosePressed;
		public event EventHandler ButtonNewPressed;
		public event EventHandler ButtonOpenPressed;
		public event EventHandler ButtonOpenPreviousPressed;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NetConfigSynapseForm"/> class.
		/// </summary>
		public StartForm()
		{
			this.InitializeComponent();
		}

		#endregion

		private void buttonNew_Click(object sender, EventArgs e)
		{
			this.ButtonNewPressed(this, e);
		}

		private void buttonOpen_Click(object sender, EventArgs e)
		{
			this.ButtonOpenPressed(this, e);
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			this.ButtonClosePressed(this, e);
		}

		private void buttonOpenPrevious_Click ( object sender, EventArgs e )
		{
			this.ButtonOpenPreviousPressed ( this, e );
		}
	}
}
