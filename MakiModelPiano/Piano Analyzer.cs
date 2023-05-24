using System;
using System.Windows.Forms;
using System.IO;

namespace MakiModelPiano
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void ExecuteMakiModel(string selectedFilePath)
        {
            WavToSpectrogram.ExecuteConvertAndSlice(selectedFilePath);

            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string[] slicePaths = Directory.GetFiles(folderPath, "slice_*.png");

            string outputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "predicted_labels.txt");

            // Clear the text file
            File.WriteAllText(outputFile, string.Empty);

            foreach (string slicePath in slicePaths)
            {
               var imageBytes = File.ReadAllBytes(slicePath);
                MLModel1.ModelInput sampleData = new MLModel1.ModelInput()
                {
                    ImageSource = imageBytes,
                };

                var result = MLModel1.Predict(sampleData);

                // Save predicted label to the text file
                // File.AppendAllText(outputFile, Path.GetFileName(slicePath));
                File.AppendAllText(outputFile, result.PredictedLabel + Environment.NewLine);
            }
            textBox1.Text = ("Your prediction text file is saved at:" + folderPath);
            MidiBuilder.BuildMidi();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog.FileName;
                ExecuteMakiModel(selectedFilePath);
            }
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
