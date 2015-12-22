using System;
using System.Drawing;
using System.IO;
using OpenHTM.CLA;
using Xunit;
using Region = OpenHTM.CLA.Region;

namespace CLA.Tests
{
	public class RegionTests
	{
		private void DefaultSynapseValues()
		{
			Synapse.ConnectedPermanence = 0.2f;
			Synapse.InitialPermanence = 0.22f;
			Synapse.PermanenceDecrement = 0.01f;
			Synapse.PermanenceIncrement = 0.015f;
		}

		///<summary>
		///Very simple test of the HTM CLA components for correctness.  This test
		///creates a test Cell with a few isolated Synapses to test very basic synapse
		///connection functionality amongst Cells.
		///</summary>
		[Fact]
		public void TestSynapse()
		{
			this.DefaultSynapseValues();

			var cell = new Cell(null, 0);
			cell.ActiveState[Global.T] = true;
			cell.LearnState[Global.T] = true;

			Synapse syn = new DistalSynapse(cell, Synapse.ConnectedPermanence);
			Assert.Equal(true, syn.IsConnected());

			Assert.Equal(true, syn.IsActive(Global.T));
			Assert.Equal(false, syn.IsActive(Global.T - 1));
			Assert.Equal(false, syn.IsActiveFromLearning(Global.T - 1));

			syn.DecreasePermanence();
			Assert.Equal(false, syn.IsConnected());
			Assert.Equal(true, syn.IsActive(Global.T));

			cell.ActiveState[Global.T] = false;
			Assert.Equal(false, syn.IsActive(Global.T));
		}

		///<summary>
		///Very simple test of the HTM Region for correctness.  This test
		///creates a very small region (2 columns) to test very basic cell
		///connection functionality within the Region.
		///</summary>
		[Fact]
		public void TestRegion_CellStates()
		{
			this.DefaultSynapseValues();

			var data = new int[2,1];
			data[0, 0] = 1;
			data[1, 0] = 0;

			var inputSize = new Size(2, 1);
			int localityRadius = 0;
			int cellsPerCol = 1;
			int segmentActiveThreshold = 1;
			int newSynapses = 1;

			var region = new Region(0, null, inputSize, 1.0f, 1.0f, 1, localityRadius, cellsPerCol,
			                        segmentActiveThreshold, newSynapses);
			Cell cell0 = region.Columns[0].Cells[0];
			Cell cell1 = region.Columns[1].Cells[0];

			Global.TemporalLearning = false;
			region.SetInput(data);

			region.NextTimeStep();

			//at this point we expect column0 to be active and col1 to be inactive
			Assert.Equal(true, region.Columns[0].ActiveState[Global.T]);
			Assert.Equal(false, region.Columns[1].ActiveState[Global.T]);

			Global.TemporalLearning = true;
			region.NextTimeStep();

			//at this point we expect cell0 to be active+learning, cell1 to be inactive
			Assert.Equal(true, cell0.ActiveState[Global.T]);
			Assert.Equal(true, cell0.LearnState[Global.T]);
			Assert.Equal(false, cell1.ActiveState[Global.T]);

			//we expect cell1 to have a new segment with a synapse to cell0
			data[0, 0] = 0;
			data[1, 0] = 1;
			region.NextTimeStep();

			Assert.Equal(1, cell1.DistalSegments.Count);
			Assert.Equal(1, cell1.DistalSegments[0].Synapses.Count);

			var syn = (DistalSynapse) cell1.DistalSegments[0].Synapses[0];
			Assert.Equal(syn.InputSource, cell0);
			Assert.NotEqual(syn.InputSource, cell1);
		}

