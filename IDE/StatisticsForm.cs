using System;
using System.ComponentModel;
using System.Windows.Forms;
using OpenHTM.CLA;
using WeifenLuo.WinFormsUI.Docking;

namespace OpenHTM.IDE
{
	internal partial class StatisticsForm : DockContent
	{
		#region Fields

		// Private singleton instance
		private static StatisticsForm _instance;

		// Selected elements to show statistics
		private Region _selectedRegion;
		private Column _selectedColumn;
		private Cell _selectedCell;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static StatisticsForm Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new StatisticsForm();
				}
				return _instance;
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="StatisticsForm"/> class.
		/// </summary>
		public StatisticsForm()
		{
			this.InitializeComponent();

			// Set UI properties
			this.MdiParent = MainForm.Instance;
			this.tabControlMain.SelectedIndex = 1;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Bind the UI controls to an object that implements Observer design pattern
		/// </summary>
		public void ReBind()
		{
			// Bind the UI controls to an object that implements Observer design pattern
			this.dataGridColumns.AutoGenerateColumns = false;
			//this.dataColumnColumnPosition.DataPropertyName = "Position";
			//this.dataColumnColumnInputPosition.DataPropertyName = "InputPosition";
			//this.dataColumnColumnActivityRate.DataPropertyName = "ActivityRate";
			//this.dataColumnColumnPredictionPrecision.DataPropertyName = "PredictPrecision";
			this.dataColumnColumnPosition.DataPropertyName = "PositionInRegion";
			this.dataColumnColumnInputPosition.DataPropertyName = "CentralPositionInInput";
			this.dataColumnColumnActivityRate.DataPropertyName = "ActiveDutyCycle";
			this.dataColumnColumnPredictionPrecision.DataPropertyName = "PredictPrecision";

			this.dataGridCells.AutoGenerateColumns = false;
			this.dataColumnCellIndex.DataPropertyName = "Index";
			this.dataColumnCellActivityRate.DataPropertyName = "ActivityRate";
			this.dataColumnCellPredictPrecision.DataPropertyName = "PredictPrecision";
		}

		/// <summary>
		/// Refresh controls for each time step.
		/// </summary>
		public void RefreshControls()
		{
			if (NetControllerForm.Instance.SelectedNode != null && NetControllerForm.Instance.SelectedNode.Region != this._selectedRegion)
			{
				this._selectedRegion = NetControllerForm.Instance.SelectedNode.Region;

				// Bind again columns with columns from this region
				var bindingListColumns = new BindingList<Column>();
				foreach (var column in this._selectedRegion.Columns)
				{
					bindingListColumns.Add(column);
				}
				this.dataGridColumns.DataSource = bindingListColumns;
			}

			// Update region statistics controls
			this.textBoxRegionStepCounter.Text = "";
			this.textBoxRegionActivityRate.Text = "";
			this.textBoxRegionPredictionPrecision.Text = "";
			this.textBoxRegionPredictionCounter.Text = "";
			this.textBoxRegionCorrectPredictionCounter.Text = "";
			if (this._selectedRegion != null)
			{
				this.textBoxRegionStepCounter.Text = this._selectedRegion.Statistics.StepCounter.ToString();
				this.textBoxRegionActivityRate.Text = this._selectedRegion.Statistics.ActivityRate.ToString();
				this.textBoxRegionPredictionPrecision.Text = this._selectedRegion.Statistics.PredictPrecision.ToString();
				this.textBoxRegionPredictionCounter.Text = this._selectedRegion.Statistics.PredictionCounter.ToString();
				this.textBoxRegionCorrectPredictionCounter.Text = this._selectedRegion.Statistics.CorrectPredictionCounter.ToString();
			}

			// Refresh grids
			this.dataGridColumns.Refresh();
			this.dataGridCells.Refresh();
		}

		#endregion

		#region Events

		private void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Hide();
			this.Parent = null;
			e.Cancel = true;
		}

		private void dataGridColumns_SelectionChanged(object sender, EventArgs e)
		{
			if (this.dataGridColumns.SelectedRows.Count > 0)
			{
				// Get old selection
				if (this._selectedColumn != null)
				{
					this._selectedColumn.IsDataGridSelected = false;
				}

				// Retrieve selected Column
				this._selectedColumn = this.dataGridColumns.SelectedRows[0].DataBoundItem as Column;
				this._selectedColumn.IsDataGridSelected = true;

				// Bind the cells of the selected column
				this.dataGridCells.DataSource = this._selectedColumn.Cells;
			}
		}

		private void dataGridCells_SelectionChanged(object sender, EventArgs e)
		{
			if (this.dataGridCells.SelectedRows.Count > 0)
			{
				// Get old selection
				if (this._selectedCell != null)
				{
					this._selectedCell.IsDataGridSelected = false;
				}

				// Retrieve selected Column
				this._selectedCell = this.dataGridCells.SelectedRows[0].DataBoundItem as Cell;
				this._selectedCell.IsDataGridSelected = true;

				// Deactivate Column selection:
				Column selectedColumn = this._selectedCell.Column;
				selectedColumn.IsDataGridSelected = false;
			}
		}

		#endregion




	}
}
