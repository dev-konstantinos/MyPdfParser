using System;
using System.IO;
using System.Linq;
using System.Text;

namespace MyPdfParser
{
    /// <summary>
    /// Interactive console menu for PDF word frequency analysis and JSON export.
    /// </summary>
    internal class ConsoleMenu
    {
        private DocWordParser parser = new DocWordParser();

        private bool analysisPerformed = false;

        private enum ReportType { None, Full, Selected, NoStops }

        private ReportType lastReport = ReportType.None;

        /// <summary>
        /// Starts the interactive menu loop.
        /// </summary>
        public void Run()
        {
            Console.InputEncoding = Encoding.UTF8;
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                DisplayMenu();

                string? input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        HandleFullAnalysis();
                        break;
                    case "2":
                        HandleSelectedWordsAnalysis();
                        break;
                    case "3":
                        HandleFullAnalysisNoStops();
                        break;
                    case "4":
                        HandleExportToJson();
                        break;
                    case "0":
                        Console.WriteLine("Exiting application...");
                        return;
                    default:
                        Console.WriteLine("Unknown option. Please enter 1, 2, 3 or 4.");
                        break;
                }
            }
        }

        /// <summary>
        /// Displays the available menu options.
        /// </summary>
        private void DisplayMenu()
        {
            Console.WriteLine("\n=== PDF Analyzer Menu ===");
            Console.WriteLine("1 - Analyze all words in a PDF file");
            Console.WriteLine("2 - Analyze selected words in a PDF");
            Console.WriteLine("3 - Analyze all words (without stop words)");
            Console.WriteLine("4 - Export the result to JSON");
            Console.WriteLine("0 - Exit the application");
            Console.Write("Select an option: ");
        }

        /// <summary>
        /// Handles full word frequency analysis.
        /// </summary>
        private void HandleFullAnalysis()
        {
            Console.Write("Enter full path to the PDF file (or 0 to cancel): ");
            string? filePath = Console.ReadLine();

            if (filePath == "0")
            {
                Console.WriteLine("Operation canceled.");
                return;
            }

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Invalid file path.");
                return;
            }

            try
            {
                parser.ShowWordsByCount(filePath);
                analysisPerformed = parser.WordFrequency.Count > 0;
                lastReport = analysisPerformed ? ReportType.Full : ReportType.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during analysis: {ex.Message}");
                analysisPerformed = false;
                lastReport = ReportType.None;
            }
        }

        /// <summary>
        /// Handles analysis of selected words.
        /// </summary>
        private void HandleSelectedWordsAnalysis()
        {
            Console.Write("Enter full path to the PDF file (or 0 to cancel): ");
            string? filePath = Console.ReadLine();

            if (filePath == "0")
            {
                Console.WriteLine("Operation canceled.");
                return;
            }

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Invalid file path.");
                return;
            }

            Console.Write("Enter comma-separated list of target words: ");
            string? inputWords = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(inputWords))
            {
                Console.WriteLine("No words provided.");
                return;
            }

            var selectedWords = inputWords
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
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
                analysisPerformed = parser.SelectedWordFrequency.Count > 0;
                lastReport = analysisPerformed ? ReportType.Selected : ReportType.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during analysis: {ex.Message}");
                analysisPerformed = false;
                lastReport = ReportType.None;
            }
        }

        /// <summary>
        /// Handles word frequency analysis excluding stop words.
        /// </summary>
        private void HandleFullAnalysisNoStops()
        {
            Console.Write("Enter full path to the PDF file (or 0 to cancel): ");
            string? filePath = Console.ReadLine();

            if (filePath == "0")
            {
                Console.WriteLine("Operation canceled.");
                return;
            }

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                Console.WriteLine("Invalid file path.");
                return;
            }

            try
            {
                parser.ShowWordsByCountNoStopWords(filePath);
                analysisPerformed = parser.WithNoStopWordFrequency.Count > 0;
                lastReport = analysisPerformed ? ReportType.NoStops : ReportType.None;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during analysis: {ex.Message}");
                analysisPerformed = false;
                lastReport = ReportType.None;
            }
        }

        /// <summary>
        /// Handles export of the most recent analysis result to a JSON file.
        /// </summary>
        private void HandleExportToJson()
        {
            if (!analysisPerformed || lastReport == ReportType.None)
            {
                Console.WriteLine("No analysis data available. Run analysis first (1 or 2).");
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
                Console.WriteLine("Invalid file path.");
                return;
            }

            try
            {
                Dictionary<string, int>? dataToExport = lastReport switch
                {
                    ReportType.Full => parser.WordFrequency,
                    ReportType.Selected => parser.SelectedWordFrequency,
                    ReportType.NoStops => parser.WithNoStopWordFrequency,
                    _ => null,
                };

                if (dataToExport == null || dataToExport.Count == 0)
                {
                    Console.WriteLine("No data available to export.");
                    return;
                }

                parser.ExportCountInJson(outputPath, dataToExport);
                Console.WriteLine("Export completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during export: {ex.Message}");
            }
        }
    }
}
