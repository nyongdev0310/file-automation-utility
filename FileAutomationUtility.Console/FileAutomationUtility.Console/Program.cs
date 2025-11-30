using System;
using System.IO;

namespace FileAutomationUtility.ConsoleVersion
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== File Automation Utility (Console) ===");

            string inputFolder = "sample";
            string outputFolder = "sorted";

            // Optional: allow args override
            if (args.Length >= 1)
                inputFolder = args[0];
            if (args.Length >= 2)
                outputFolder = args[1];

            Console.WriteLine($"Input folder : {inputFolder}");
            Console.WriteLine($"Output folder: {outputFolder}");

            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine($"Input folder not found: {inputFolder}");
                return;
            }

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            try
            {
                ProcessFiles(inputFolder, outputFolder);
                Console.WriteLine("Automation completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while processing files:");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Copies files from inputFolder into sub‑folders of outputFolder
        /// grouped by file extension.
        /// </summary>
        private static void ProcessFiles(string inputFolder, string outputFolder)
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

                Console.WriteLine($"Copied: {file} → {dest}");
            }
        }
    }
}
