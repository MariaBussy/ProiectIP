using System;
using System.Collections.Generic;
using System.Linq;
using TaskLibrary;

namespace PROIECT
{
    /// <summary>
    /// Clasa TeamLeadState implementează interfața IUserState și definește comportamentul specific pentru utilizatorii de tip TeamLead.
    /// </summary>
    public class TeamLeadState : IUserState
    {
        /// <summary>
        /// Adaugă un task la lista de task-uri a clientului specificat.
        /// </summary>
        /// <param name="client">Clientul pentru care se adaugă task-ul.</param>
        /// <param name="task">Task-ul de adăugat.</param>
        public void AddTask(Client client, Task task)
        {
            client.Tasks.Add(task);
        }

        /// <summary>
        /// Modifică un task existent în lista de task-uri a clientului specificat.
        /// </summary>
        /// <param name="client">Clientul pentru care se modifică task-ul.</param>
        /// <param name="taskName">Numele task-ului de modificat.</param>
        /// <param name="updatedTask">Task-ul actualizat.</param>
        public void ModifyTask(Client client, string taskName, Task updatedTask)
        {
            // Caută task-ul după numele acestuia
            var task = client.Tasks.FirstOrDefault(t => t.NumeTask == taskName);
            if (task != null)
            {
                // Actualizează proprietățile task-ului
                task.NumeTask = updatedTask.NumeTask;
                task.DescriereTask = updatedTask.DescriereTask;
                task.OreLogate = updatedTask.OreLogate;
            }
        }

        /// <summary>
        /// Șterge un task din lista de task-uri a clientului specificat.
        /// </summary>
        /// <param name="client">Clientul pentru care se șterge task-ul.</param>
        /// <param name="taskName">Numele task-ului de șters.</param>
        public void RemoveTask(Client client, string taskName)
        {
            // Caută task-ul după numele acestuia
            var task = client.Tasks.FirstOrDefault(t => t.NumeTask == taskName);
            if (task != null)
            {
                // Șterge task-ul din listă
                client.Tasks.Remove(task);
            }
        }

        /// <summary>
        /// Vizualizează toate task-urile asociate cu clientul specificat.
        /// </summary>
        /// <param name="client">Clientul pentru care se vizualizează task-urile.</param>
        /// <returns>O listă de task-uri asociate cu clientul.</returns>
        public List<Task> ViewTasks(Client client)
        {
            return client.Tasks;
        }
    }
}
