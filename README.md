# PDF File Analyzer

A simple console application to parse and analyze PDF files using the [UglyToad.PdfPig](https://github.com/UglyToad/PdfPig) library.

## ✨ Features

- ✅ Reads a PDF file specified by the user  
- ✅ Extracts words, cleans punctuation, and normalizes text  
- ✅ Counts the frequency of each word (case-insensitive)  
- ✅ Displays a sorted frequency list in the console  
- ✅ Exports word frequency data to a JSON file  
- ✅ Interactive console menu with options to analyze, export, or exit  

## 📄 Example Output

```
Word frequency:
lorem — 42
ipsum — 39
dolor — 35
...

Processing completed.
```

### 📦 Sample JSON Export

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

1. Choose `1` to analyze a PDF and view word frequency in the console.
2. Choose `2` to export the frequency data to a JSON file (saved in the same folder).
3. Choose `0` to exit.

## 📌 Current Status

This is a working prototype focused on word frequency analysis.  
The architecture is designed to be extensible for future features.

## 🛠️ Planned Improvements

- Export to CSV or Excel formats  
- Stop-word filtering and phrase detection  
- Improved error handling and logging  
- GUI or web interface  

---

> Developed as a practical C# project to demonstrate PDF parsing and console app design.

