using System;
using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    /// <summary>
    /// Interfața IUser definește operațiunile și proprietățile necesare pentru un utilizator.
    /// </summary>
    public interface IUser
    {
        /// <summary>
        /// Numele utilizatorului.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Lista de task-uri asociate cu utilizatorul.
        /// </summary>
        List<Task> Tasks { get; set; }

        /// <summary>
        /// Starea curentă a utilizatorului, care definește comportamentul său.
        /// </summary>
        IUserState State { get; set; }

        /// <summary>
        /// Adaugă un task la lista de task-uri a utilizatorului.
        /// </summary>
        /// <param name="task">Task-ul de adăugat.</param>
        void AddTask(Task task);

        /// <summary>
        /// Modifică un task existent în lista de task-uri a utilizatorului.
        /// </summary>
        /// <param name="taskName">Numele task-ului de modificat.</param>
        /// <param name="updatedTask">Task-ul actualizat.</param>
        void ModifyTask(string taskName, Task updatedTask);

        /// <summary>
        /// Șterge un task din lista de task-uri a utilizatorului.
        /// </summary>
        /// <param name="taskName">Numele task-ului de șters.</param>
        void RemoveTask(string taskName);

        /// <summary>
        /// Vizualizează toate task-urile asociate cu utilizatorul.
        /// </summary>
        /// <returns>O listă de task-uri.</returns>
        List<Task> ViewTasks();

        /// <summary>
        /// Editează un task existent în lista de task-uri a utilizatorului.
        /// </summary>
        /// <param name="taskName">Numele task-ului de editat.</param>
        /// <param name="newName">Noul nume al task-ului.</param>
        /// <param name="newDescription">Noua descriere a task-ului.</param>
        /// <param name="newLoggedHours">Noile ore logate pentru task.</param>
        void EditTask(string taskName, string newName, string newDescription, double newLoggedHours);
    }
}
