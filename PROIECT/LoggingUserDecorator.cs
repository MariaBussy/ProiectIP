using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT
{
    // Implementare pentru decorator de logging
    public class LoggingUserDecorator : UserDecorator
    {
        public LoggingUserDecorator(IUser user) : base(user)
        {
        }

        public override void AddTask(Task task)
        {
            Console.WriteLine($"Logging: Adding task '{task.Name}' to user '{_user.Name}'");
            base.AddTask(task);
        }

        public override void ModifyTask(string taskName, Task updatedTask)
        {
            Console.WriteLine($"Logging: Modifying task '{taskName}' for user '{_user.Name}'");
            base.ModifyTask(taskName, updatedTask);
        }

        public override void RemoveTask(string taskName)
        {
            Console.WriteLine($"Logging: Removing task '{taskName}' from user '{_user.Name}'");
            base.RemoveTask(taskName);
        }

        public override List<Task> ViewTasks()
        {
            Console.WriteLine($"Logging: Viewing tasks for user '{_user.Name}'");
            return base.ViewTasks();
        }

        public override void EditTask(string taskName, string newName, string newDescription, DateTime newDeadline)
        {
            Console.WriteLine($"Logging: Editing task '{taskName}' for user '{_user.Name}' to new name '{newName}', new description '{newDescription}', new deadline '{newDeadline}'");
            base.EditTask(taskName, newName, newDescription, newDeadline);
        }
    }

}
