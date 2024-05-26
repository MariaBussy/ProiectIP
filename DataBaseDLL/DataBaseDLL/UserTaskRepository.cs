using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskLibrary;

namespace DataBase
{
    public class UserTaskRepository
    {
        private string connectionString = "Data Source=database.sqlite;Version=3;";
        private TaskRepository taskRepository = new TaskRepository();

        public void AddUserTask(string numeTask, int userId)
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
                        command.Parameters.AddWithValue("@TaskId", taskRepository.GetTaskId(numeTask));

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

        public void DeleteUserTask(string numeTask, int userId)
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
                        command.Parameters.AddWithValue("@TaskId", taskRepository.GetTaskId(numeTask));

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

        public void UpdateUserTask(string oldTaskName, int oldUserId, string newTaskName, int newUserId)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE UserTasks SET TaskId = @NewTaskId, UserId = @NewUserId WHERE TaskId = @OldTaskId AND UserId = @OldUserId";

                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NewTaskId", taskRepository.GetTaskId(newTaskName));
                        command.Parameters.AddWithValue("@NewUserId", newUserId);
                        command.Parameters.AddWithValue("@OldTaskId", taskRepository.GetTaskId(oldTaskName));
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
        public List<TaskLibrary.Task> GetUserTasks(int userId)
        {
            var tasks = new List<TaskLibrary.Task>();
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                SELECT Tasks.TaskId, Tasks.NumeTask, Tasks.dataAsignarii, Tasks.OreLogate, Tasks.DescriereTask, Tasks.NumeAssigner
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
                                TaskLibrary.Task task = new TaskLibrary.Task(nume: reader.GetString(1), numePersoana: reader.GetString(5));
                                task.DataAsignariiAsDateTime = reader.GetDateTime(2);
                                task.OreLogate = reader.GetDouble(3);
                                task.DescriereTask = reader.GetString(4);

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
        /*public List<User> GetUsersByTaskId(int taskId)
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
        }*/

    }
}
