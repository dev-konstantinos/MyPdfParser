using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace MyPdfParser
{
    /// <summary>
    /// Parses words from a PDF file and calculates their frequency.
    /// </summary>
    internal class DocWordParser
    {
        /// <summary>
        /// List of all raw words extracted from the PDF.
        /// </summary>
        public List<string> Words { get; private set; } = new List<string>();

        /// <summary>
        /// Dictionary containing word frequency counts (case-insensitive).
        /// </summary>
        public Dictionary<string, int> WordFrequency { get; private set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Extracts words from the given PDF file and fills the Words list.
        /// </summary>
        /// <param name="filePath">Full path to the PDF file.</param>
        private void ParsePdfWords(string filePath)
        {
            try
            {
                using (PdfDocument document = PdfDocument.Open(filePath))
                {
                    foreach (Page page in document.GetPages())
                    {
                        var wordsOnPage = page.GetWords();

                        foreach (var word in wordsOnPage)
                        {
                            Words.Add(word.Text);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while opening PDF file: {ex.Message}");
                throw; // Re-throwing the exception to signal failure
            }
        }

        /// <summary>
        /// Processes the specified PDF file, calculates word frequency,
        /// and outputs the result to the console.
        /// </summary>
        /// <param name="filePath">Path to the PDF file.</param>
        public void ShowWordsByCount(string filePath)
        {
            Words.Clear();
            WordFrequency.Clear();

            Console.WriteLine($"Processing file: {filePath}");

            try
            {
                ParsePdfWords(filePath);
            }
            catch
            {
                Console.WriteLine("Processing failed due to an error.");
                return;
            }

            if (Words.Count == 0)
            {
                Console.WriteLine("No words found in the document.");
                return;
            }

            Console.WriteLine("\nWord frequency:");

            foreach (var word in Words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

                // Remove punctuation and normalize case to count words correctly
                var cleanedWord = Regex.Replace(word, @"[^\p{L}\p{N}]", "").ToLowerInvariant();

                if (string.IsNullOrWhiteSpace(cleanedWord))
                    continue;

                if (WordFrequency.ContainsKey(cleanedWord))
                    WordFrequency[cleanedWord]++;
                else
                    WordFrequency[cleanedWord] = 1;
            }

            WordFrequency = WordFrequency
                .OrderByDescending(kvp => kvp.Value)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (var kvp in WordFrequency)
            {
                Console.WriteLine($"{kvp.Key} — {kvp.Value}");
            }

            Console.WriteLine("\nProcessing completed.");
        }
        
        /// <summary>
        /// Exports the current word frequency dictionary to a JSON file.
        /// Assumes that the dictionary has already been sorted externally (e.g., in ShowWordsByCount).
        /// </summary>
        /// <param name="outputPath">The full file path where the JSON output should be saved.</param>
        public void ExportCountInJson(string outputPath)
        {
            if (WordFrequency == null || WordFrequency.Count == 0)
            {
                Console.WriteLine("No word frequency data to export.");
                return;
            }

            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(WordFrequency, options);

                File.WriteAllText(outputPath, json);

                Console.WriteLine($"JSON file with word frequencies created: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during JSON export: {ex.Message}");
            }
        }
    }
}
