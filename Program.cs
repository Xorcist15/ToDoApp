//using ToDoApp.Core;

namespace ToDoApp {
    class Program {
        static void Main(String[] args) {

            // show a help message
            if (args.Length == 0 || args[0] == "--help") {
                showHelp(); return;
            }
        }

        static void showHelp () {
            string help = @"CLI Task Manager - version 1.0.0
A simple command-line tool to manage your daily tasks.

Usage:
    ????

Commands
    --help

    --show              shows all tasks
    --date <yyyy-mm-dd> filters tasks by date

    --add <desc> <date> add new task, defaults to default status

    --del  <id>         deletes a task by id
    "; 
            Console.WriteLine(help);
        }
    }
}
