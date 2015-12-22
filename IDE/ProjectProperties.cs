using System.IO;
using System.Xml.Serialization;

namespace OpenHTM.IDE
{
	/// <summary>
	/// Loads and saves the Elements of the ProjectProperties.xml file, that contains user entries for project properties
	/// Provides loaded elements as a structure to return. 
	/// </summary>
	public class ProjectProperties
	{
		#region Fields

		// Private singleton instance
		private static ProjectProperties _instance;

		#endregion

		#region Properties

		/// <summary>
		/// Singleton
		/// </summary>
		public static ProjectProperties Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new ProjectProperties();
				}
				return _instance;
			}
		}

		/// <summary>
		/// Switch for spatial learning
		/// </summary>
		public bool SpatialLearning { get; set; }

		/// <summary>
		/// Switch for temporal learning
		/// </summary>
		public bool TemporalLearning { get; set; }

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectProperties"/> class.
		/// </summary>
		private ProjectProperties()
		{
			this.SpatialLearning = true;
			this.TemporalLearning = true;
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectProperties"/> class and sets it to a singleton instance.
		/// </summary>
		public static void New()
		{
			_instance = new ProjectProperties();
		}

		/// <summary>
		/// Loads the content from XML file to <see cref="ProjectProperties"/> instance.
		/// </summary>
		/// <param name="filePath">The XML file path.</param>
		public static void LoadFromFile(string filePath)
		{
			// Dessearialize XML to a new instance
			var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			_instance = (ProjectProperties) new XmlSerializer(Instance.GetType()).Deserialize(fileStream);
			fileStream.Close();
		}

		/// <summary>
		/// Saves the content from <see cref="ProjectProperties"/> instance to XML file.
		/// </summary>
		/// <param name="filePath">The XML file path.</param>
		public static void SaveToFile(string filePath)
		{
			// Serialize instance to XML file
			var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
			new XmlSerializer(Instance.GetType()).Serialize(fileStream, Instance);
			fileStream.Close();
		}

		#endregion
	}
}
