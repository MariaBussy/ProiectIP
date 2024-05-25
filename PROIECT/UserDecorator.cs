using System.Collections.Generic;
using TaskLibrary;

namespace PROIECT
{
    public abstract class UserDecorator : IUser
    {
        protected IUser _user;

        public UserDecorator(IUser user)
        {
            _user = user;
        }

        public string Name
        {
            get { return _user.Name; }
            set { _user.Name = value; }
        }

        public List<Task> Tasks
        {
            get { return _user.Tasks; }
            set { _user.Tasks = value; }
        }

        public IUserState State
        {
            get { return _user.State; }
            set { _user.State = value; }
        }

        public virtual void AddTask(Task task)
        {
            _user.AddTask(task);
        }

        public virtual void ModifyTask(string taskName, Task updatedTask)
        {
            _user.ModifyTask(taskName, updatedTask);
        }

        public virtual void RemoveTask(string taskName)
        {
            _user.RemoveTask(taskName);
        }

        public virtual List<Task> ViewTasks()
        {
            return _user.ViewTasks();
        }

        public virtual void EditTask(string taskName, string newName, string newDescription, double newLoggedHours)
        {
            _user.EditTask(taskName, newName, newDescription, newLoggedHours);
        }
    }
}
