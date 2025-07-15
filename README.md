# PDF File Analyzer

A simple console / WPF application to parse and analyze PDF files using the [UglyToad.PdfPig](https://github.com/UglyToad/PdfPig) library.

## ✨ Features

- ✅ Reads a PDF file specified by the user  
- ✅ Extracts words, cleans punctuation, and normalizes text  
- ✅ Counts the frequency of each word (case-insensitive)  
- ✅ Displays a **sorted frequency list** in the console  
- ✅ Analyzes word frequencies with or without a **stop-word list**
- ✅ Analyzes **only selected words** specified by the user  
- ✅ Exports **selected word frequencies** to JSON report
- ✅ Interactive console menu with options to analyze & export

## 📄 Example Console Output

```
Word frequency:
lorem — 42
ipsum — 39
dolor — 35
...

Processing completed.
```

### 📦 JSON Export Example

```json
{
  "lorem": 42,
  "ipsum": 39,
  "dolor": 35
}
```

## 🔧 Requirements

- [.NET 6.0 SDK or later](https://dotnet.microsoft.com/en-us/download)
- [UglyToad.PdfPig](https://www.nuget.org/packages/UglyToad.PdfPig)

## 🚀 Usage

```bash
dotnet run
```

### Menu Options

1. Analyze and show all word frequencies in a PDF file
2. Analyze and show only selected words in a PDF file
3. Analyze word frequencies without predefined stop words
4. Export the latest result to JSON  
0. Exit the application

## 📌 Current Status

This is a working prototype focused on word frequency analysis.  
The codebase is modular and easily extensible for future features.

## 🛠️ Planned Improvements

- Export to CSV or Excel formats  
- Improved error handling and logging

---

> Developed as a practical C# project to demonstrate PDF parsing, user interaction, and modular architecture.
