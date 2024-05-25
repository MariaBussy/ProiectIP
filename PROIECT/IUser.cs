using System;
using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    public interface IUser
    {
        string Name { get; set; }
        List<Task> Tasks { get; set; }
        IUserState State { get; set; }

        void AddTask(Task task);
        void ModifyTask(string taskName, Task updatedTask);
        void RemoveTask(string taskName);
        List<Task> ViewTasks();
        void EditTask(string taskName, string newName, string newDescription, double newLoggedHours);
    }
}
