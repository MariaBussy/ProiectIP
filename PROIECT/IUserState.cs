using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    /// <summary>
    /// Interfața IUserState definește comportamentul pe care un utilizator îl poate avea în funcție de starea sa.
    /// </summary>
    public interface IUserState
    {
        /// <summary>
        /// Adaugă un task pentru clientul specificat.
        /// </summary>
        /// <param name="client">Clientul pentru care se adaugă task-ul.</param>
        /// <param name="task">Task-ul de adăugat.</param>
        void AddTask(Client client, Task task);

        /// <summary>
        /// Modifică un task existent al clientului specificat.
        /// </summary>
        /// <param name="client">Clientul pentru care se modifică task-ul.</param>
        /// <param name="taskName">Numele task-ului de modificat.</param>
        /// <param name="updatedTask">Task-ul actualizat.</param>
        void ModifyTask(Client client, string taskName, Task updatedTask);

        /// <summary>
        /// Șterge un task existent al clientului specificat.
        /// </summary>
        /// <param name="client">Clientul pentru care se șterge task-ul.</param>
        /// <param name="taskName">Numele task-ului de șters.</param>
        void RemoveTask(Client client, string taskName);

        /// <summary>
        /// Vizualizează toate task-urile asociate cu clientul specificat.
        /// </summary>
        /// <param name="client">Clientul pentru care se vizualizează task-urile.</param>
        /// <returns>O listă de task-uri asociate cu clientul.</returns>
        List<Task> ViewTasks(Client client);
    }
}
