using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWpfPdfParser
{
    internal class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log.txt");

        public static void Log(string message)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                File.AppendAllText(LogFilePath, $"[{timestamp}] {message}{Environment.NewLine}");
            }
            catch (Exception ex)
            {
                try
                {
                    string fallbackPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logger_errors.txt");
                    File.AppendAllText(fallbackPath, $"[{DateTime.Now}] Logging error: {ex.Message}{Environment.NewLine}");
                }
                catch
                {
                    // Doing nothing here, as we cannot log the error
                }

            }
        }
    }
}
