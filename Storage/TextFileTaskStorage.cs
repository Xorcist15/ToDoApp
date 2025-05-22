using ToDoApp.Models;

namespace ToDoApp.Storage {
    public class TextFileTaskStorage : ITaskStorage {

        private const char Delimiter = '|';

        /// <summary>
        /// Saves a collection of tasks to the specified file path in text format
        /// Each task is saved as single line, props are seperated by a delimiter
        /// </summary>
        /// <param name="tasks">The collection of tasks to save</param>
        /// <param name="filePath">The path to the file where the tasks should be saved</param>
        public void SaveTasks(IEnumerable<ToDoTask> tasks, string filePath) {
            // input validation
            if (tasks == null) {
                string msg = "The collection of tasks to save can't be null";
                throw new ArgumentNullException(nameof(tasks), msg);
            } 
            if (string.IsNullOrWhiteSpace(filePath)) {
                string msg = "The file path can't be null or empty";
                throw new ArgumentException(msg, nameof(filePath));
            }

            // serialization
            var linesToWrite = new List<string>();
            foreach (var task in tasks) {
                linesToWrite.Add(
                    $"{task.Id}{Delimiter}" +
                    $"{task.Description.Replace(Delimiter, ' ')}{Delimiter}" +
                    $"{task.DueDate.ToBinary()}{Delimiter}" +
                    $"{task.Status}"
                );
            }

            // file writing
            try {
                File.WriteAllLines(filePath, linesToWrite);
                Console.WriteLine($"Successfully saved {linesToWrite.Count} tasks to '{filePath}'.");
            }
            // error handling
            catch (IOException ex) {
                Console.WriteLine($"Error saving tasks to '{filePath}': {ex.Message}");
            }
            catch (UnauthorizedAccessException ex) {
                Console.WriteLine($"Permission denied when saving to '{filePath}': {ex.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"An unexpected error occured while saving tasks: {ex.Message}");
            }
        }


        /// <summary>
        /// Loads a collection of tasks from specified file path
        /// each line is expected to represent a task, with properties
        /// seperated by a specified delimiter
        /// </summary>
        /// <param name="filePath">The path to the file from which the tasks should be loaded</param>
        /// <returns>
        /// A list of task objects loaded from file
        /// returns an empty list if file does not exist
        /// </returns>
        public List<ToDoTask> LoadTasks(string filePath) {
            var loadedTasks = new List<ToDoTask>();

            // input validation
            if (string.IsNullOrWhiteSpace(filePath)) {
                string msg = "The file path can't be null or empty.";
                throw new ArgumentNullException(msg, nameof(filePath));
            }

            // pre-check file existence
            if (!File.Exists(filePath)) {
                Console.WriteLine($"No task file found at {filePath}. Starting with an empty task list");
                return loadedTasks;
            }

            // deserialization
            try {
                string[] lines = File.ReadAllLines(filePath);

                foreach (string line in lines) {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(Delimiter);

                    // data validation
                    if (parts.Length == 4) {
                        if (int.TryParse(parts[0], out int id) && long.TryParse(parts[2], out long dueDateBinary)) {
                            try {
                                DateTime dueDate = DateTime.FromBinary(dueDateBinary);
                                string description = parts[1];
                                string status = parts[3];

                                var task = new ToDoTask(id, description, dueDate);
                                task.Status = status;

                                loadedTasks.Add(task);
                            }
                            catch (ArgumentException ex) {
                                Console.WriteLine($"Skipping malformed date in line '{line}'. Error: {ex.Message}");
                            }
                        }
                        else {
                            Console.WriteLine($"Skipping malformed ID or DueDate in line: '{line}'.");
                        }
                    }
                    else {
                        Console.WriteLine($"Skipping malformed line (incorrect number of parts [{parts.Length}]): '{line}'.");
                    }
                }
            }
            // error handling
            catch (IOException ex) {
                Console.WriteLine($"Error loading tasks from '${filePath}': {ex.Message}");
            }
            catch (UnauthorizedAccessException ex) {
                Console.WriteLine($"Permission denied when loading from '{filePath}': {ex.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"An unexpected error occured while loading tasks from '{filePath}': {ex.Message}");
            }

            return loadedTasks;
        }
    }
}
