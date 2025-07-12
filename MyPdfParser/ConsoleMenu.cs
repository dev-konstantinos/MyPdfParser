using System;
using System.IO;

namespace MyPdfParser
{
    /// <summary>
    /// Represents an interactive console menu for PDF word frequency analysis,
    /// including word counting and exporting frequency data to JSON.
    /// </summary>
    internal class ConsoleMenu
    {
        /// <summary>
        /// Instance of the word parser, maintaining state between operations.
        /// </summary>
        private DocWordParser parser = new DocWordParser();

        /// <summary>
        /// Indicates whether word frequency data is available for export.
        /// </summary>
        private bool frequencyDataReady = false;

        /// <summary>
        /// Starts the interactive console menu loop.
        /// </summary>
        public void Run()
        {
            Console.InputEncoding = System.Text.Encoding.UTF8;
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            while (true)
            {
                DisplayMenu();

                string? input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine("Exiting application...");
                    break;
                }

                switch (input)
                {
                    case "1":
                        HandleWordCount();
                        break;
                    case "2":
                        HandleExportToJson();
                        break;
                    default:
                        Console.WriteLine("Unknown option. Please enter 1, 2 or 0.");
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the main console menu options.
        /// </summary>
        private void DisplayMenu()
        {
            Console.WriteLine("\n=== PDF Analyzer ===");
            Console.WriteLine("1 - Count and show word frequencies");
            Console.WriteLine("2 - Export word frequencies to JSON");
            Console.WriteLine("0 - Exit");
            Console.Write("Select an option: ");
        }

        /// <summary>
        /// Handles the user's choice to count words in a PDF.
        /// Prompts for the PDF file path, performs analysis, and updates state.
        /// </summary>
        private void HandleWordCount()
        {
            Console.Write("Enter full path to PDF file (or 0 to cancel): ");
            string? filePath = Console.ReadLine();

            if (filePath == "0")
            {
                Console.WriteLine("Operation canceled.");
                return;
            }

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Invalid file path. Please try again.");
                return;
            }

            try
            {
                parser.ShowWordsByCount(filePath);
                frequencyDataReady = parser.WordFrequency.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
                frequencyDataReady = false;
            }
        }

        /// <summary>
        /// Handles the user's choice to export the word frequency data to a JSON file.
        /// Prompts for the output file path and calls the export method.
        /// </summary>
        private void HandleExportToJson()
        {
            if (!frequencyDataReady)
            {
                Console.WriteLine("Word frequency data not available. Please run word count first (1).");
                return;
            }

            Console.Write("Enter full path to save JSON file (or 0 to cancel): ");
            string? outputPath = Console.ReadLine();

            if (outputPath == "0")
            {
                Console.WriteLine("Export canceled.");
                return;
            }

            if (string.IsNullOrWhiteSpace(outputPath))
            {
                Console.WriteLine("Invalid file path. Please try again.");
                return;
            }

            try
            {
                parser.ExportCountInJson(outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during JSON export: {ex.Message}");
            }
        }
    }
}