		///<summary>
		///This test creates a hardcoded Region of size 250x1 and feeds in data that
		///has 10% (25) elements active.  We then repeat the same sequence 10 times to
		///try to teach the region to learn the full sequence.
		///</summary>
		[Fact]
		public void TestRegion_BasicTemporalPooling()
		{
			this.DefaultSynapseValues();

			var inputSize = new Size(250, 1);
			int localityRadius = 0;
			int cellsPerCol = 1;
			int segmentActiveThreshold = 3;
			int newSynapses = 4;

			var region = new Region(0, null, inputSize, 1.0f, 1.0f, 1, localityRadius, cellsPerCol,
			                        segmentActiveThreshold, newSynapses);

			var data = new int[250,1];
			Global.TemporalLearning = true;
			region.SetInput(data);

			//create a sequence of length 10.  repeat it 10 times and check region accuracy.
			for (int k = 0; k < 10; ++k)
			{
				for (int i = 0; i < 10; ++i)
				{
					for (int j = 0; j < 250; ++j) //reset all data to 0
					{
						data[j, 0] = 0;
					}
					for (int j = 0; j < 25; ++j) //assign next set of 25 to 1's
					{
						data[(i * 25) + j, 0] = 1;
					}

					region.NextTimeStep();

					// Expect 25 active columns matching the 25 active input bits
					Assert.Equal(25, region.Statistics.NumberActiveColumns);

					if (k > 1 || (k == 1 && i >= 1))
					{
						//after 1 full sequence presented, we expect 100% accuracy
						Assert.Equal(1.0f, region.Statistics.ColumnActivationAccuracy, 5);
						Assert.Equal(1.0f, region.Statistics.ColumnPredictionAccuracy, 5);
					}
					else
					{
						//before that we expect 0% accuracy
						Assert.Equal(0.0f, region.Statistics.ColumnActivationAccuracy, 5);
						Assert.Equal(0.0f, region.Statistics.ColumnPredictionAccuracy, 5);
					}
				}
			}
		}

