using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using WeifenLuo.WinFormsUI.Docking;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AdamsLair.WinForms.PropertyEditing;
using OpenHTM.CLA;


namespace OpenHTM.IDE
{
	internal partial class MainForm : Form
	{
		#region Fields

		// Private singleton instance
		private static MainForm _instance;

		private bool _pendingProjectChanges;

		public bool SimulationInitialized;

		private int _numberSteps = 100;

		private DialogResult _dialogResult = DialogResult.None;

		private StartForm _startForm;
		private bool _projectChangesAccepted;

		//reference to simulation form
		public Simulation3D Sim3D { get; private set;}

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static MainForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new MainForm();
				}
				return _instance;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MainForm"/> class.
		/// </summary>
		private MainForm()
		{
			this.InitializeComponent();

			this.textBoxNumberSteps.Text = this._numberSteps.ToString();
			this.MarkProjectChanges(false);
			this.menuFileNew.Enabled = false;

			this._startForm = new StartForm();
			this._startForm.ButtonNewPressed += this._startForm_ButtonNewPressed;
			this._startForm.ButtonClosePressed += this._startForm_ButtonClosePressed;
			this._startForm.ButtonOpenPressed += this._startForm_ButtonOpenPressed;
			this._startForm.ButtonOpenPreviousPressed += this._startForm_ButtonOpenPreviousProjectPressed;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Show all tool windows.
		/// </summary>
		private void ShowDefaultTools()
		{
			// Show states information
			StateInformationForm.Instance.Show(this.dockPanel);
			StateInformationForm.Instance.DockState = DockState.DockRight;
			this.dockPanel.DockRightPortion = this.dockPanel.Width * .60;

			// Show statistics
			StatisticsForm.Instance.Show(this.dockPanel);
			StatisticsForm.Instance.DockState = DockState.DockBottom;

			// Show network controller
			NetControllerForm.Instance.InitializeParams();
			NetControllerForm.Instance.Show(this.dockPanel);
			NetControllerForm.Instance.DockState = DockState.Document;

			//Show Watch window
			WatchForm.Instance.InitializeParams ();
			WatchForm.Instance.Show ( this.dockPanel );
			WatchForm.Instance.DockState = DockState.Document;


		}

		/// <summary>
		/// Prepare UI to load a new configuration.
		/// </summary>
		private void CleanUp()
		{
			this.buttonInitHTM.Enabled = true;
			this.EnableSteeringButtons(false);
			this.menuView3DSimulation.Enabled = false;
			this.menuProjectProperties.Enabled = true;

			if (Simulation3D.IsActive)
			{
				Simulation3D.End();
			}
		}

		/// <summary>
		/// Provides an UI reaction to any project changes or a new or saved unchanged project.
		/// </summary>
		public void MarkProjectChanges(bool hasChanges)
		{
			this._pendingProjectChanges = hasChanges;
			this.menuFileSave.Enabled = hasChanges;
		}

		/// <summary>
		/// Checks if the current file has changed.
		/// </summary>
		private DialogResult CheckCurrentConfigChanges()
		{
			var result = DialogResult.No;

			// If changes happened, ask to user if he wish saves them
			if (this._pendingProjectChanges)
			{
				result = MessageBox.Show("Current project has changed. Do you want save this changes?"
				                         , Application.ProductName, MessageBoxButtons.YesNoCancel);
				if (result == DialogResult.Yes)
				{
					this.menuFileSave_Click(null, null);
				}
			}

			return result;
		}

		/// <summary>
		/// Enables or disables buttons in toolbar.
		/// </summary>
		/// <param name="value"></param>
		private void EnableSteeringButtons(bool value)
		{
			// Enable other control buttons:
			this.buttonStepHTM.Enabled = value;
			this.buttonFastStepHTM.Enabled = value;
			this.buttonPauseHTM.Enabled = value;
			this.buttonStopHTM.Enabled = value;
			this.textBoxNumberSteps.Enabled = value;
		}

		/// <summary>
		/// // Rebind UI controls of dependent forms.
		/// </summary>
		private void ReBind()
		{
			StatisticsForm.Instance.ReBind();
		}

		/// <summary>
		/// Refresh UI controls of dependent forms.
		/// </summary>
		private void RefreshControls()
		{
			StatisticsForm.Instance.RefreshControls();
			StateInformationForm.Instance.RefreshControls();
		}

		#endregion

		#region Events

		#region Form

		private void Form_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (Simulation3D.IsActive)
			{
				Simulation3D.End();
			}
		}

