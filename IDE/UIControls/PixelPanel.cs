using System.Drawing;
using System.Windows.Forms;

namespace OpenHTM.IDE.UIControls
{
	public partial class PixelPanel : Panel
	{
		public PixelPanel()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Draw an bit map representation in this panel
		/// </summary>
		public void DrawBitMap(int[,] data, bool useGrayScale)
		{
			const int rectangleSideLenght = 5;
			const int rectangleSpaceWidth = 1;

			int originalWidth = (data.GetUpperBound(0) + 1);
			int originalHeight = (data.GetUpperBound(1) + 1);

			var resultBitmap =
				new Bitmap(
					(rectangleSideLenght + rectangleSpaceWidth) * originalWidth,
					(rectangleSideLenght + rectangleSpaceWidth) * originalHeight);
			Graphics g = Graphics.FromImage(resultBitmap);

			for (int x = 0; x < originalWidth; x++)
			{
				for (int y = 0; y < originalHeight; y++)
				{
					// Create a rectangle
					var rectangle = new Rectangle
					{
						Width = rectangleSideLenght,
						Height = rectangleSideLenght,
						X = x * (rectangleSideLenght + rectangleSpaceWidth),
						Y = y * (rectangleSideLenght + rectangleSpaceWidth)
					};

					var penLightGray = new Pen(Color.LightGray, 0.5f);
					var brush = new SolidBrush(Color.Black);
					if (data[x, y] == 0)
					{
						brush.Color = Color.White;
					}
					else
					{
						if (useGrayScale)
						{
							// The less the number, the darker the color. For example:
							// RGB(0, 0, 0) means 'black', RGB(255, 255, 255) means 'white'
							int color = 255 - data[x, y];

							// Reduces to 80% the color number in order to the color don't
							// get too light
							color = (int) (color * 0.8);

							brush.Color = Color.FromArgb(color, color, color);
						}
						else
						{
							brush.Color = Color.Black;
						}
					}

					g.FillRectangle(brush, rectangle);
					g.DrawRectangle(penLightGray, rectangle);
				}
			}
			// Copy the image and scale it to fit the panel.
			this.CreateGraphics().DrawImage(resultBitmap, 0, 0, this.Width, this.Height);

			// If not disposed, may cause memory leak.
			resultBitmap.Dispose();
		}
	}
}
