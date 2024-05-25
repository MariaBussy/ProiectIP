using System;
using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    /// <summary>
    /// Clasa Client reprezintă un utilizator și implementează interfața IUser.
    /// </summary>
    public class Client : IUser
    {
        /// <summary>
        /// Numele utilizatorului.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Lista de task-uri asociate cu utilizatorul.
        /// </summary>
        public List<Task> Tasks { get; set; } = new List<Task>();

        /// <summary>
        /// Starea curentă a utilizatorului, care definește comportamentul său.
        /// </summary>
        public IUserState State { get; set; }

        /// <summary>
        /// Adaugă un task la lista de task-uri a utilizatorului.
        /// </summary>
        /// <param name="task">Task-ul de adăugat.</param>
        public void AddTask(Task task)
        {
            State.AddTask(this, task);
        }

        /// <summary>
        /// Modifică un task existent în lista de task-uri a utilizatorului.
        /// </summary>
        /// <param name="taskName">Numele task-ului de modificat.</param>
        /// <param name="updatedTask">Task-ul actualizat.</param>
        public void ModifyTask(string taskName, Task updatedTask)
        {
            State.ModifyTask(this, taskName, updatedTask);
        }

        /// <summary>
        /// Șterge un task din lista de task-uri a utilizatorului.
        /// </summary>
        /// <param name="taskName">Numele task-ului de șters.</param>
        public void RemoveTask(string taskName)
        {
            State.RemoveTask(this, taskName);
        }

        /// <summary>
        /// Vizualizează toate task-urile asociate cu utilizatorul.
        /// </summary>
        /// <returns>O listă de task-uri.</returns>
        public List<Task> ViewTasks()
        {
            return State.ViewTasks(this);
        }

        /// <summary>
        /// Editează un task existent în lista de task-uri a utilizatorului.
        /// </summary>
        /// <param name="taskName">Numele task-ului de editat.</param>
        /// <param name="newName">Noul nume al task-ului.</param>
        /// <param name="newDescription">Noua descriere a task-ului.</param>
        /// <param name="newLoggedHours">Noile ore logate pentru task.</param>
        public void EditTask(string taskName, string newName, string newDescription, double newLoggedHours)
        {
            // Creează un task actualizat folosind noile valori
            var updatedTask = new Task(newName, this.Name)
            {
                DescriereTask = newDescription,
                OreLogate = newLoggedHours
            };
            // Modifică task-ul folosind starea 
        }
    }
}