		///<summary>
		///This test creates a Region of size 32x32 columns that accepts a larger
		///input of size 128x128.  Each input has a 128x128 sparse bit representation with about
		///5% of the bits active.  This example is much closer to a Region that works on
		///real world sized data.  It tests the ability of the spatial pooler to produce a
		///sparse represenation in a 32x32 column grid from a much larger 128x128 input array.
		///</summary>
		[Fact]
		public void testRegion_BasicSpatialTemporalPooling()
		{
			this.DefaultSynapseValues();

			var inputSize = new Size(128, 128); //input data is size 128x128
			var regionSize = new Size(32, 32); //region's column grid is size 32x32

			float pctInputPerCol = 0.01f; //each column connects to 1% random input bits
			float pctMinOverlap = 0.07f; //7% of column bits at minimum to be active
			int localityRadius = 0; //columns can connect anywhere within input
			float pctLocalActivity = 0.5f; //half of columns within radius inhibited
			int cellsPerCol = 4;
			int segActiveThreshold = 10;
			int newSynapseCount = 10;

			var region = new Region(0, null, regionSize, pctInputPerCol, pctMinOverlap,
			                        localityRadius, pctLocalActivity, cellsPerCol, segActiveThreshold,
			                        newSynapseCount, true);
			Global.SpatialLearning = false;
			Global.TemporalLearning = true;

			int dataSize = inputSize.Width * inputSize.Height;
			var data = new int[inputSize.Width,inputSize.Height];
			region.SetInput(data);

			int iters = 0;
			int accCount = 0;
			double accSum = 0.0;

			for (int k = 0; k < 10; ++k)
			{
				for (int i = 0; i < 10; ++i)
				{
					iters++;
					//data will contain a 128x128 bit representation
					for (int di = 0; di < inputSize.Width; ++di) //reset all data to 0
					{
						for (int dj = 0; dj < inputSize.Height; ++dj)
						{
							data[di, dj] = 0;
						}
					}

					for (int j = 0; j < dataSize / 10; ++j) //assign next 10% set to 1's
					{
						int index = (i * (dataSize / 10)) + j;
						int di = index == 0 ? 0 : index % inputSize.Width;
						int dj = index == 0 ? 0 : index / inputSize.Width;
						data[di, dj] = 1;
					}

					region.NextTimeStep();

					// Expect 15-25 active columns per time step
					Assert.InRange(region.Statistics.NumberActiveColumns, 15, 25);

					float columnActivationAccuracy = region.Statistics.ColumnActivationAccuracy;
					float columnPredictionAccuracy = region.Statistics.ColumnPredictionAccuracy;
#if (DEBUG)
					Console.Write("\niter" + iters + "  Acc: " + columnActivationAccuracy + "  " + columnPredictionAccuracy);
					Console.Write(" nc:" + region.Statistics.NumberActiveColumns);
#endif

					//find the max sequence segment count across the Region; should be 1
					int maxSeg = 0;
					float meanSeg = 0.0f;
					float totalSeg = 0;
					foreach (var col in region.Columns)
					{
						foreach (var cell in col.Cells)
						{
							int nseg = 0;
							foreach (var seg in cell.DistalSegments)
							{
								if (seg.NumberPredictionSteps == 1)
								{
									nseg++;
								}
							}
							if (nseg > maxSeg)
							{
								maxSeg = nseg;
							}
							meanSeg += nseg;
							totalSeg += 1;
						}
					}
					meanSeg /= totalSeg;
#if (DEBUG)
					Console.Write("  maxSeg: " + maxSeg);
#endif
					if (iters > 1)
					{
						Assert.Equal(maxSeg, 1);
					}

					if (k > 0 && i > 0)
					{
						Console.Write(" ");
					}

					// Get the current column predictions.  outData is size 32x32 to match the
					// column grid.  each value represents whether the column is predicted to
					// happen soon.  a value of 1 indicates the column is predicted to be active
					// in t+1, value of 2 for t+2, etc.  value of 0 indicates column is not
					// being predicted any time soon.
					int[,] outData = region.GetPredictingColumnsByTimeStep();
					int n1 = 0, n2 = 0, n3 = 0;
					foreach (var outVal in outData)
					{
						n1 += outVal == 1 ? 1 : 0;
						n2 += outVal == 2 ? 1 : 0;
						n3 += outVal == 3 ? 1 : 0;
					}
#if (DEBUG)
					Console.Write(" np:" + n1 + " " + n2 + " " + n3);
#endif

					if (k > 1 || (k == 1 && i >= 1))
					{
						//after 1 full sequence presented, we expect 100% prediction accuracy.
						//Activation accuracy may be slightly less than 100% if some columns share
						//activation states amongst different inputs (can happen based on random
						//initial connections in the spatial pooler).
						Assert.InRange(region.Statistics.ColumnActivationAccuracy, 0.9f, 1.0f);
						Assert.Equal(1.0f, region.Statistics.ColumnPredictionAccuracy, 5);

						accCount += 1;
						accSum += region.Statistics.ColumnActivationAccuracy;

						//we also expect predicting columns to match previous active columns
						//each successive time step we expect farther-out predictions
						Assert.InRange(n1, 15, 25);
						if (k > 1)
						{
							Assert.InRange(n2, 15, 25);
							if (k > 2)
							{
								Assert.InRange(n3, 15, 25);
							}
						}
					}
					else
					{
						//before that we expect 0% accuracy and 0 predicting columns
						Assert.Equal(0.0f, region.Statistics.ColumnActivationAccuracy, 5);
						Assert.Equal(0.0f, region.Statistics.ColumnPredictionAccuracy, 5);
						if (k == 0)
						{
							Assert.Equal(0, n1);
							Assert.Equal(0, n2);
							Assert.Equal(0, n3);
						}
					}
				}
#if (DEBUG)
				Console.Write("\n");
#endif
			}

			double meanAccuracy = accSum / accCount;
#if (DEBUG)
			Console.WriteLine("total iters = " + iters);
			Console.WriteLine("meanAcc: " + meanAccuracy);
#endif
			Assert.InRange(meanAccuracy, 0.99, 1.0); //at least 99% average activation accuracy
		}

