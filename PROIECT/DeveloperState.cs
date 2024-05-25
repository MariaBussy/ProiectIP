using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    public class DeveloperState : IUserState
    {
        public void AddTask(Client client, Task task)
        {
            // Developers cannot add tasks
        }

        public void ModifyTask(Client client, string taskName, Task updatedTask)
        {
            // Developers cannot modify tasks
        }

        public void RemoveTask(Client client, string taskName)
        {
            // Developers cannot remove tasks
        }

        public List<Task> ViewTasks(Client client)
        {
            return client.Tasks;
        }
    }
}
