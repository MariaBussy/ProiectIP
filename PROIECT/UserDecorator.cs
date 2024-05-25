using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    /// <summary>
    /// Clasa abstractă UserDecorator extinde funcționalitatea unui obiect IUser prin intermediul pattern-ului Decorator.
    /// </summary>
    public abstract class UserDecorator : IUser
    {
        /// <summary>
        /// Obiectul IUser pe care îl decorează.
        /// </summary>
        protected IUser _user;

        /// <summary>
        /// Constructorul pentru UserDecorator, care primește un obiect IUser.
        /// </summary>
        /// <param name="user">Obiectul IUser pe care îl decorează.</param>
        public UserDecorator(IUser user)
        {
            _user = user;
        }

        /// <summary>
        /// Proprietatea Name preia sau setează numele utilizatorului decorat.
        /// </summary>
        public string Name
        {
            get { return _user.Name; }
            set { _user.Name = value; }
        }

        /// <summary>
        /// Proprietatea Tasks preia sau setează lista de task-uri a utilizatorului decorat.
        /// </summary>
        public List<Task> Tasks
        {
            get { return _user.Tasks; }
            set { _user.Tasks = value; }
        }

        /// <summary>
        /// Proprietatea State preia sau setează starea utilizatorului decorat.
        /// </summary>
        public IUserState State
        {
            get { return _user.State; }
            set { _user.State = value; }
        }

        /// <summary>
        /// Metodă virtuală pentru adăugarea unui task la utilizatorul decorat.
        /// </summary>
        /// <param name="task">Task-ul de adăugat.</param>
        public virtual void AddTask(Task task)
        {
            _user.AddTask(task);
        }

        /// <summary>
        /// Metodă virtuală pentru modificarea unui task al utilizatorului decorat.
        /// </summary>
        /// <param name="taskName">Numele task-ului de modificat.</param>
        /// <param name="updatedTask">Task-ul actualizat.</param>
        public virtual void ModifyTask(string taskName, Task updatedTask)
        {
            _user.ModifyTask(taskName, updatedTask);
        }

        /// <summary>
        /// Metodă virtuală pentru ștergerea unui task al utilizatorului decorat.
        /// </summary>
        /// <param name="taskName">Numele task-ului de șters.</param>
        public virtual void RemoveTask(string taskName)
        {
            _user.RemoveTask(taskName);
        }

        /// <summary>
        /// Metodă virtuală pentru vizualizarea tuturor task-urilor utilizatorului decorat.
        /// </summary>
        /// <returns>O listă de task-uri.</returns>
        public virtual List<Task> ViewTasks()
        {
            return _user.ViewTasks();
        }

        /// <summary>
        /// Metodă virtuală pentru editarea unui task al utilizatorului decorat.
        /// </summary>
        /// <param name="taskName">Numele task-ului de editat.</param>
        /// <param name="newName">Noul nume al task-ului.</param>
        /// <param name="newDescription">Noua descriere a task-ului.</param>
        /// <param name="newLoggedHours">Noile ore logate pentru task.</param>
        public virtual void EditTask(string taskName, string newName, string newDescription, double newLoggedHours)
        {
            _user.EditTask(taskName, newName, newDescription, newLoggedHours);
        }
    }
}
