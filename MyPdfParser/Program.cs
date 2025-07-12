namespace MyPdfParser
{
    /// <summary>
    /// Application entry point.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Starts the console menu interface.
        /// </summary>
        static void Main(string[] args)
        {
            var menu = new ConsoleMenu();
            menu.Run();
        }
    }
}
