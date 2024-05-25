using System;
using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create clients with different roles
            Client manager = new Client
            {
                Name = "Manager1",
                State = new ManagerState()
            };

            Client teamLead = new Client
            {
                Name = "TeamLead1",
                State = new TeamLeadState()
            };

            Client developer = new Client
            {
                Name = "Developer1",
                State = new DeveloperState()
            };

            // Decorate the clients with logging functionality
            IUser loggingManager = new LoggingUserDecorator(manager);
            IUser loggingTeamLead = new LoggingUserDecorator(teamLead);
            IUser loggingDeveloper = new LoggingUserDecorator(developer);

            // Create some tasks
            Task task1 = new Task("Task1", "Manager1");
            Task task2 = new Task("Task2", "TeamLead1");
            Task task3 = new Task("Task3", "Manager1");

            // Manager adding tasks
            loggingManager.AddTask(task1);
            loggingManager.AddTask(task2);

            // TeamLead adding a task
            loggingTeamLead.AddTask(task3);

            // Viewing tasks
            Console.WriteLine("Manager's Tasks:");
            foreach (var task in loggingManager.ViewTasks())
            {
                Console.WriteLine($"- {task.NumeTask}");
            }

            Console.WriteLine("TeamLead's Tasks:");
            foreach (var task in loggingTeamLead.ViewTasks())
            {
                Console.WriteLine($"- {task.NumeTask}");
            }

            // Editing a task
            loggingManager.EditTask("Task1", "UpdatedTask1", "Updated description", 5.0);

            // Viewing tasks after modification
            Console.WriteLine("Manager's Tasks after modification:");
            foreach (var task in loggingManager.ViewTasks())
            {
                Console.WriteLine($"- {task.NumeTask}: {task.DescriereTask}");
            }

            // Removing a task
            loggingManager.RemoveTask("Task2");

            // Viewing tasks after removal
            Console.WriteLine("Manager's Tasks after removal:");
            foreach (var task in loggingManager.ViewTasks())
            {
                Console.WriteLine($"- {task.NumeTask}");
            }
        }
    }
}
