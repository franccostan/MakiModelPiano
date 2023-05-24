using Spectrogram;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tensorflow.Summary.Types;
using Image = System.Drawing.Image;
using System.Drawing.Drawing2D;

namespace MakiModelPiano
{
    internal class WavToSpectrogram
    {
        public static void ExecuteConvertAndSlice(string filepath)
        {
            /*string directory = "C:\\Users\\lordh\\Downloads\\SpectrogramTests";
            string fileName = "C.wav";
            string filePath = Path.Combine(directory, fileName);*/
            ConvertWavToSpectrogram(filepath);
            SliceSpectrograms();
        }

        private static void ConvertWavToSpectrogram(string filePath)
        {
            (double[] audio, int sampleRate) = ReadMono(filePath);
            var sg = new SpectrogramGenerator(sampleRate, fftSize: 32768, stepSize: 500, maxFreq: 1500);

            sg.Add(audio);
            sg.Colormap = Colormap.Grayscale;

            //Bitmap image = sg.GetBitmap();
            ResizeSpectrogram(sg);
            //image.Save(AppDomain.CurrentDomain.BaseDirectory + "\\Spectrogram.png", ImageFormat.Png);
        }

        private static void ResizeSpectrogram(SpectrogramGenerator sg)
        {
            // Generate spectrogram bitmap
            Bitmap image = sg.GetBitmap();

            int desiredWidth = image.Width;
            int desiredHeight = 768;

            // Resize the image to the desired dimensions
            Bitmap resizedImage = ResizeImage(image, desiredWidth, desiredHeight);

            // Save the resized image
            resizedImage.Save(AppDomain.CurrentDomain.BaseDirectory + "\\Spectrogram.png", ImageFormat.Png);
        }

        private static void SliceSpectrograms()
        {
            // Load the spectrogram image
            Image spectrogramImage = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Spectrogram.png");

            // Set the frequency threshold
            int frequencyThreshold = 128; // Adjust this value as needed

            // Specify the folder to save the sliced images
            string saveFolderPath = AppDomain.CurrentDomain.BaseDirectory;

            // Create an instance of SpectrogramAnalyzer
            SpectrogramAnalyzer analyzer = new SpectrogramAnalyzer();

            // Slice the spectrogram image and save each slice
            List<string> savedSlicePaths = analyzer.SliceSpectrogram(spectrogramImage, frequencyThreshold, saveFolderPath);

        }

        static (double[] audio, int sampleRate) ReadMono(string filePath, double multiplier = 16_000)
        {
            var afr = new NAudio.Wave.AudioFileReader(filePath);
            int sampleRate = afr.WaveFormat.SampleRate;
            int bytesPerSample = afr.WaveFormat.BitsPerSample / 8;
            int sampleCount = (int)(afr.Length / bytesPerSample);
            int channelCount = afr.WaveFormat.Channels;
            var audio = new List<double>(sampleCount);
            var buffer = new float[sampleRate * channelCount];
            int samplesRead = 0;
            while ((samplesRead = afr.Read(buffer, 0, buffer.Length)) > 0)
                audio.AddRange(buffer.Take(samplesRead).Select(x => x * multiplier));
            return (audio.ToArray(), sampleRate);
        }

        static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(image, 0, 0, width, height);
            }
            return resizedImage;
        }
    }
}
