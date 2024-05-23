using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT
{
    // Implementare pentru Developer
    public class DeveloperState : IUserState
    {
        public void AddTask(Client user, Task task)
        {
            // Developers cannot add tasks
        }

        public void ModifyTask(Client user, string taskName, Task updatedTask)
        {
            // Developers cannot modify tasks
        }

        public void RemoveTask(Client user, string taskName)
        {
            // Developers cannot remove tasks
        }

        public List<Task> ViewTasks(Client user)
        {
            return user.Tasks;
        }
    }

}
