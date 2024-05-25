using System;
using System.Collections.Generic;
using System.Linq;
using TaskLibrary;

namespace PROIECT
{
    public class ManagerState : IUserState
    {
        public void AddTask(Client client, Task task)
        {
            client.Tasks.Add(task);
        }

        public void ModifyTask(Client client, string taskName, Task updatedTask)
        {
            var task = client.Tasks.FirstOrDefault(t => t.NumeTask == taskName);
            if (task != null)
            {
                task.NumeTask = updatedTask.NumeTask;
                task.DescriereTask = updatedTask.DescriereTask;
                task.OreLogate = updatedTask.OreLogate;
            }
        }

        public void RemoveTask(Client client, string taskName)
        {
            var task = client.Tasks.FirstOrDefault(t => t.NumeTask == taskName);
            if (task != null)
            {
                client.Tasks.Remove(task);
            }
        }

        public List<Task> ViewTasks(Client client)
        {
            return client.Tasks;
        }
    }
}
