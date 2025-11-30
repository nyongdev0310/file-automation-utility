using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileAutomationUtility.WinForms
{
    public partial class MainForm : Form
    {
        private TextBox txtInputFolder;
        private TextBox txtOutputFolder;
        private Button btnBrowseInput;
        private Button btnBrowseOutput;
        private Button btnRun;
        private TextBox txtLog;

        public MainForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.Text = "File Automation Utility";
            this.Width = 600;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            var lblInput = new Label
            {
                Text = "Input Folder:",
                Left = 20,
                Top = 20,
                Width = 90
            };

            txtInputFolder = new TextBox
            {
                Left = 120,
                Top = 18,
                Width = 340
            };

            btnBrowseInput = new Button
            {
                Text = "Browse...",
                Left = 470,
                Top = 16,
                Width = 90
            };
            btnBrowseInput.Click += BtnBrowseInput_Click;

            var lblOutput = new Label
            {
                Text = "Output Folder:",
                Left = 20,
                Top = 60,
                Width = 90
            };

            txtOutputFolder = new TextBox
            {
                Left = 120,
                Top = 58,
                Width = 340
            };

            btnBrowseOutput = new Button
            {
                Text = "Browse...",
                Left = 470,
                Top = 56,
                Width = 90
            };
            btnBrowseOutput.Click += BtnBrowseOutput_Click;

            btnRun = new Button
            {
                Text = "Run Automation",
                Left = 20,
                Top = 100,
                Width = 540,
                Height = 30
            };
            btnRun.Click += BtnRun_Click;

            txtLog = new TextBox
            {
                Left = 20,
                Top = 150,
                Width = 540,
                Height = 180,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            this.Controls.Add(lblInput);
            this.Controls.Add(txtInputFolder);
            this.Controls.Add(btnBrowseInput);
            this.Controls.Add(lblOutput);
            this.Controls.Add(txtOutputFolder);
            this.Controls.Add(btnBrowseOutput);
            this.Controls.Add(btnRun);
            this.Controls.Add(txtLog);
        }

        private void BtnBrowseInput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtInputFolder.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnBrowseOutput_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    txtOutputFolder.Text = fbd.SelectedPath;
                }
            }
        }

        private void BtnRun_Click(object sender, EventArgs e)
        {
            txtLog.Clear();

            string inputFolder = txtInputFolder.Text;
            string outputFolder = txtOutputFolder.Text;

            if (!Directory.Exists(inputFolder))
            {
                Log("Input folder not found.");
                MessageBox.Show("Input folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(outputFolder))
            {
                Log("Output folder path is empty.");
                MessageBox.Show("Please select an output folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            try
            {
                Log("Scanning files...");
                ProcessFiles(inputFolder, outputFolder);
                Log("Automation complete.");
                MessageBox.Show("File automation completed.", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Log("Error: " + ex.Message);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Copies files from inputFolder into sub‑folders of outputFolder based on file extension.
        /// Example: .txt → /txt, .csv → /csv
        /// </summary>
        private void ProcessFiles(string inputFolder, string outputFolder)
        {
            foreach (var file in Directory.GetFiles(inputFolder))
            {
                string extension = Path.GetExtension(file).TrimStart('.').ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(extension))
                    extension = "no_extension";

                string targetFolder = Path.Combine(outputFolder, extension);

                if (!Directory.Exists(targetFolder))
                    Directory.CreateDirectory(targetFolder);

                string dest = Path.Combine(targetFolder, Path.GetFileName(file));

                File.Copy(file, dest, overwrite: true);

                Log($"Copied: {file} → {dest}");
            }
        }

        private void Log(string message)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
        }
    }
}
