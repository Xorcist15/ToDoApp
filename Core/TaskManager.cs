using ToDoApp.Models;
using ToDoApp.Storage;

namespace ToDoApp.Core {
    public class TaskManager {
        private List<ToDoTask> _tasks;
        private int _nextId = 1;
        private ITaskStorage _storage;
        private string _storageFilePath;

        /// <summary>
        /// Init new instance of TaskManager class
        /// Depends on ITaskStorage implementation to handle persistence
        /// </summary>
        /// <param name="storage">Storage mechanism to use for loading and saving tasks</param>
        /// <param name="storageFilePath">File path for task storage</param>
        public TaskManager (ITaskStorage storage, string storageFilePath) {
            _storage = storage ?? 
                throw new ArgumentNullException(nameof(storage), "Task storage mechanism can't be null");
            _storageFilePath = storageFilePath ?? 
                throw new ArgumentNullException(nameof(storageFilePath), "Storage file path can't be null or empty");

            try {
                _tasks = _storage.LoadTasks(_storageFilePath);
                if (_tasks.Any()) _nextId = _tasks.Max(t => t.Id) + 1;
            }
            catch (Exception ex) {
                Console.WriteLine($"Warning: Could not load tasks on startup. Starting with an empty task list. Error: {ex.Message}");
                _tasks = new List<ToDoTask>();
            }
        }
    }
}
