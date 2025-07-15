using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace MyWpfPdfParser
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
            "they", "them", "his", "her", "its", "their", "which", "who", "whom", "what", "where", "when", "why", "how",

            // Russian
            "и", "в", "во", "не", "что", "он", "на", "я", "с", "со", "как", "а", "то", "все", "она", "так", "его",
            "но", "да", "ты", "к", "у", "же", "вы", "за", "бы", "по", "только", "ее", "мне", "было", "вот", "от", "меня",
            "еще", "нет", "о", "из", "ему", "теперь", "когда", "даже", "ну", "вдруг", "ли", "если", "уже", "или", "ни",
            "быть", "был", "него", "до", "вас", "нибудь", "опять", "уж", "вам", "ведь", "там", "потом", "себя", "ничего",
            "ей", "может", "они", "тут", "где", "есть", "надо", "ней", "для", "мы", "тебя", "их", "чем", "была", "сам",
            "чтоб", "без", "будто", "чего", "раз", "тоже", "себе", "под", "будет", "ж", "тогда", "кто", "этот",

            // German
            "der", "die", "das", "und", "in", "den", "von", "zu", "mit", "auf", "für", "ist", "im", "dem", "nicht", "ein",
            "eine", "als", "auch", "an", "es", "am", "aus", "er", "sie", "nach", "bei", "über", "wir", "was", "so", "des", "dass",
            "ich", "du", "haben", "hat", "dass", "sein", "noch", "wie", "man", "nur", "wenn", "kann", "doch", "schon",
            "ja", "oder", "aber", "mehr", "vor", "zur", "bis", "unter", "weil", "wird", "muss", "sind", "ihre", "einen", "einem", "einer", "dieser", "jenen", "diese", "dieses", "solche", "solcher", "solches", "solchen", "solcherweise", "dort", "hier", "wo", "wann", "warum", "wer", "was", "wie", "welche", "welcher", "welches"
        };

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
                Logger.Log($"Error while opening PDF file: {ex.Message}");
                throw;
            }
        }

        private void BuildWordFrequency(string filePath)
        {
            Words.Clear();
            WordFrequency.Clear();

            Logger.Log($"Processing file: {filePath}");

            try
            {
                ParsePdfWords(filePath);
            }
            catch
            {
                Logger.Log("Processing failed due to an error.");
                throw;
            }

            if (Words.Count == 0)
            {
                Logger.Log("No words found in the document.");
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

            Logger.Log("Word frequency:");

            foreach (var kvp in WordFrequency)
            {
                Logger.Log($"{kvp.Key} — {kvp.Value}");
            }

            Logger.Log("Processing completed.");
        }

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

            Logger.Log("Selected words frequency:");

            foreach (var kvp in SelectedWordFrequency)
            {
                Logger.Log($"{kvp.Key} — {kvp.Value}");
            }

            Logger.Log("Processing completed.");
        }

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

            Logger.Log("Word frequency (excluding stop words):");

            foreach (var kvp in WithNoStopWordFrequency.OrderByDescending(k => k.Value))
            {
                Logger.Log($"{kvp.Key} — {kvp.Value}");
            }

            Logger.Log("Processing completed.");
        }

        public void ExportCountInJson(string outputPath, Dictionary<string, int> dict)
        {
            if (dict == null || dict.Count == 0)
            {
                Logger.Log("No word frequency data to export.");
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

                Logger.Log($"JSON file with word frequencies created: {outputPath}");
            }
            catch (Exception ex)
            {
                Logger.Log($"Error during JSON export: {ex.Message}");
            }
        }
    }
}
