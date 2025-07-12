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
        /// Indicates whether selected word frequency data is available for export.
        /// </summary>
        private bool selectedDataReady = false;

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
                    case "3":
                        HandleSelectedWordsCount();
                        break;
                    case "4":
                        HandleExportSelectedToJson();
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
            Console.WriteLine("3 - Analyze and show selected word frequencies");
            Console.WriteLine("4 - Export selected word frequencies to JSON");
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
        /// Handles the analysis of only target words from a PDF file.
        /// Prompts for the PDF file path and a comma-separated list of target words.
        /// </summary>
        private void HandleSelectedWordsCount()
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

            Console.Write("Enter comma-separated list of target words (e.g. data, analysis, test): ");
            string? inputWords = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputWords))
            {
                Console.WriteLine("No words provided. Operation aborted.");
                return;
            }

            var selectedWords = inputWords
                .Split(", ", StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim())
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .ToList();

            if (selectedWords.Count == 0)
            {
                Console.WriteLine("No valid words entered.");
                return;
            }

            try
            {
                parser.ShowSelectedWordFrequencies(filePath, selectedWords);
                selectedDataReady = parser.SelectedWordFrequency.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
                selectedDataReady = false;
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

        /// <summary>
        /// Handles the export of selected word frequency data to a JSON file.
        /// </summary>
        private void HandleExportSelectedToJson()
        {
            if (!selectedDataReady)
            {
                Console.WriteLine("Selected word frequency data not available. Please analyze selected words first (3).");
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
                parser.ExportSelectedInJson(outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during JSON export: {ex.Message}");
            }
        }

    }
}
