using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT
{
    public class Client : IUser
    {
        public string Name { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
        public IUserState State { get; set; }

        public void AddTask(Task task)
        {
            State.AddTask(this, task);
        }

        public void ModifyTask(string taskName, Task updatedTask)
        {
            State.ModifyTask(this, taskName, updatedTask);
        }

        public void RemoveTask(string taskName)
        {
            State.RemoveTask(this, taskName);
        }

        public List<Task> ViewTasks()
        {
            return State.ViewTasks(this);
        }

        public void EditTask(string taskName, string newName, string newDescription, DateTime newDeadline)
        {
            var updatedTask = new Task { Name = newName, Description = newDescription, Deadline = newDeadline };
            State.ModifyTask(this, taskName, updatedTask);
        }
    }
}
