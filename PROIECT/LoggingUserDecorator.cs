using System;
using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    public class LoggingUserDecorator : UserDecorator
    {
        public LoggingUserDecorator(IUser user) : base(user) { }

        public override void AddTask(Task task)
        {
            Console.WriteLine($"Adding task '{task.NumeTask}' for user '{Name}'");
            base.AddTask(task);
        }

        public override void ModifyTask(string taskName, Task updatedTask)
        {
            Console.WriteLine($"Modifying task '{taskName}' for user '{Name}'");
            base.ModifyTask(taskName, updatedTask);
        }

        public override void RemoveTask(string taskName)
        {
            Console.WriteLine($"Removing task '{taskName}' for user '{Name}'");
            base.RemoveTask(taskName);
        }

        public override List<Task> ViewTasks()
        {
            Console.WriteLine($"Viewing tasks for user '{Name}'");
            return base.ViewTasks();
        }

        public override void EditTask(string taskName, string newName, string newDescription, double newLoggedHours)
        {
            Console.WriteLine($"Editing task '{taskName}' for user '{Name}'");
            base.EditTask(taskName, newName, newDescription, newLoggedHours);
        }
    }
}
