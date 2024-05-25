using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    public interface IUserState
    {
        void AddTask(Client client, Task task);
        void ModifyTask(Client client, string taskName, Task updatedTask);
        void RemoveTask(Client client, string taskName);
        List<Task> ViewTasks(Client client);
    }
}
