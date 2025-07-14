# PDF File Analyzer

A simple console application to parse and analyze PDF files using the [UglyToad.PdfPig](https://github.com/UglyToad/PdfPig) library.

## ✨ Features

- ✅ Reads a PDF file specified by the user  
- ✅ Extracts words, cleans punctuation, and normalizes text  
- ✅ Counts the frequency of each word (case-insensitive)  
- ✅ Displays a sorted frequency list in the console  
- ✅ Exports **all word frequencies** to a JSON file  
- ✅ Analyzes **only selected words** specified by the user  
- ✅ Exports **selected word frequencies** to JSON  
- ✅ Interactive console menu with options to analyze, export, or exit  

## 📄 Example Console Output

```
Word frequency:
lorem — 42
ipsum — 39
dolor — 35
...

Processing completed.
```

### 🎯 Selected Words Output

```
Selected words frequency:
data — 21
test — 10
model — 7
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

### 📦 Selected JSON Export Example

```json
{
  "data": 21,
  "test": 10,
  "model": 7
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

1. Analyze a PDF and view word frequency in the console  
2. Export all word frequencies to a JSON file  
3. Analyze only selected words (comma-separated input)  
4. Export selected word frequencies to a JSON file  
0. Exit the application  

## 📌 Current Status

This is a working prototype focused on word frequency analysis.  
The codebase is modular and extensible for future features.

## 🛠️ Planned Improvements

- Export to CSV or Excel formats  
- Stop-word filtering and phrase detection  
- Improved error handling and logging  
- GUI or web interface  
- Unit tests for core logic  

---

> Developed as a practical C# project to demonstrate PDF parsing, user interaction, and modular architecture.
