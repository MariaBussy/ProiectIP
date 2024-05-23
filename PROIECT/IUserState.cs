using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT
{
    // Interfața IUserState care definește comportamentul în funcție de rol
    public interface IUserState
    {
        void AddTask(Client user, Task task);
        void ModifyTask(Client user, string taskName, Task updatedTask);
        void RemoveTask(Client user, string taskName);
        List<Task> ViewTasks(Client user);
    }

}
