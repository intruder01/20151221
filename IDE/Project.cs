using System.IO;

namespace OpenHTM.IDE
{
	/// <summary>
	/// Loads and saves the Elements of the Project file, that contains user entries for Network configuration
	/// Provides loaded elements as a structure to return. 
	/// </summary>
	public class Project
	{
		#region Fields

		// Private singleton instance
		private static Project _instance;

		// Path of the current project
		public static string ProjectFolderPath = "";

		// Project properties file
		public static string ProjectPropertiesFile = "ProjectProperties.xml";

		// Neural network configuration file
		public static string NetConfigFile = "NetConfig.xml";

		// Trained data file
		public static string TrainedDataFile = "TrainedData.xml";

		// Statistics file
		public static string StatisticsFile = "Statistics.xml";

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static Project Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Project();
				}
				return _instance;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="Project"/> class and sets it to 
		/// a singleton instance.
		/// </summary>
		public static void New()
		{
			// Create new project properties
			ProjectProperties.New();

			// Create new neural network configuration
			NetConfig.New();
		}

		/// <summary>
		/// Loads the elements instance from XML file to <see cref="Project"/>.
		/// </summary>
		public static void Open()
		{
			// TODO: Read from xml

			// Load project properties from file
			ProjectProperties.LoadFromFile(ProjectFolderPath +
			                               Path.DirectorySeparatorChar + ProjectPropertiesFile);

			// Load neural network configuration from file
			NetConfig.LoadFromFile(ProjectFolderPath + Path.DirectorySeparatorChar 
			                       + NetConfigFile);
		}

		/// <summary>
		/// Saves all elements instances from <see cref="Project"/> to XML file.
		/// </summary>
		public static void Save()
		{
			// TODO: Save to xml     

			// Save project properties to file
			ProjectProperties.SaveToFile(ProjectFolderPath + Path.DirectorySeparatorChar
			                             + ProjectPropertiesFile);

			// Save neural network configuration to file
			NetConfig.SaveToFile(ProjectFolderPath + Path.DirectorySeparatorChar
			                     + NetConfigFile);
		}

		#endregion
	}
}
