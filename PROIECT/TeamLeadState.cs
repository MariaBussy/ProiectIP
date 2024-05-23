using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT
{
    // Implementare pentru TeamLead
    public class TeamLeadState : IUserState
    {
        public void AddTask(Client user, Task task)
        {
            user.Tasks.Add(task);
        }

        public void ModifyTask(Client user, string taskName, Task updatedTask)
        {
            var task = user.Tasks.Find(t => t.Name == taskName);
            if (task != null)
            {
                task.Name = updatedTask.Name;
                task.Description = updatedTask.Description;
                task.Deadline = updatedTask.Deadline;
            }
        }

        public void RemoveTask(Client user, string taskName)
        {
            user.Tasks.RemoveAll(t => t.Name == taskName);
        }

        public List<Task> ViewTasks(Client user)
        {
            return user.Tasks;
        }
    }

}
