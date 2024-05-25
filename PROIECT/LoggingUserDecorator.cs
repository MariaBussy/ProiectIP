using System;
using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    /// <summary>
    /// Clasa LoggingUserDecorator extinde funcționalitatea unui utilizator prin adăugarea de mesaje de logare pentru fiecare operațiune.
    /// </summary>
    public class LoggingUserDecorator : UserDecorator
    {
        /// <summary>
        /// Constructorul pentru LoggingUserDecorator, care primește un obiect IUser.
        /// </summary>
        /// <param name="user">Obiectul IUser pe care îl decorează.</param>
        public LoggingUserDecorator(IUser user) : base(user) { }

        /// <summary>
        /// Adaugă un task și loghează operațiunea.
        /// </summary>
        /// <param name="task">Task-ul de adăugat.</param>
        public override void AddTask(Task task)
        {
            Console.WriteLine($"Adding task '{task.NumeTask}' for user '{Name}'");
            base.AddTask(task);
        }

        /// <summary>
        /// Modifică un task și loghează operațiunea.
        /// </summary>
        /// <param name="taskName">Numele task-ului de modificat.</param>
        /// <param name="updatedTask">Task-ul actualizat.</param>
        public override void ModifyTask(string taskName, Task updatedTask)
        {
            Console.WriteLine($"Modifying task '{taskName}' for user '{Name}'");
            base.ModifyTask(taskName, updatedTask);
        }

        /// <summary>
        /// Șterge un task și loghează operațiunea.
        /// </summary>
        /// <param name="taskName">Numele task-ului de șters.</param>
        public override void RemoveTask(string taskName)
        {
            Console.WriteLine($"Removing task '{taskName}' for user '{Name}'");
            base.RemoveTask(taskName);
        }

        /// <summary>
        /// Vizualizează toate task-urile și loghează operațiunea.
        /// </summary>
        /// <returns>O listă de task-uri.</returns>
        public override List<Task> ViewTasks()
        {
            Console.WriteLine($"Viewing tasks for user '{Name}'");
            return base.ViewTasks();
        }

        /// <summary>
        /// Editează un task și loghează operațiunea.
        /// </summary>
        /// <param name="taskName">Numele task-ului de editat.</param>
        /// <param name="newName">Noul nume al task-ului.</param>
        /// <param name="newDescription">Noua descriere a task-ului.</param>
        /// <param name="newLoggedHours">Noile ore logate pentru task.</param>
        public override void EditTask(string taskName, string newName, string newDescription, double newLoggedHours)
        {
            Console.WriteLine($"Editing task '{taskName}' for user '{Name}'");
            base.EditTask(taskName, newName, newDescription, newLoggedHours);
        }
    }
}
