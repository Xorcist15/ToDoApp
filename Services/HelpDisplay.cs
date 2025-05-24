namespace ToDoApp.Services
{
    public class HelpDisplay
    {
        private readonly string _helpFilePath;

        /// <summary>
        /// Initializes a new instance of the HelpDisplay class.
        /// </summary>
        /// <param name="helpFilePath">The path to the help file relative to the executable directory.</param>
        public HelpDisplay(string helpFilePath)
        {
            // Basic validation for the file path
            _helpFilePath = helpFilePath ?? throw new ArgumentNullException(nameof(helpFilePath));
            if (string.IsNullOrWhiteSpace(_helpFilePath))
            {
                throw new ArgumentException("Help file path cannot be null or empty.", nameof(helpFilePath));
            }
        }

        /// <summary>
        /// Reads and displays the content of the help file to the console.
        /// </summary>
        public void ShowHelp()
        {
            try
            {
                // Combine the base directory of the running application with the relative path
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _helpFilePath);

                if (!File.Exists(fullPath))
                {
                    string msg = $"Error: Help file not found at '{fullPath}'. " +
                        "Please ensure 'us-help.txt' is correctly " + 
                        "configured and copied to the output directory.";
                    Console.WriteLine(msg);
                    return;
                }

                string helpContent = File.ReadAllText(fullPath);
                Console.WriteLine(helpContent);
            }
            catch (Exception ex) // Catch any other exceptions during file read
            {
                Console.WriteLine($"An unexpected error occurred while trying to display help: {ex.Message}");
            }
        }
    }
}
