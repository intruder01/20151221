using System;
using System.Windows.Forms;

// Note: At design time, I set Modifiers = Public for the
// textbox so the main program can read its value.

namespace OpenHTM.IDE.UIControls
{
	public partial class InputBox : Form
	{
		public InputBox()
		{
			this.InitializeComponent();
		}

		// Replace Show so the program cannot use it.
		private new void Show()
		{
			throw new InvalidOperationException(
				"Use ShowDialog not Show to display this dialog");
		}
	}
}
