using System.Drawing;
using System.IO;
using OpenHTM.CLA;
using Region = OpenHTM.CLA.Region;

namespace OpenHTM.IDE
{
	/// <summary>
	/// A data structure representing a sensor that get data from a file.
	/// </summary>
	public class FileSensor : INode
	{
		/// <summary>
		/// A higher region in hierarchy which this file will feed-forward it.
		/// </summary>
		public Region ParentRegion { get; set; }

		/// <summary>
		/// The width and height of the records.
		/// </summary>
		public Size Size { get; set; }

		/// <summary>
		/// Indicates if file reading was started or not.
		/// </summary>
		public bool Initialized { get; set; }

		/// <summary>
		/// The output is a array representing current record that come from this file.
		/// </summary>
		private int[,] _output;

		/// <summary>
		/// File name to be handled.
		/// </summary>
		private string _file;

		/// <summary>
		/// File stream to handle the file.
		/// </summary>
		private FileStream _fileStream;

		/// <summary>
		/// Constructor
		/// </summary>
		public FileSensor(Region parentRegion, string file)
		{
			// Set fields
			this.ParentRegion = parentRegion;
			this._file = file;

			// Since this sensor is child of an higher region in the hierarchy then updates the set of children of the latter
			if (this.ParentRegion != null)
			{
				this.ParentRegion.Children.Add(this);
			}
		}

		/// <summary>
		/// Perfoms actions related to time step progression.
		/// </summary>
		public void NextTimeStep()
		{
			// If file reading did not start, then open it and place the cursor on the first record
			if (!this.Initialized)
			{
				this.Initialize();
				this.Initialized = true;
			}

			// Get next record in file
			// If file reading is over then start from scratch again
			this.GetNextRecord();

			// Feedforward input to the parent region
			this.ParentRegion.SetInput(this.GetOutput());
		}

		/// <summary>
		/// Get the output to the current time step.
		/// The output is a array representing current data that come from this file.
		/// </summary>
		public int[,] GetOutput()
		{
			return this._output;
		}

		/// <summary>
		/// Open file and place cursor on the first record.
		/// </summary>
		public void Initialize()
		{
			// Open file
			this._fileStream = new FileStream(this._file, FileMode.Open, FileAccess.Read);

			// Get dimensions of the record
			int width = 0;
			int height = 0;
			int character = 0;
			do
			{
				// Read next character
				character = this._fileStream.ReadByte();

				// Check if character is 'return' and not a number, i.e. if the first record was read
				if (character == '\r')
				{
					character = this._fileStream.ReadByte();
				}
				if (character == '\n')
				{
					break;
				}

				// Pass over the line until find a 'return' character in order to get the width
				width = 0;
				while (character != '\n')
				{
					width += 1;
					character = this._fileStream.ReadByte();
					if (character == '\r')
					{
						character = this._fileStream.ReadByte();
					}
				}

				// Increments height
				height += 1;
			} while (character != 0);

			// If current file record dimensions is not the same to sensor size then throws exception
			if (this.Size.Width != width || this.Size.Height != height)
			{
				this.Size = new Size(width, height);
			}

			// Places the cursor on the first record
			this._fileStream.Position = 0;
		}

		/// <summary>
		/// Get the next record from file.
		/// If file end is reached then start reading from scratch again.
		/// </summary>
		private void GetNextRecord()
		{
			// Initialize the vector for representing the current record
			this._output = new int[this.Size.Width, this.Size.Height];

			// If end of file was reached then place cursor on the first record again
			if (this._fileStream.Position == this._fileStream.Length)
			{
				this._fileStream.Position = 0;
			}

			// Start reading from last position
			int character;
			for (int y = 0; y < this.Size.Height; y++)
			{
				for (int x = 0; x < this.Size.Width; x++)
				{
					character = this._fileStream.ReadByte();
					switch (character)
					{
						case '1':
							this._output[x, y] = 1;
							break;
						case '0':
							this._output[x, y] = 0;
							break;
						default:
							throw new FileFormatException();
					}
				}

				// Check if next char is a 'return', i.e. the row end
				character = this._fileStream.ReadByte();
				if (character == '\r')
				{
					character = this._fileStream.ReadByte();
				}
				if (character != '\n')
				{
					throw new FileFormatException();
				}
			}

			// Check if next char is a 'return', i.e. the record end
			character = this._fileStream.ReadByte();
			if (character == '\r')
			{
				character = this._fileStream.ReadByte();
			}
			if (character != '\n' && character != -1)
			{
				throw new FileFormatException();
			}
		}
	}
}