		/// <summary>
		/// Test a hardcoded spatial Region using bitmap inputs where the sequence
		/// forms a repeating pattern of "ABBCBB".  We expect the temporal pooler
		/// learning to stabilize to 100% accuracy after about 7 full passes.
		/// The results are also written as prediction bitmap files in the
		/// ./CLA.Tests/images/ABBCBBA directory.
		/// </summary>
		[Fact]
		public void TestRegion_ABBCBB()
		{
			int regionIterationsToLearnExpected = 28;
			int repeatSequencesToRun = 20;

			this.DefaultSynapseValues();
			var imageFiles = new string[]
			{
				"../CLA.Tests/images/ABBCBBA/image-A.bmp",
				"../CLA.Tests/images/ABBCBBA/image-B.bmp",
				"../CLA.Tests/images/ABBCBBA/image-B.bmp",
				"../CLA.Tests/images/ABBCBBA/image-C.bmp",
				"../CLA.Tests/images/ABBCBBA/image-B.bmp",
				"../CLA.Tests/images/ABBCBBA/image-B.bmp"
				//"../CLA.Tests/images/ABBCBBA/image-A.bmp",
			};

			string outDir = "../CLA.Tests/images/ABBCBBA/";
			var data = new int[imageFiles.Length][,];

			for (int i = 0; i < imageFiles.Length; ++i)
			{
				data[i] = this.GetNextPictureFromFile(imageFiles[i]);
			}

			int[,] sample = this.GetNextPictureFromFile(imageFiles[0]);
			var inputSize = new Size(sample.GetLength(0), sample.GetLength(1));
			int localityRadius = 0;
			int cellsPerCol = 4;
			int segmentActiveThreshold = 4;
			int newSynapses = 5;

			var region = new Region(0, null, inputSize, 1.0f, 1.0f, 1, localityRadius, cellsPerCol,
			                        segmentActiveThreshold, newSynapses);
			Global.TemporalLearning = true;

			int iters = 0;
			for (int iter = 0; iter < repeatSequencesToRun; ++iter)
			{
				for (int img = 0; img < imageFiles.Length; ++img)
				{
					iters++;
					region.SetInput(data[img]);
					region.NextTimeStep();

					// Examine region accuracy
					float columnActivationAccuracy = region.Statistics.ColumnActivationAccuracy;
					float columnPredictionAccuracy = region.Statistics.ColumnPredictionAccuracy;
#if (DEBUG)
					Console.Write("\niter=" + iter + " img=" + img + "  accuracy: " + columnActivationAccuracy + "  " + columnPredictionAccuracy);
					Console.Write(" nc: " + region.Statistics.NumberActiveColumns);
#endif

					int[,] outData = region.GetPredictingColumnsByTimeStep();
					int n1 = 0, n2 = 0, n3 = 0;

					foreach (var outVal in outData)
					{
						n1 += outVal == 1 ? 1 : 0;
						n2 += outVal == 2 ? 1 : 0;
						n3 += outVal == 3 ? 1 : 0;
					}

#if (DEBUG)
					Console.Write(" np:" + n1 + " " + n2 + " " + n3);
#endif

					this.WritePredictionsToFile(outData, outDir + "prediction-pass-" + iter + "-image-" + (img + 1) + ".bmp");

					if (iter > regionIterationsToLearnExpected)
					{
						// we expect 100% accuracy
						Assert.Equal(1.0f, region.Statistics.ColumnActivationAccuracy, 5);
						Assert.Equal(1.0f, region.Statistics.ColumnPredictionAccuracy, 5);
					}
					else
					{
						//before that we expect 0% accuracy
						//Assert.Equal(0.0f, region.ColumnActivationAccuracy, 5);
						//Assert.Equal(0.0f, region.ColumnPredictionAccuracy, 5);
					}
				}
			}
		}

