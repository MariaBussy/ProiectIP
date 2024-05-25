using System;
using System.Collections.Generic;

namespace DataBase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*Database database = new Database(); // Ensure the database is created
            UserRepository userRepository = new UserRepository();
            TaskRepository taskRepository = new TaskRepository();
            UserTaskRepository userTaskRepository = new UserTaskRepository();

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Display all users");
                Console.WriteLine("2. Display all tasks");
                Console.WriteLine("3. Add a user");
                Console.WriteLine("4. Add a task for specific users");
                Console.WriteLine("5. Modify a user");
                Console.WriteLine("6. Modify a task");
                Console.WriteLine("7. Delete a user");
                Console.WriteLine("8. Delete a task");
                Console.WriteLine("9. Display all tasks for a user");
                Console.WriteLine("10. Display all users for a task");
                Console.WriteLine("11. Display users by teamleadId");
                Console.WriteLine("12. Exit");
                Console.Write("Select an option: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        DisplayAllUsers(userRepository);
                        break;
                    case "2":
                        DisplayAllTasks(taskRepository);
                        break;
                    case "3":
                        AddUser(userRepository);
                        break;
                    case "4":
                        AddTaskForUser(taskRepository, userRepository, userTaskRepository);
                        break;
                    case "5":
                        ModifyUser(userRepository);
                        break;
                    case "6":
                        ModifyTask(taskRepository);
                        break;
                    case "7":
                        DeleteUser(userRepository);
                        break;
                    case "8":
                        DeleteTask(taskRepository);
                        break;
                    case "12":
                        return;
                    case "9":
                        DisplayUserTasks(userRepository, taskRepository, userTaskRepository);
                        break;
                    case "10":
                        DisplayTaskUsers(taskRepository, userTaskRepository);
                        break;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                    case "11":
                        DisplayUsersByTeamLead(userRepository);
                        break;
                }
            }
        }*/
        }
    }
}

        /*static void DisplayAllUsers(UserRepository userRepository)
        {
            try
            {
                var users = userRepository.GetAllUsers();
                Console.WriteLine("\nAll Users:");
                foreach (var user in users)
                {
                    Console.WriteLine($"UserId: {user.UserId}, Name: {user.Name}, UserStat: {user.UserStat}, TeamLeadId: {(user.TeamLeadId.HasValue ? user.TeamLeadId.ToString() : "N/A")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying users: {ex.Message}");
            }
        }

        static void DisplayAllTasks(TaskRepository taskRepository)
        {
            try
            {
                var tasks = taskRepository.GetAllTasks();
                Console.WriteLine("\nAll Tasks:");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"TaskId: {taskRepository.GetTaskId(task.NumeTask)}, NumeTask: {task.NumeTask}, DataAsignarii: {task.GetDataAsignarii}, OreLogate: {task.OreLogate}, DescriereTask: {task.DescriereTask}, NumeAssigner: {task.NumeAssigner}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying tasks: {ex.Message}");
            }
        }

        static void AddUser(UserRepository userRepository)
        {
            try
            {
                Console.Write("Enter Name: ");
                string name = Console.ReadLine();
                Console.Write("Enter UserStat (Manager/TeamLeader/Developer): ");
                string userStatStr = Console.ReadLine();
                UserStats userStat;
                if (!Enum.TryParse(userStatStr, out userStat))
                {
                    Console.WriteLine("Invalid UserStat entered. User not added.");
                    return;
                }

                User newUser = new User
                {
                    Name = name,
                    UserStat = userStat,
                    TeamLeadId = null,
                    Tasks = new List<Task>()
                };

                userRepository.CreateUser(newUser);
                Console.WriteLine("User added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding a user: {ex.Message}");
            }
        }

        static void AddTaskForUser(TaskRepository taskRepository, UserRepository userRepository, UserTaskRepository userTaskRepository)
        {
            try
            {
                Console.Write("Enter UserIds for the task (comma-separated): ");
                string[] userIdsStr = Console.ReadLine().Split(',');
                List<int> userIds = new List<int>();
                foreach (var userIdStr in userIdsStr)
                {
                    if (int.TryParse(userIdStr, out int userId))
                    {
                        userIds.Add(userId);
                    }
                }


                Console.Write("Enter NumeTask: ");
                string numeTask = Console.ReadLine();
                Console.Write("Enter DescriereTask: ");
                string descriereTask = Console.ReadLine();
                Console.Write("Enter NumeAssigner: ");
                string numeAssigner = Console.ReadLine();

                Task newTask = new Task
                {
                    NumeTask = numeTask,
                    DataAsignarii = DateTime.Now,
                    OreLogate = 0,
                    DescriereTask = descriereTask,
                    NumeAssigner = numeAssigner
                };

                taskRepository.CreateTask(newTask);
                foreach (int userId in userIds)
                {
                    userTaskRepository.AddUserTask(newTask.TaskId, userId);
                }
                Console.WriteLine("Task added successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding a task: {ex.Message}");
            }
        }
        static void ModifyUser(UserRepository userRepository)
        {
            try
            {
                Console.Write("Enter UserId of the user to modify: ");
                int userId = int.Parse(Console.ReadLine());
                var user = userRepository.GetUserById(userId);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return;
                }

                Console.Write("Enter new Name: ");
                string newName = Console.ReadLine();
                Console.Write("Enter new UserStat (Manager/TeamLeader/Developer): ");
                UserStats newUserStat;
                while (!Enum.TryParse(Console.ReadLine(), out newUserStat))
                {
                    Console.Write("Invalid UserStat. Enter new UserStat (Manager/TeamLeader/Developer): ");
                }

                Console.Write("Enter new TeamLeadId (leave blank for none): ");
                int? newTeamLeadId = null;
                string newTeamLeadIdStr = Console.ReadLine();
                if (!string.IsNullOrEmpty(newTeamLeadIdStr))
                {
                    if (int.TryParse(newTeamLeadIdStr, out int teamLeadId))
                    {
                        newTeamLeadId = teamLeadId;
                    }
                }

                User modifiedUser = new User
                {
                    UserId = userId,
                    Name = newName,
                    UserStat = newUserStat,
                    TeamLeadId = newTeamLeadId
                };

                userRepository.UpdateUser(modifiedUser);
                Console.WriteLine("User modified successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while modifying the user: {ex.Message}");
            }
        }

        static void ModifyTask(TaskRepository taskRepository)
        {
            try
            {
                Console.Write("Enter TaskId of the task to modify: ");
                int taskId = int.Parse(Console.ReadLine());
                var task = taskRepository.GetTaskById(taskId);
                if (task == null)
                {
                    Console.WriteLine("Task not found.");
                    return;
                }

                Console.Write("Enter new NumeTask: ");
                string newNumeTask = Console.ReadLine();
                Console.Write("Enter new DescriereTask: ");
                string newDescriereTask = Console.ReadLine();
                Console.Write("Enter new OreLogate: ");
                double newOreLogate = double.Parse(Console.ReadLine());
                Console.Write("Enter new NumeAssigner: ");
                string newNumeAssigner = Console.ReadLine();

                Task modifiedTask = new Task
                {
                    TaskId = taskId,
                    NumeTask = newNumeTask,
                    DescriereTask = newDescriereTask,
                    OreLogate = newOreLogate,
                    NumeAssigner = newNumeAssigner,
                    DataAsignarii = task.DataAsignarii
                };

                taskRepository.UpdateTask(modifiedTask);
                Console.WriteLine("Task modified successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while modifying the task: {ex.Message}");
            }
        }

        static void DeleteUser(UserRepository userRepository)
        {
            try
            {
                Console.Write("Enter UserId of the user to delete: ");
                int userId = int.Parse(Console.ReadLine());

                userRepository.DeleteUser(userId);
                Console.WriteLine("User deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the user: {ex.Message}");
            }
        }

        static void DeleteTask(TaskRepository taskRepository)
        {
            try
            {
                Console.Write("Enter TaskId of the task to delete: ");
                int taskId = int.Parse(Console.ReadLine());

                taskRepository.DeleteTask(taskId);
                Console.WriteLine("Task deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the task: {ex.Message}");
            }
        }
        static void DisplayUserTasks(UserRepository userRepository, TaskRepository taskRepository, UserTaskRepository userTaskRepository)
        {
            try
            {
                Console.Write("Enter UserId to display tasks: ");
                int userId = int.Parse(Console.ReadLine());

                var user = userRepository.GetUserById(userId);
                if (user == null)
                {
                    Console.WriteLine("User not found.");
                    return;
                }

                var tasks = userTaskRepository.GetUserTasks(userId);

                Console.WriteLine($"\nTasks for User '{user.Name}':");
                foreach (var task in tasks)
                {
                    Console.WriteLine($"TaskId: {task.TaskId}, NumeTask: {task.NumeTask}, DescriereTask: {task.DescriereTask}, NumeAssigner: {task.NumeAssigner}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying user tasks: {ex.Message}");
            }
        }
        static void DisplayTaskUsers(TaskRepository taskRepository, UserTaskRepository userTaskRepository)
        {
            try
            {
                Console.Write("Enter TaskId to display users assigned to the task: ");
                int taskId = int.Parse(Console.ReadLine());
                var task = taskRepository.GetTaskById(taskId);
                if (task == null)
                {
                    Console.WriteLine("Task not found.");
                    return;
                }

                var users = userTaskRepository.GetUsersByTaskId(taskId);
                Console.WriteLine($"\nUsers assigned to Task {task.NumeTask} (TaskID: {task.TaskId}):");
                foreach (var user in users)
                {
                    Console.WriteLine($"UserId: {user.UserId}, Name: {user.Name}, UserStat: {user.UserStat}, TeamLeadId: {(user.TeamLeadId.HasValue ? user.TeamLeadId.ToString() : "N/A")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying users for the task: {ex.Message}");
            }
        }
        static void DisplayUsersByTeamLead(UserRepository userRepository)
        {
            try
            {
                Console.Write("Enter TeamLeadId to display users under the team lead: ");
                int teamLeadId = int.Parse(Console.ReadLine());
                var users = userRepository.GetUsersByTeamLeadId(teamLeadId);
                Console.WriteLine($"\nUsers under TeamLeadId {teamLeadId}:");
                foreach (var user in users)
                {
                    Console.WriteLine($"UserId: {user.UserId}, Name: {user.Name}, UserStat: {user.UserStat}, TeamLeadId: {(user.TeamLeadId.HasValue ? user.TeamLeadId.ToString() : "N/A")}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while displaying users by TeamLeadId: {ex.Message}");
            }
        }

    }*/
