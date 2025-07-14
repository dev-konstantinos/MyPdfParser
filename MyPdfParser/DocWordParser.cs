using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace MyPdfParser
{
    /// <summary>
    /// Parses a PDF document and analyzes word frequency, with options for filtering.
    /// </summary>
    internal class DocWordParser
    {
        public List<string> Words { get; private set; } = new List<string>();

        public Dictionary<string, int> WordFrequency { get; private set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, int> SelectedWordFrequency { get; private set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, int> WithNoStopWordFrequency { get; private set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Multilingual stop-word list (English, Russian, German).
        /// </summary>
        private readonly HashSet<string> stopWords = new(StringComparer.OrdinalIgnoreCase)
        {
            // English
            "the", "and", "or", "in", "on", "at", "of", "to", "a", "an", "is", "are", "was", "were", "by", "for", "with",
            "as", "from", "that", "this", "it", "be", "but", "not", "have", "has", "had", "i", "you", "he", "she", "we",
            "they", "them", "his", "her", "its", "their", "which",

            // Russian
            "и", "в", "во", "не", "что", "он", "на", "я", "с", "со", "как", "а", "то", "все", "она", "так", "его",
            "но", "да", "ты", "к", "у", "же", "вы", "за", "бы", "по", "только", "ее", "мне", "было", "вот", "от", "меня",
            "еще", "нет", "о", "из", "ему", "теперь", "когда", "даже", "ну", "вдруг", "ли", "если", "уже", "или", "ни",
            "быть", "был", "него", "до", "вас", "нибудь", "опять", "уж", "вам", "ведь", "там", "потом", "себя", "ничего",
            "ей", "может", "они", "тут", "где", "есть", "надо", "ней", "для", "мы", "тебя", "их", "чем", "была", "сам",
            "чтоб", "без", "будто", "чего", "раз", "тоже", "себе", "под", "будет", "ж", "тогда", "кто", "этот",

            // German
            "der", "die", "das", "und", "in", "den", "von", "zu", "mit", "auf", "für", "ist", "im", "dem", "nicht", "ein",
            "eine", "als", "auch", "an", "es", "am", "aus", "er", "sie", "nach", "bei", "über", "wir", "was", "so",
            "ich", "du", "haben", "hat", "dass", "sein", "noch", "wie", "man", "nur", "wenn", "kann", "doch", "schon",
            "ja", "oder", "aber", "mehr", "vor", "zur", "bis", "unter", "weil", "wird"
        };

        /// <summary>
        /// Extracts raw words from a PDF file.
        /// </summary>
        private void ParsePdfWords(string filePath)
        {
            try
            {
                using var document = PdfDocument.Open(filePath);

                foreach (Page page in document.GetPages())
                {
                    foreach (var word in page.GetWords())
                    {
                        Words.Add(word.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while opening PDF file: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Builds a case-insensitive word frequency dictionary from the PDF content.
        /// </summary>
        private void BuildWordFrequency(string filePath)
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
                throw;
            }

            if (Words.Count == 0)
            {
                Console.WriteLine("No words found in the document.");
                return;
            }

            foreach (var word in Words)
            {
                if (string.IsNullOrWhiteSpace(word))
                    continue;

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
        }

        /// <summary>
        /// Shows the frequency of all words in descending order.
        /// </summary>
        public void ShowWordsByCount(string filePath)
        {
            try
            {
                BuildWordFrequency(filePath);
            }
            catch
            {
                return;
            }

            Console.WriteLine("\nWord frequency:");

            foreach (var kvp in WordFrequency)
            {
                Console.WriteLine($"{kvp.Key} — {kvp.Value}");
            }

            Console.WriteLine("\nProcessing completed.");
        }

        /// <summary>
        /// Shows frequency for user-selected words only.
        /// </summary>
        public void ShowSelectedWordFrequencies(string filePath, List<string> selectedWords)
        {
            try
            {
                BuildWordFrequency(filePath);
            }
            catch
            {
                return;
            }

            var normalizedSelection = selectedWords
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .Select(w => Regex.Replace(w, @"[^\p{L}\p{N}]", "").ToLowerInvariant())
                .ToHashSet();

            SelectedWordFrequency = WordFrequency
                .Where(kvp => normalizedSelection.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            Console.WriteLine("\nSelected words frequency:");

            foreach (var kvp in SelectedWordFrequency)
            {
                Console.WriteLine($"{kvp.Key} — {kvp.Value}");
            }

            Console.WriteLine("\nProcessing completed.");
        }

        /// <summary>
        /// Shows word frequencies excluding stop words.
        /// </summary>
        public void ShowWordsByCountNoStopWords(string filePath)
        {
            try
            {
                BuildWordFrequency(filePath);
            }
            catch
            {
                return;
            }

            WithNoStopWordFrequency = WordFrequency
                .Where(kvp => !stopWords.Contains(kvp.Key))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            Console.WriteLine("\nWord frequency (excluding stop words):");

            foreach (var kvp in WithNoStopWordFrequency.OrderByDescending(k => k.Value))
            {
                Console.WriteLine($"{kvp.Key} — {kvp.Value}");
            }

            Console.WriteLine("\nProcessing completed.");
        }

        /// <summary>
        /// Exports any frequency dictionary to a JSON file.
        /// </summary>
        public void ExportCountInJson(string outputPath, Dictionary<string, int> dict)
        {
            if (dict == null || dict.Count == 0)
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

                string json = JsonSerializer.Serialize(dict, options);
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
