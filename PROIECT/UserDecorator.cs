using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT
{
    // Clasa abstractă pentru decorator
    public abstract class UserDecorator : IUser
    {
        protected IUser _user;

        public UserDecorator(IUser user)
        {
            _user = user;
        }

        public virtual string Name { get => _user.Name; set => _user.Name = value; }
        public virtual List<Task> Tasks { get => _user.Tasks; set => _user.Tasks = value; }
        public virtual IUserState State { get => _user.State; set => _user.State = value; }

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

        public virtual void EditTask(string taskName, string newName, string newDescription, DateTime newDeadline)
        {
            _user.EditTask(taskName, newName, newDescription, newDeadline);
        }
    }

}
