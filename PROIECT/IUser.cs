using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT
{
    // Interfața IUser care definește funcționalitățile de bază ale unui utilizator
    public interface IUser
    {
        string Name { get; set; }
        List<Task> Tasks { get; set; }
        IUserState State { get; set; }

        void AddTask(Task task);
        void ModifyTask(string taskName, Task updatedTask);
        void RemoveTask(string taskName);
        List<Task> ViewTasks();
        void EditTask(string taskName, string newName, string newDescription, DateTime newDeadline);
    }

}
