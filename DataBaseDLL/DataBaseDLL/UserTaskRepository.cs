using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLibrary;

namespace DataBaseDLL
{
    /// <summary>
    /// Clasă care se ocupă de operațiile legate de atribuirea task-urilor utilizatorilor în baza de date.
    /// </summary>

    public class UserTaskRepository
    {
        private string connectionString = "Data Source=database.sqlite;Version=3;";

        /// <summary>
        /// Adaugă o atribuire între un task și un utilizator în baza de date.
        /// </summary>
        /// <param name="taskId">ID-ul task-ului.</param>
        /// <param name="userId">ID-ul utilizatorului.</param>
        public void AddUserTask(int taskId, int userId)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO UserTasks (UserId, TaskId) VALUES (@UserId, @TaskId)";

                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@TaskId", taskId);

                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Task assigned successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while assigning the Task: {ex.Message}");
            }
        }

        /// <summary>
        /// Șterge o atribuire între un task și un utilizator din baza de date.
        /// </summary>
        /// <param name="taskId">ID-ul task-ului.</param>
        /// <param name="userId">ID-ul utilizatorului.</param>
        public void DeleteUserTask(int taskId, int userId)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM UserTasks WHERE UserId = @UserId AND TaskId = @TaskId";

                    using (var command = new SQLiteCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@TaskId", taskId);

                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Task unassigned successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while unassigning the Task: {ex.Message}");
            }
        }

        /// <summary>
        /// Actualizează atribuirea unui task către un utilizator în baza de date.
        /// </summary>
        /// <param name="oldTaskId">ID-ul vechi al task-ului.</param>
        /// <param name="oldUserId">ID-ul vechi al utilizatorului.</param>
        /// <param name="newTaskId">ID-ul nou al task-ului.</param>
        /// <param name="newUserId">ID-ul nou al utilizatorului.</param>
        public void UpdateUserTask(int oldTaskId, int oldUserId, int newTaskId, int newUserId)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE UserTasks SET TaskId = @NewTaskId, UserId = @NewUserId WHERE TaskId = @OldTaskId AND UserId = @OldUserId";

                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewTaskId", newTaskId);
                        command.Parameters.AddWithValue("@NewUserId", newUserId);
                        command.Parameters.AddWithValue("@OldTaskId", oldTaskId);
                        command.Parameters.AddWithValue("@OldUserId", oldUserId);

                        command.ExecuteNonQuery();
                    }

                    Console.WriteLine("Task assignment updated successfully!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the Task assignment: {ex.Message}");
            }
        }
        /// <summary>
        /// Obține lista de task-uri atribuite unui utilizator.
        /// </summary>
        /// <param name="userId">ID-ul utilizatorului.</param>
        /// <returns>O listă de task-uri atribuite utilizatorului.</returns>
        public List<TaskLibrary.Task> GetUserTasks(int userId)
        {
            var tasks = new List<TaskLibrary.Task>();
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT Tasks.TaskId, Tasks.NumeTask, Tasks.DescriereTask, Tasks.NumeAssigner
                FROM Tasks
                INNER JOIN UserTasks ON Tasks.TaskId = UserTasks.TaskId
                WHERE UserTasks.UserId = @UserId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var task = new TaskLibrary.Task(reader.GetString(1), reader.GetString(5))
                                {
                                    OreLogate = reader.GetDouble(3),
                                    DescriereTask = reader.GetString(4)
                                };
                                tasks.Add(task);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving user tasks: {ex.Message}");
            }
            return tasks;
        }
        /// <summary>
        /// Obține lista de utilizatori care au atribuit un anumit task.
        /// </summary>
        /// <param name="taskId">ID-ul task-ului.</param>
        /// <returns>O listă de utilizatori care au atribuit task-ul.</returns>
        public List<User> GetUsersByTaskId(int taskId)
        {
            var users = new List<User>();
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT Users.UserId, Users.Name, Users.UserStat, Users.TeamLeadId
                FROM Users
                INNER JOIN UserTasks ON Users.UserId = UserTasks.UserId
                WHERE UserTasks.TaskId = @TaskId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TaskId", taskId);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new User
                                {
                                    UserId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    UserStat = (UserStats)Enum.Parse(typeof(UserStats), reader.GetString(2)),
                                    TeamLeadId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving users by task ID: {ex.Message}");
            }
            return users;
        }

    }
}
