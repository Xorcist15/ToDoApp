using ToDoApp.Core;
using ToDoApp.Storage;
using ToDoApp.Models;

namespace ToDoApp {
    class Program {
        static void Main(String[] args) {

            var storage = new TextFileTaskStorage();
            var filePath = "/home/yousef/github/ToDoApp/tasks.txt";
            var taskManager = new TaskManager(storage, filePath);

            // show a help message
            if (args.Length > 0) {
                switch (args[0]) {

                    case "--version":
                    case "-v":
                        ShowVersion();
                        break;

                    case "--list-tasks":
                    case "-ls":
                        var tasks = taskManager.GetAllTasks();
                        Console.WriteLine(FormatTasks(tasks));
                        break;
                        
                    case "--delete":
                    case "-d":
                        if (args.Length < 2) {
                            Console.WriteLine("Error, missing parameters");
                            Console.WriteLine("Usage: --delete <task id>");
                            return;
                        }

                        if (int.TryParse(args[1], out int id)) {
                            taskManager.DeleteTask(id);    
                        }
                        else {
                            Console.WriteLine("Error, missing parameters");
                            Console.WriteLine("Usage: --delete <task id>");
                            return;
                        }

                        break;

                    case "--add-task":
                    case "-a":
                        if (args.Length < 2) {
                            Console.WriteLine("Error, missing parameters");
                            Console.WriteLine("Usage: --add-task <description> <date>");
                            return;
                        }

                        string description = args[1];
                        if (string.IsNullOrWhiteSpace(description)) {
                            Console.WriteLine("Error, missing parameters");
                            Console.WriteLine("Usage: --add-task <description> <date>");
                            return;
                        }

                        DateTime dueDate;
                        if (args.Length >= 3) {
                            if (!DateTime.TryParse(args[2], out dueDate)) {
                                Console.WriteLine("Error: Invalid due date format. try <dd-mm-yyyy>");
                            }
                        } 
                        else {
                            dueDate = DateTime.Now.AddDays(7);
                            Console.WriteLine("No date provided, defaulting to 7 days from now");
                            return;
                        }

                        taskManager.AddTask(description, dueDate);
                        break;
                }
            }
        }

        static void ShowVersion() {
            string version = ""; 
            version += "CLI Task Manager - version 1.0.0";
            Console.WriteLine(version);
        }

        static string FormatTasks(IEnumerable<ToDoTask> tasks) {

            if (!tasks.Any()) return "No tasks to show.";

            // headers
            string headerId = "ID";
            string headerDesc = "DESC";
            string headerDate = "DATE";
            string headerStatus = "STATUS";

            // columns' widths
            int colWidthId = headerId.Length;
            int colWidthDesc = headerDesc.Length;
            int colWidthDate = headerDate.Length;
            int colWidthStatus = headerStatus.Length; 


            foreach(var task in tasks) {
                if(task.Id.ToString().Length > colWidthId) {
                    colWidthId = task.Id.ToString().Length;
                }
                if(task.Description.Length > colWidthDesc) {
                    colWidthDesc = task.Description.Length;
                }
                
                string formattedDate = task.DueDate.ToString("dd-mm-yyyy");
                if(formattedDate.Length > colWidthDate) {
                    colWidthDate = formattedDate.Length;
                }

                if(task.Status.Length > colWidthStatus) {
                    colWidthStatus = task.Status.Length;
                }
            }

            int padding = 2;
            colWidthId += padding;
            colWidthDesc += padding;
            colWidthDate += padding;
            colWidthStatus += padding;

            string formattedTasks = "";

            // offsets vars
            int offId = colWidthId - headerId.Length;
            int offDesc = colWidthDesc - headerDesc.Length;
            int offDate = colWidthDate - headerDate.Length;
            int offStatus = colWidthStatus - headerStatus.Length;

            // table width
            int totalWidth = colWidthId + colWidthDesc + colWidthDate + colWidthStatus;

            formattedTasks += "+" + new string('-', totalWidth + 7) + "+\n"; 

            formattedTasks +=
                $"| {headerId}" + new string(' ', offId) +
                $"| {headerDesc}" + new string(' ', offDesc) +
                $"| {headerDate}" + new string(' ', offDate) +
                $"| {headerStatus}" + new string(' ', offStatus) + "|\n";

            formattedTasks += "+" + new string('-', totalWidth + 7) + "+\n"; 

            foreach(var task in tasks) {

                string formattedDate = task.DueDate.ToString("dd/MM/yyyy");

                // offsets vars reused here
                offId = colWidthId - task.Id.ToString().Length;
                offDesc = colWidthDesc - task.Description.Length;
                offDate = colWidthDate - formattedDate.Length;
                offStatus = colWidthStatus - task.Status.Length;

                formattedTasks +=
                    $"| {task.Id}" + new string(' ', offId) +
                    $"| {task.Description}" + new string(' ', offDesc) +
                    $"| {formattedDate}" + new string(' ', offDate) +
                    $"| {task.Status}" + new string(' ', offStatus) + "|\n";
            }

            formattedTasks += "+" + new string('-', totalWidth + 7) + "+\n"; 

            return formattedTasks;
        }
    }
}
