using System;
using System.IO;

namespace MyPdfParser
{
    /// <summary>
    /// Represents an interactive console menu for PDF word frequency analysis.
    /// </summary>
    internal class ConsoleMenu
    {
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

                    default:
                        Console.WriteLine("Unknown option. Please enter 1 or 0.");
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
            Console.WriteLine("1 - Count words in PDF");
            Console.WriteLine("0 - Exit");
            Console.Write("Select an option: ");
        }

        /// <summary>
        /// Prompts user for a PDF file path and runs word frequency analysis.
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
                var parser = new DocWordParser();
                parser.ShowWordsByCount(filePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while processing the file: {ex.Message}");
            }
        }
    }
}

