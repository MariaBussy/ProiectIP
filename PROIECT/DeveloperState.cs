using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    /// <summary>
    /// Clasa DeveloperState implementează interfața IUserState și definește comportamentul specific pentru utilizatorii de tip Developer.
    /// </summary>
    public class DeveloperState : IUserState
    {
        /// <summary>
        /// Metoda AddTask nu face nimic deoarece Developerii nu pot adăuga task-uri.
        /// </summary>
        /// <param name="client">Clientul care încearcă să adauge un task.</param>
        /// <param name="task">Task-ul care încearcă să fie adăugat.</param>
        public void AddTask(Client client, Task task)
        {
            // Developerii nu pot adăuga task-uri
        }

        /// <summary>
        /// Metoda ModifyTask nu face nimic deoarece Developerii nu pot modifica task-uri.
        /// </summary>
        /// <param name="client">Clientul care încearcă să modifice un task.</param>
        /// <param name="taskName">Numele task-ului care încearcă să fie modificat.</param>
        /// <param name="updatedTask">Task-ul actualizat care încearcă să fie salvat.</param>
        public void ModifyTask(Client client, string taskName, Task updatedTask)
        {
            // Developerii nu pot modifica task-uri
        }

        /// <summary>
        /// Metoda RemoveTask nu face nimic deoarece Developerii nu pot șterge task-uri.
        /// </summary>
        /// <param name="client">Clientul care încearcă să șteargă un task.</param>
        /// <param name="taskName">Numele task-ului care încearcă să fie șters.</param>
        public void RemoveTask(Client client, string taskName)
        {
            // Developerii nu pot șterge task-uri
        }

        /// <summary>
        /// Metoda ViewTasks returnează lista de task-uri asociate cu clientul.
        /// </summary>
        /// <param name="client">Clientul pentru care se returnează lista de task-uri.</param>
        /// <returns>O listă de task-uri asociate cu clientul.</returns>
        public List<Task> ViewTasks(Client client)
        {
            return client.Tasks;
        }
    }
}
