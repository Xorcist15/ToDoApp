using ToDoApp.Models;

namespace ToDoApp.Storage {
    public interface ITaskStorage {

        /// <summary>
        /// Saves a collection of tasks to specified file path
        /// </summary>
        /// <param name="tasks">The collection of tasks to save</param>
        /// <param name="filePath">The path to the file where the tasks will be saved</param>
        void SaveTasks(IEnumerable<ToDoTask> tasks, string filePath);

        /// <summary>
        /// Loads a collection of tasks from specified file path
        /// </summary>
        /// <param name="filePath">The path to the file from which the tasks will be loaded</param>
        /// <returns>
        /// A collection of tasks
        /// </returns>
        List<ToDoTask> LoadTasks(string filePath);
    }
}
