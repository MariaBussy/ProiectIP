using System;
using System.Collections.Generic;
using TaskLibrary;

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

        public void EditTask(string taskName, string newName, string newDescription, double newLoggedHours)
        {
            var updatedTask = new Task(newName, this.Name)
            {
                DescriereTask = newDescription,
                OreLogate = newLoggedHours
            };
            State.ModifyTask(this, taskName, updatedTask);
        }
    }
}
