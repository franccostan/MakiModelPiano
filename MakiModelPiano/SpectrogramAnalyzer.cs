using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakiModelPiano
{
    internal class SpectrogramAnalyzer
    {

        public List<string> SliceSpectrogram(Image spectrogramImage, int frequencyThreshold, string saveFolderPath)
        {
            Bitmap spectrogramBitmap = new Bitmap(spectrogramImage);

            int width = spectrogramBitmap.Width;
            int height = spectrogramBitmap.Height;

            List<string> savedSlicePaths = new List<string>();
            bool isInSlice = false;
            int sliceStartX = 0;
            int sliceIndex = 1;

            // Iterate over the columns of the spectrogram
            for (int x = 0; x < width; x++)
            {
                bool isFrequencyAboveThreshold = IsFrequencyAboveThreshold(spectrogramBitmap, x, height, frequencyThreshold);

                if (isFrequencyAboveThreshold && !isInSlice)
                {
                    // Start a new slice
                    isInSlice = true;
                    sliceStartX = x;
                }
                else if (!isFrequencyAboveThreshold && isInSlice)
                {
                    // End the current slice
                    isInSlice = false;

                    // Extract the slice as a separate image
                    int sliceWidth = x - sliceStartX;
                    Rectangle sliceRectangle = new Rectangle(sliceStartX, 0, sliceWidth, height);
                    Bitmap sliceBitmap = spectrogramBitmap.Clone(sliceRectangle, spectrogramBitmap.PixelFormat);

                    // Save the slice to a file
                    string sliceFilePath = Path.Combine(saveFolderPath, $"slice_{sliceIndex}.png");
                    sliceBitmap.Save(sliceFilePath);
                    savedSlicePaths.Add(sliceFilePath);

                    sliceIndex++;
                }
            }

            // If the spectrogram ends with a slice, save it as well
            if (isInSlice)
            {
                int sliceWidth = width - sliceStartX;
                Rectangle sliceRectangle = new Rectangle(sliceStartX, 0, sliceWidth, height);
                Bitmap sliceBitmap = spectrogramBitmap.Clone(sliceRectangle, spectrogramBitmap.PixelFormat);

                // Save the slice to a file
                string sliceFilePath = Path.Combine(saveFolderPath, $"slice_{sliceIndex}.png");
                sliceBitmap.Save(sliceFilePath);
                savedSlicePaths.Add(sliceFilePath);
            }

            return savedSlicePaths;
        }




        private bool IsFrequencyAboveThreshold(Bitmap spectrogramBitmap, int x, int height, int frequencyThreshold)
        {
            // Check if any pixel in the column exceeds the frequency threshold
            for (int y = 0; y < height; y++)
            {
                Color pixelColor = spectrogramBitmap.GetPixel(x, y);
                int intensity = (pixelColor.R + pixelColor.G + pixelColor.B) / 3; // Grayscale intensity

                if (intensity > frequencyThreshold)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
