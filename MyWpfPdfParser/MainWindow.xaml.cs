using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace MyWpfPdfParser
{
    public partial class MainWindow : Window
    {
        private readonly DocWordParser parser = new();

        private string CurrentFilePath => SelectedFileTextBox.Text;

        private Dictionary<string, int> lastResult = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectPdfButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf",
                Title = "Select a PDF file"
            };

            if (dlg.ShowDialog() == true)
            {
                SelectedFileTextBox.Text = dlg.FileName;
            }
        }

        private void AnalyzeAll_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(CurrentFilePath)) return;

            parser.ShowWordsByCount(CurrentFilePath);
            lastResult = parser.WordFrequency;
            WordFrequencyGrid.ItemsSource = lastResult.ToList();
        }

        private void AnalyzeNoStopWords_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(CurrentFilePath)) return;

            parser.ShowWordsByCountNoStopWords(CurrentFilePath);
            lastResult = parser.WithNoStopWordFrequency;
            WordFrequencyGrid.ItemsSource = lastResult.ToList();
        }

        private void AnalyzeSelectedWords_Click(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(CurrentFilePath)) return;

            var words = SelectedWordsTextBox.Text
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(w => w.Trim())
                .ToList();

            parser.ShowSelectedWordFrequencies(CurrentFilePath, words);
            lastResult = parser.SelectedWordFrequency;
            WordFrequencyGrid.ItemsSource = lastResult.ToList();
        }

        private void ExportJson_Click(object sender, RoutedEventArgs e)
        {
            if (lastResult == null || lastResult.Count == 0)
            {
                MessageBox.Show("No data to export", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "JSON Files (*.json)|*.json",
                Title = "Save as JSON"
            };

            if (dlg.ShowDialog() == true)
            {
                parser.ExportCountInJson(dlg.FileName, lastResult);
                MessageBox.Show("Export completed", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
