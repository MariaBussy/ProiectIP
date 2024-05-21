using System;

namespace DataBase
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database database = new Database(); // Ensure the database is created
            UserRepository userRepository = new UserRepository();
            TaskRepository taskRepository = new TaskRepository();

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Display all users");
                Console.WriteLine("2. Display all tasks");
                Console.WriteLine("3. Add a user");
                Console.WriteLine("4. Add a task for a specific user");
                Console.WriteLine("5. Modify a user");
                Console.WriteLine("6. Modify a task");
                Console.WriteLine("7. Delete a user");
                Console.WriteLine("8. Delete a task");
                Console.WriteLine("9. Exit");
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
                        AddTaskForUser(taskRepository);
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
                    case "9":
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        static void DisplayAllUsers(UserRepository userRepository)
        {
            var users = userRepository.GetAllUsers();
            Console.WriteLine("\nAll Users:");
            foreach (var user in users)
            {
                Console.WriteLine($"UserId: {user.UserId}, FirstName: {user.FirstName}, LastName: {user.LastName}, UserStat: {user.UserStat}, TeamLeadId: {(user.TeamLeadId.HasValue ? user.TeamLeadId.ToString() : "N/A")}");
            }
        }

        static void DisplayAllTasks(TaskRepository taskRepository)
        {
            var tasks = taskRepository.GetAllTasks();
            Console.WriteLine("\nAll Tasks:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"TaskId: {task.TaskId}, Title: {task.Title}, Description: {task.Description}, Status: {task.Status}, EstimatedTime: {task.EstimatedTime}, Notes: {task.Notes}, UserId: {task.UserId}");
            }
        }

        static void AddUser(UserRepository userRepository)
        {
            Console.Write("Enter FirstName: ");
            string firstName = Console.ReadLine();
            Console.Write("Enter LastName: ");
            string lastName = Console.ReadLine();

            User newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserStat = UserStats.Employee, // Default UserStat
                TeamLeadId = null
            };

            userRepository.CreateUser(newUser);
            Console.WriteLine("User added successfully!");
        }
        static void AddTaskForUser(TaskRepository taskRepository)
        {
            Console.Write("Enter UserId for the task: ");
            int userId = int.Parse(Console.ReadLine());
            Console.Write("Enter Title for the task: ");
            string title = Console.ReadLine();
            Console.Write("Enter Description for the task: ");
            string description = Console.ReadLine();
            Console.Write("Enter Status for the task: ");
            string status = Console.ReadLine();
            Console.Write("Enter EstimatedTime for the task: ");
            int estimatedTime = int.Parse(Console.ReadLine());
            Console.Write("Enter Notes for the task: ");
            string notes = Console.ReadLine();

            Task newTask = new Task
            {
                Title = title,
                Description = description,
                Status = status,
                EstimatedTime = estimatedTime,
                Notes = notes,
                UserId = userId
            };

            taskRepository.CreateTask(newTask);
            Console.WriteLine("Task added successfully!");
        }

        static void ModifyUser(UserRepository userRepository)
        {
            Console.Write("Enter UserId of the user to modify: ");
            int userId = int.Parse(Console.ReadLine());

            // Collect new details
            Console.Write("Enter new FirstName: ");
            string newFirstName = Console.ReadLine();
            Console.Write("Enter new LastName: ");
            string newLastName = Console.ReadLine();
            Console.Write("Enter new UserStat (Manager/TeamLeader/Employee): ");
            string newUserStatStr = Console.ReadLine();
            UserStats newUserStat;
            if (!Enum.TryParse(newUserStatStr, out newUserStat))
            {
                Console.WriteLine("Invalid UserStat entered. User not modified.");
                return;
            }

            int? newTeamLeadId = null;
            Console.Write("Enter new TeamLeadId (leave blank for none): ");
            string newTeamLeadIdStr = Console.ReadLine();
            if (!string.IsNullOrEmpty(newTeamLeadIdStr))
            {
                if (!int.TryParse(newTeamLeadIdStr, out int teamLeadId))
                {
                    Console.WriteLine("Invalid TeamLeadId entered. User not modified.");
                    return;
                }
                newTeamLeadId = teamLeadId;
            }

            // Create modified user object
            User modifiedUser = new User
            {
                UserId = userId,
                FirstName = newFirstName,
                LastName = newLastName,
                UserStat = newUserStat,
                TeamLeadId = newTeamLeadId
            };

            // Update user
            userRepository.UpdateUser(modifiedUser);
            Console.WriteLine("User modified successfully!");
        }


        static void ModifyTask(TaskRepository taskRepository)
        {
            Console.Write("Enter TaskId of the task to modify: ");
            int taskId = int.Parse(Console.ReadLine());
            Console.Write("Enter new Title: ");
            string newTitle = Console.ReadLine();
            Console.Write("Enter new Description: ");
            string newDescription = Console.ReadLine();
            Console.Write("Enter new Status: ");
            string newStatus = Console.ReadLine();
            Console.Write("Enter new EstimatedTime: ");
            int newEstimatedTime = int.Parse(Console.ReadLine());
            Console.Write("Enter new Notes: ");
            string newNotes = Console.ReadLine();

            Task modifiedTask = new Task
            {
                TaskId = taskId,
                Title = newTitle,
                Description = newDescription,
                Status = newStatus,
                EstimatedTime = newEstimatedTime,
                Notes = newNotes
            };

            taskRepository.UpdateTask(modifiedTask);
            Console.WriteLine("Task modified successfully!");
        }

        static void DeleteUser(UserRepository userRepository)
        {
            Console.Write("Enter UserId of the user to delete: ");
            int userId = int.Parse(Console.ReadLine());

            userRepository.DeleteUser(userId);
            Console.WriteLine("User deleted successfully!");
        }

        static void DeleteTask(TaskRepository taskRepository)
        {
            Console.Write("Enter TaskId of the task to delete: ");
            int taskId = int.Parse(Console.ReadLine());

            taskRepository.DeleteTask(taskId);
            Console.WriteLine("Task deleted successfully!");
        }

    }
}
