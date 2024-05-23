using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROIECT
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Crearea utilizatorilor cu stări specifice
            IUser pers1 = new Client { Name = "Alice", State = new ManagerState() };
            IUser pers2 = new Client { Name = "Bob", State = new TeamLeadState() };
            IUser pers3 = new Client { Name = "Charlie", State = new DeveloperState() };

            // Adăugăm un decorator pentru logging și pentru funcționalități de vizualizare/editare
            pers1 = new LoggingUserDecorator(pers1);
            pers2 = new LoggingUserDecorator(pers2);

            // Adăugare task-uri
            Task task1 = new Task { Name = "Task1", Description = "Description1", Deadline = DateTime.Now.AddDays(1) };
            pers1.AddTask(task1);

            Task task2 = new Task { Name = "Task2", Description = "Description2", Deadline = DateTime.Now.AddDays(2) };
            pers2.AddTask(task2);

            // Vizualizare task-uri
            Console.WriteLine("Tasks for Alice:");
            foreach (var task in pers1.ViewTasks())
            {
                Console.WriteLine($" - {task.Name}, {task.Description}, {task.Deadline}");
            }

            // Editare task-uri
            pers1.EditTask("Task1", "Task1 Edited", "Description1 Edited", DateTime.Now.AddDays(3));

            // Ștergere task-uri
            pers1.RemoveTask("Task1 Edited");

            // Afișare utilizatori și task-uri
            DisplayUserTasks(pers1);
            DisplayUserTasks(pers2);
            DisplayUserTasks(pers3);
        }

        private static void DisplayUserTasks(IUser user)
        {
            Console.WriteLine($"User: {user.Name}, Role: {user.State.GetType().Name.Replace("State", "")}");
            foreach (var task in user.ViewTasks())
            {
                Console.WriteLine($" - Task: {task.Name}, Description: {task.Description}, Deadline: {task.Deadline}");
            }
        }
    }
}