		private void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.buttonStopHTM.Enabled)
			{
				this.buttonStopHTM_Click(null, null);
			}

			if (this.CheckCurrentConfigChanges() == DialogResult.Cancel ||
			    this._dialogResult == DialogResult.Cancel)
			{
				e.Cancel = true;
			}

			Properties.Settings.Default.Save ();

		}

		private void Form_Shown(object sender, EventArgs e)
		{
			this._startForm.ShowDialog(); //bypass start form here
		}

		#endregion

		#region Menus

		/// <summary>
		/// Creates a new project.
		/// </summary>
		private void menuFileNew_Click(object sender, EventArgs e)
		{
			// Check if the current project has changed before continue operation
			this.CheckCurrentConfigChanges();
			this.CleanUp();
			this.MarkProjectChanges(false);
			this.menuFileNew.Enabled = false;

			// Initialize project state
			this.Text = "Untitled - " + Application.ProductName + " " + Application.ProductVersion;

			// Create new project
			Project.ProjectFolderPath = "";
			Project.New();
			this.menuProjectProperties_Click(this, null);

			// New project was canceled.
			if (this._projectChangesAccepted)
			{
				// Show tools like network selector, statistics, etc
				this.ShowDefaultTools();

				// Bind the UI controls
				this.ReBind();
			}
			else
			{
				if (!this._startForm.Visible)
				{
					this._startForm.ShowDialog();
				}
			}
		}

		/// <summary>
		/// Open an existing project
		/// </summary>
		private void menuFileOpen_Click(object sender, EventArgs e)
		{
			// Check if the current project has changed before continue operation
			this.CheckCurrentConfigChanges();
			
			// Reset steering buttons and 3DSimulation
			this.CleanUp();

			// Ask user for an existing repository
			var folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.SelectedPath = Application.StartupPath +
											   Path.DirectorySeparatorChar + "Data";
			//default to last project path
			if (Properties.Settings.Default.LastProjectPath.Length > 0)
			{
				if (Directory.Exists ( Properties.Settings.Default.LastProjectPath ))
				{
					folderBrowserDialog.SelectedPath = Properties.Settings.Default.LastProjectPath;
				}
			}						

			// If repository exists, continue operation
			DialogResult result = folderBrowserDialog.ShowDialog();

			if (result == DialogResult.OK)
			{
				if (File.Exists(folderBrowserDialog.SelectedPath +
				                Path.DirectorySeparatorChar + Project.ProjectPropertiesFile))
				{
					// Initialize project state
					this.MarkProjectChanges(false);
					this.Text = Path.GetFileName(folderBrowserDialog.SelectedPath) + " - "
					            + Application.ProductName + " " + Application.ProductVersion;

					// Open project
					Project.ProjectFolderPath = folderBrowserDialog.SelectedPath;
					try
					{
						// Read from XML:
						// ProjectProperties.LoadFromFile(...)
						// NetConfig.LoadFromFile(...)
						Project.Open();

						this._startForm.Close();
						this.menuFileNew.Enabled = true;

						// Show forms like network selector, statistics, etc
						this.ShowDefaultTools();

						// Bind the UI controls
						this.ReBind();

						Properties.Settings.Default.LastProjectPath = Project.ProjectFolderPath;

						// JS - Initialize network right away
						buttonInitHTM_Click( sender, e);
						buttonStepHTM_Click ( sender, e );

					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
				else
				{
					MessageBox.Show(
					                "The current repository do not have any project files.",
					                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}



		/// <summary>
		/// Open previous project
		/// </summary>
		private void menuFileOpenPrevious_Click ( object sender, EventArgs e )
		{
			// Check if the current project has changed before continue operation
			this.CheckCurrentConfigChanges ();
			this.CleanUp ();

			//// Ask user for an existing repository
			//var folderBrowserDialog = new FolderBrowserDialog ();
			//folderBrowserDialog.SelectedPath = Application.StartupPath +
			//								   Path.DirectorySeparatorChar + "Data";
			////default to last project path
			//if (Properties.Settings.Default.LastProjectPath.Length > 0)
			//{
			//	if (Directory.Exists ( Properties.Settings.Default.LastProjectPath ))
			//	{
			//		folderBrowserDialog.SelectedPath = Properties.Settings.Default.LastProjectPath;
			//	}
			//}

			// If repository exists, continue operation
			string path = Properties.Settings.Default.LastProjectPath;

			if (File.Exists ( path + 
					Path.DirectorySeparatorChar + Project.ProjectPropertiesFile ))
			{
				// Initialize project state
				this.MarkProjectChanges ( false );
				this.Text = Path.GetFileName ( path ) + " - "
							+ Application.ProductName + " " + Application.ProductVersion;

				// Open project
				Project.ProjectFolderPath = path;
				try
				{
					Project.Open ();
					this._startForm.Close ();
					this.menuFileNew.Enabled = true;

					// Show tools like network selector, statistics, etc
					this.ShowDefaultTools ();

					// Bind the UI controls
					this.ReBind ();

					Properties.Settings.Default.LastProjectPath = Project.ProjectFolderPath;

					// JS - Initialize network right away
					buttonInitHTM_Click ( sender, e );
					buttonStepHTM_Click ( sender, e );

					StateInformationPanel.StateInformationPanel_SelectionChanged += WatchForm.Instance.Handler_StateInfoPanelSelectionChanged;

				}
				catch (Exception ex)
				{
					MessageBox.Show ( ex.Message );
				}
			}
			else
			{
				MessageBox.Show (
								"The previous project repository do not have any project files.",
								"Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			}

		}

		/// <summary>
		/// Save the current project
		/// </summary>
		private void menuFileSave_Click(object sender, EventArgs e)
		{
			// If current project is new, ask user for valid repository
			if (Project.ProjectFolderPath == "")
			{
				// Ask user for valid repository
				var folderBrowserDialog = new FolderBrowserDialog();
				folderBrowserDialog.SelectedPath = Application.StartupPath +
				                                   Path.DirectorySeparatorChar + "Data";

				// If repository exists, continue operation
				DialogResult result = folderBrowserDialog.ShowDialog();
				if (result == DialogResult.OK)
				{
					Project.ProjectFolderPath = folderBrowserDialog.SelectedPath;
				}
			}

			// If repository is Ok, continue operation
			if (Project.ProjectFolderPath != "")
			{
				// Initialize project state
				this.MarkProjectChanges(false);
				this.Text = Path.GetFileName(Project.ProjectFolderPath) + " - " +
				            Application.ProductName + " " + Application.ProductVersion;

				try
				{
					Project.Save();
					//NetControllerForm.Instance.TopNode.Region.SaveToFile ( Project.ProjectFolderPath + Path.DirectorySeparatorChar
					//					 + "DataFile.xml" );
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
		}

		private void menuFileExit_Click(object sender, EventArgs e)
		{
			this.CheckCurrentConfigChanges();
			Application.Exit();
		}

		private void menuProjectProperties_Click(object sender, EventArgs e)
		{
			// Open Project properties form
			ProjectPropertiesForm.Instance.CalledFromStartForm = true;
			ProjectPropertiesForm.Instance.SetControlsValues();
			DialogResult dialogResult = ProjectPropertiesForm.Instance.ShowDialog();

			if (dialogResult == DialogResult.OK)
			{
				this.MarkProjectChanges(true);
				this.menuFileNew.Enabled = true;
				this._startForm.Close();
				this._projectChangesAccepted = true;
			}
			else
			{
				this._projectChangesAccepted = false;
			}
		}

		/// <summary>
		/// Open visualisation of HTM-Network 1: spatial and temporal pooling
		/// </summary>
		private void menuView3DSimulation_Click(object sender, EventArgs e)
		{
			Simulation3D.Start();

			//this worked
			//Simulation3D.SimEngineStarted += WatchGridForm.engine_Started;
			
			//this doesnt work - Engine is null at this point
			//Simulation3D.Engine.SelectionChangedEvent += WatchGridForm.SimSelectionChanged_Handler;

			//wire Engine events
			Simulation3D.EngineStarted += WatchForm.Instance.Handler_SimEngineStarted;
			Simulation3D.EngineShutdown += WatchForm.Instance.Handler_SimEngineShutdown;
		}

		private void menuViewStateInformation_Click(object sender, EventArgs e)
		{
			StateInformationForm.Instance.RefreshControls();
			StateInformationForm.Instance.Show(this.dockPanel);
		}

		private void menuViewStatistics_Click(object sender, EventArgs e)
		{
			StatisticsForm.Instance.RefreshControls();
			StatisticsForm.Instance.Show(this.dockPanel);
		}

		private void menuUserWiki_Click(object sender, EventArgs e)
		{
			Process.Start("https://sourceforge.net/p/openhtm/wiki/UD_Main/");
		}

		private void menuGoToWebsite_Click(object sender, EventArgs e)
		{
			Process.Start("https://sourceforge.net/p/openhtm/wiki/Home/");
		}

		private void menuAbout_Click(object sender, EventArgs e)
		{
			MessageBox.Show("v. " + Assembly.GetEntryAssembly().GetName().Version +
			                Environment.NewLine + "Get more info at our home page.", "About OpenHTM");
		}

		#endregion

		#region Toolbar

		/// <summary>
		/// Initialzes the HTM-Network by creating the htm-controller to connect to events database
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonInitHTM_Click(object sender, EventArgs e)
		{
			// Set flag initialization on
			this.SimulationInitialized = true;

			// Disable relevant buttons:
			this.buttonInitHTM.Enabled = false;
			this.EnableSteeringButtons(true);

			// There was no simulation yet.
			this.buttonPauseHTM.Enabled = false;
			this.buttonStopHTM.Enabled = false;

			// Initialize HTM Processor
			NetControllerForm.Instance.InitializeNetwork();

		}

		/// <summary>
		/// Performs a single simulation step.
		/// </summary>
		private void buttonStepHTM_Click(object sender, EventArgs e)
		{
			//try
			{
				NetControllerForm.Instance.TopNode.Region.NextTimeStep();
			}
			//catch (Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}



			// Refresh controls
			this.RefreshControls();

			this.menuView3DSimulation.Enabled = true;
			this.buttonStopHTM.Enabled = true;
		}

		/// <summary>
		/// Performs full HTM simulation.
		/// </summary>
		private void buttonFastStepHTM_Click(object sender, EventArgs e)
		{
			// In case, simulation will be asynchronous.
			this.menuView3DSimulation.Enabled = true;
			this.buttonPauseHTM.Enabled = true;
			this.buttonStopHTM.Enabled = true;
			Trace.WriteLine("Start...");

			try
			{
				for (int i = 0; i < this._numberSteps; i++)
				{
					NetControllerForm.Instance.TopNode.Region.NextTimeStep();

					// Refresh controls
					this.RefreshControls();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}

			Trace.WriteLine("Stop...");

			this.buttonPauseHTM_Click(null, null);
		}

		private void buttonPauseHTM_Click(object sender, EventArgs e)
		{
			// TODO: Pause stepping.
			this.buttonPauseHTM.Enabled = false;
		}

		private void buttonStopHTM_Click(object sender, EventArgs e)
		{
			//this._dialogResult = MessageBox.Show ( "If you proceed, current simulation (learning) will stop!\r\n"
			//									 + "You will be able to save and start a new simulation.", "Warning", MessageBoxButtons.OKCancel);
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			
			if (this._dialogResult == DialogResult.OK)
			{
				// Set flag initialization off
				this.SimulationInitialized = false;

				// Disable relevant buttons to reset
				this.EnableSteeringButtons(false);
				this.menuProjectProperties.Enabled = true;
				this.buttonInitHTM.Enabled = true;
				this.menuView3DSimulation.Enabled = false;
				Simulation3D.End();
			}
		}

		private void textBoxNumberSteps_Validated(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.textBoxNumberSteps.Text))
			{
				this.textBoxNumberSteps.Text = this._numberSteps.ToString();
				MessageBox.Show("Invalid value specified!");
				return;
			}

			foreach (var c in this.textBoxNumberSteps.Text)
			{
				if (!char.IsDigit(c))
				{
					this.textBoxNumberSteps.Text = this._numberSteps.ToString();
					MessageBox.Show("Invalid value specified!");
					return;
				}
			}

			int.TryParse(this.textBoxNumberSteps.Text, out this._numberSteps);

			if (this._numberSteps < 2)
			{
				this.textBoxNumberSteps.Text = this._numberSteps.ToString();
				MessageBox.Show("Invalid value specified!");
			}
		}

		#endregion

		#region Start Form buttons

		private void _startForm_ButtonNewPressed(object sender, EventArgs e)
		{
			this.menuFileNew_Click(this, e);
		}

		private void _startForm_ButtonOpenPressed(object sender, EventArgs e)
		{
			this.menuFileOpen_Click(this, e);
		}

		private void _startForm_ButtonOpenPreviousProjectPressed ( object sender, EventArgs e )
		{
			this.menuFileOpenPrevious_Click ( this, e );
		}

		private void _startForm_ButtonClosePressed(object sender, EventArgs e)
		{
			this.menuFileExit_Click(this, e);
		}

		#endregion

		private void MainForm_Load ( object sender, EventArgs e )
		{

		}

		#endregion

		private void btnTest1_Click ( object sender, EventArgs e )
		{
			OpenHTM.CLA.Region region = new OpenHTM.CLA.Region ( 0, null, new Size ( 3, 3 ), 10.1f, 20.2f, 3, 30.3f, 5, 2, 3, false );
			region.Initialize ();
			region.Initialized = true;
			region.NextTimeStep ();
			region.InhibitionRadius = 111;
			
			//NetControllerForm.Instance.TopNode.Region.SaveToFile ( Project.ProjectFolderPath + Path.DirectorySeparatorChar
			//					 + "DataFile.xml" );
			region.SaveToFile ( Project.ProjectFolderPath + Path.DirectorySeparatorChar
								 + "DataFile.xml" );
			
		}

		private void btnTest2_Click ( object sender, EventArgs e )
		{
			//OpenHTM.CLA.Region region = new OpenHTM.CLA.Region ( 0, null, new Size ( 3, 3 ), 10.1f, 20.2f, 3, 10f, 5, 2, 3, false );
			OpenHTM.CLA.Region region = new OpenHTM.CLA.Region ();
			//= new OpenHTM.CLA.Region ();

			//NetControllerForm.Instance.TopNode.Region.LoadFromFile ( Project.ProjectFolderPath + Path.DirectorySeparatorChar
			//					 + "DataFile.xml" );
			region.LoadFromFile ( Project.ProjectFolderPath + Path.DirectorySeparatorChar
								 + "DataFile.xml" );
		}

	
	}
}