		/// <summary>
		/// Read the bitmap values into the int array from the given file name.
		/// </summary>
		/// <param name="fileName">the bitmap file to read from.</param>
		/// <returns>an array sized to match the bitmap filled with 0 or 1 values.</returns>
		/// <remarks>All bitmap values greater than 128 (any channel) will be
		/// read as 1 values, while everything else is read as 0.</remarks>
		private int[,] GetNextPictureFromFile(string fileName)
		{
			var stream = new FileStream(fileName, FileMode.Open);
			var bitmap = new Bitmap(stream);
			try
			{
				int width = bitmap.Width;
				int height = bitmap.Height;
				if (width != 20 || height != 20)
				{
					throw new Exception("wrong bitmap size");
				}
				var pixels = new int[width,height];

				for (int x = 0; x < width; ++x)
				{
					for (int y = 0; y < height; ++y)
					{
						Color clrColor = bitmap.GetPixel(x, y);
						int nPixelValue = 0;
						if ((clrColor.R >= 128) || (clrColor.G >= 128) || (clrColor.B >= 128))
						{
							nPixelValue = 1;
						}
						pixels[x, y] = nPixelValue;
					}
				}

				return pixels;
			}
			finally
			{
				bitmap.Dispose();
				stream.Close();
			}
		}

		/// <summary>
		/// Write the data array values as a bitmap to the given file name.
		/// </summary>
		/// <param name="data">the data values to write.</param>
		/// <param name="fileName">the name of the file to create.</param>
		/// <remarks>All values of 1 will be written as white pixels, while
		/// everything else will be written as light gray.</remarks>
		private void WritePredictionsToFile(int[,] data, string fileName)
		{
			int width = data.GetLength(0);
			int height = data.GetLength(1);
			var outImage = new Bitmap(width, height);

			for (int x = 0; x < width; ++x)
			{
				for (int y = 0; y < height; ++y)
				{
					outImage.SetPixel(x, y, data[x, y] == 1 ? Color.White : Color.LightGray);
				}
			}

			outImage.Save(fileName);
		}

		[Fact]
		public void AverageReceptiveFieldSize_Columns_HaveExpectedNumberOfColumns()
		{
			// Arrange
			var inputSize = new Size(8, 8);
			var regionSize = new Size(4, 4);
			//var instance = new Region(inputSize, regionSize, 10, 10, 3, 3, 3, 3, 15);

			// Act
			//instance.InvokeNonPublicX("AverageReceptiveFieldSize");

			// Assert
			//Assert.Equal(instance.Columns.Length, 4 * 4);
		}

		[Fact]
		public void AdaptSegments_SegmentUpdateList_Is_Null_PositiveReinforcement_Is_True_SegmentUpdateList_Assert_IsEmpty()
		{
			//TODO new tests for updated Region design
			// Arrange
			//var instance = new Region(default(Size), default(Size), 0F, 0F, 0, 0F, 0, 0, 0);
			//List<SegmentUpdate> segmentUpdateList = null;

			// Act
			//instance.AdaptSegments(segmentUpdateList, true);

			// Assert
			//Assert.Equal(segmentUpdateList.Count(), 0);
		}

		// TODO:[Theory]
		// [InlineData(4, 4, 4, 4, 2f, 2f, 2, 2f, 2, 2, 2)]
		// public void AdaptSegments_SegmentUpdateList_Assert_IsNotEmpty(int x1, int y, int x2, int y1, float pctInputPerCol, float pctMinOverlap, 
		//     int localityRadius, float pctLocalActivity, int cellsPerCol, int segmentActiveThreshold, int newNumberSynapses)
		// {
		//     // Arrange
		//     var instance = new Region(new Size(width1, height), new Size(width2, height1), pctInputPerCol, pctMinOverlap, localityRadius, pctLocalActivity, cellsPerCol, segmentActiveThreshold, newNumberSynapses);
		//     List<SegmentUpdate> segmentUpdateList = null;

		//     // Act
		//     instance.AdaptSegments(segmentUpdateList, true);

		//     // Assert
		//     Assert.True((segmentUpdateList.Count() > 0));
		// }
	}
}
