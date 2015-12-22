using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace OpenHTM.IDE
{
	/// <summary>
	/// Loads and saves the Elements of the NetworkConfig.xml file, that contains user entries for Network configuration
	/// Provides loaded elements as a structure to return. 
	/// </summary>
	public class NetConfig
	{
		#region Fields

		// Private singleton instance
		private static NetConfig _instance;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static NetConfig Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new NetConfig();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Parameters for the regions.
		/// </summary>
		public RegionParams TopRegionParams { get; set; }

		/// <summary>
		/// Parameters for the synapses.
		/// </summary>
		public SynapseParam SynapseParams { get; set; }

		#endregion

		#region Nested classes

		/// <summary>
		/// These are the necessary parameters to configure the Synapses.
		/// </summary>
		public enum Type
		{
			Region,
			FileSensor,
			DatabaseSensor
		}

		/// <summary>
		/// These are the necessary parameters to configure the Synapses.
		/// </summary>
		public class SynapseParam
		{
			#region Properties

			/// <summary>
			/// Initial permanences of synapses.
			/// </summary>
			public float InitialPermanence { get; set; }

			/// <summary>
			/// Synapses with permanences above this value are connected.
			/// </summary>
			public float ConnectedPermanence { get; set; }

			/// <summary>
			/// Amount permanences of synapses are incremented in learning.
			/// </summary>
			public float PermanenceIncrease { get; set; }

			/// <summary>
			/// Amount permanences of synapses are decremented in learning.
			/// </summary>
			public float PermanenceDecrease { get; set; }

			#endregion
		}

		/// <summary>
		/// These are the necessary parameters to configure the Regions.
		/// </summary>
		[XmlInclude(typeof (RegionParams))]
		[XmlInclude(typeof (FileSensorParams))]
		[XmlInclude(typeof (DatabaseSensorParams))]
		public abstract class NodeParams
		{
			#region Properties

			/// <summary>
			/// A higher region in hierarchy which this region will feed-forward it.
			/// This field must be ignored in XML serialization because it causes circular reference error.
			/// </summary>
			[XmlIgnore]
			public RegionParams ParentRegionParams { get; set; }

			/// <summary>
			/// All lower regions/sensors in hierarchy which this region will receive feed-forward input.
			/// </summary>
			public List<NodeParams> Children = new List<NodeParams>();

			/// <summary>
			/// Type of the node (Region or Sensor)
			/// </summary>
			public Type Type { get; set; }

			/// <summary>
			/// The name of the <see cref="NodeParams"/>.
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Size determines width and height of region. For UI splitted into X,Y values
			/// </summary>
			public Size Size { get; set; }

			#endregion

			#region Constructor

			/// <summary>
			/// Initializes a new instance of the <see cref="NodeParams"/> class.
			/// </summary>
			public NodeParams()
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="NodeParams"/> class.
			/// </summary>
			public NodeParams(RegionParams parentRegionParams, Type type, string name)
			{
				this.ParentRegionParams = parentRegionParams;

				// If this region is child of an higher region in the hierarchy then updates the set of child regions of the latter
				if (this.ParentRegionParams != null)
				{
					this.ParentRegionParams.Children.Add(this);
				}

				this.Type = type;
				this.Name = name;
			}

			#endregion
		}

		/// <summary>
		/// A class only to group properties related to regions.
		/// </summary>
		public class RegionParams : NodeParams
		{
			/// <summary>
			/// Input Parameter: InputSize -> Size of underlying sensor array or region
			/// </summary>
			public Size InputSize { get; set; }

			/// <summary>
			/// Input Parameter: Number of cells from input area to be active for feed-forward activation
			/// </summary>
			public int SegmentActivateThreshold { get; set; }

			/// <summary>
			/// Input Parameter: Number  of new synapses created by temporal learning
			/// </summary>
			public int NewNumberSynapses { get; set; }

			/// <summary>
			/// Input Parameter: CellsPColl, cells per column. More cells, more contextual information
			/// </summary>
			public int CellsPerColumn { get; set; }

			/// <summary>
			/// Amount of input from underlying region for feed-forward column in next region
			/// </summary>
			public float PercentageInputCol { get; set; }

			/// <summary>
			/// The minimum number of inputs that must be active for a column to be 
			/// considered during the inhibition step.
			/// </summary>
			public float PercentageMinOverlap { get; set; }

			/// <summary>
			/// A parameter controlling the number of columns that will be 
			/// winners after the inhibition step.
			/// </summary>
			public float PercentageLocalActivity { get; set; }

			/// <summary>
			/// Control the area from which feed-forward connections can be made
			/// </summary>
			public int LocalityRadius { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="NodeParams"/> class.
			/// </summary>
			public RegionParams()
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="NodeParams"/> class.
			/// </summary>
			public RegionParams(RegionParams parentRegionParams, string name)
				: base(parentRegionParams, Type.Region, name)
			{
			}
		}

		/// <summary>
		/// A class only to group properties related to file sensors.
		/// </summary>
		public class FileSensorParams : NodeParams
		{
			#region Fields

			private string _fileName;

			#endregion

			#region Properties

			/// <summary>
			/// The input file name to be handled. Returns the input file name only if it is in 
			/// the project directory, full path otherwise.
			/// </summary>
			public string FileName
			{
				get
				{
					return this._fileName;
				}
				set
				{
					string directoryName = Path.GetDirectoryName(value);

					if (directoryName == Project.ProjectFolderPath || directoryName == String.Empty)
					{
						this._fileName = value.Replace(Project.ProjectFolderPath + Path.DirectorySeparatorChar, String.Empty);
					}
					else
					{
						this._fileName = value;
					}
				}
			}

			#endregion

			/// <summary>
			/// Initializes a new instance of the <see cref="NodeParams"/> class.
			/// </summary>
			public FileSensorParams()
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="NodeParams"/> class.
			/// </summary>
			public FileSensorParams(RegionParams parentRegionParams, string name)
				: base(parentRegionParams, Type.FileSensor, name)
			{
			}

			#region Methods

			/// <summary>
			/// Gets the input file path.
			/// </summary>
			/// <returns></returns>
			public string GetInputFilePath()
			{
				if (Path.GetDirectoryName(this._fileName) == String.Empty)
				{
					return Project.ProjectFolderPath +
					       Path.DirectorySeparatorChar + this._fileName;
				}
				return this._fileName;
			}

			#endregion
		}

		/// <summary>
		/// A class only to group properties related to database sensors.
		/// </summary>
		public class DatabaseSensorParams : NodeParams
		{
			/// <summary>
			/// Data Source Parameter: Connection string of the database.
			/// </summary>
			public string DatabaseConnectionString { get; set; }

			/// <summary>
			/// Data Source Parameter: Target table of the database.
			/// </summary>
			public string DatabaseTable { get; set; }

			/// <summary>
			/// Data Source Parameter: Target field of the database table.
			/// </summary>
			public string DatabaseField { get; set; }

			/// <summary>
			/// Initializes a new instance of the <see cref="NodeParams"/> class.
			/// </summary>
			public DatabaseSensorParams()
			{
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="NodeParams"/> class.
			/// </summary>
			public DatabaseSensorParams(RegionParams parentRegionParams, string name)
				: base(parentRegionParams, Type.DatabaseSensor, name)
			{
			}
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="NetConfig"/> class.
		/// </summary>
		public NetConfig()
		{
			// Initialize top region params
			this.TopRegionParams = new RegionParams(null, "Top Region");
			this.TopRegionParams.InputSize = new Size(16, 16);
			this.TopRegionParams.SegmentActivateThreshold = 3;
			this.TopRegionParams.NewNumberSynapses = 5;
			this.TopRegionParams.CellsPerColumn = 4;
			this.TopRegionParams.Size = new Size(12, 12);
			this.TopRegionParams.PercentageInputCol = 15;
			this.TopRegionParams.PercentageMinOverlap = 10;
			this.TopRegionParams.PercentageLocalActivity = 10;
			this.TopRegionParams.LocalityRadius = 5;

			// Create a file sensor and add it as lower node to top region
			var fileSensorParams = new FileSensorParams(this.TopRegionParams, "Sensor");
			fileSensorParams.FileName = "input.txt";

			// Initialize the synapses parameters
			this.SynapseParams = new SynapseParam();
			this.SynapseParams.ConnectedPermanence = .2f;
			this.SynapseParams.InitialPermanence = .2f;
			this.SynapseParams.PermanenceIncrease = .05f;
			this.SynapseParams.PermanenceDecrease = .04f;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="NetConfig"/> class and sets it to a singleton instance.
		/// </summary>
		public static void New()
		{
			_instance = new NetConfig();
		}

		/// <summary>
		/// Loads the content from XML file to <see cref="NetConfig"/> instance.
		/// </summary>
		/// <param name="filePath">The XML file path.</param>
		public static void LoadFromFile(string filePath)
		{
			// Desearialize XML to a new instance
			var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			try
			{
				_instance = (NetConfig) new XmlSerializer(Instance.GetType()).Deserialize(fileStream);
			}
			catch (InvalidOperationException ex)
			{
				throw new Exception("Invalid network configuration file.", ex);
			}
			finally
			{
				fileStream.Close();
			}
		}

		/// <summary>
		/// Saves the content from <see cref="NetConfig"/> instance to XML file.
		/// </summary>
		/// <param name="filePath">The XML file path.</param>
		public static void SaveToFile(string filePath)
		{
			// Serialize Config instance to XML file
			var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			try
			{
				new XmlSerializer(Instance.GetType()).Serialize(fileStream, Instance);
			}
			catch (InvalidOperationException ex)
			{
				throw new Exception("Cannot save network configuration file.", ex);
			}
			finally
			{
				fileStream.Close();
			}
		}

		#endregion
	}
}
