using DataBase;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
namespace DataBase
{

    public class TaskRepository
    {
        private string connectionString = "Data Source=database.sqlite;Version=3;";

        public void CreateTask(Task task)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                INSERT INTO Tasks (Title, Description, Status, EstimatedTime, Notes, UserId) 
                VALUES (@Title, @Description, @Status, @EstimatedTime, @Notes, @UserId)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", task.Description);
                    command.Parameters.AddWithValue("@Status", task.Status);
                    command.Parameters.AddWithValue("@EstimatedTime", task.EstimatedTime);
                    command.Parameters.AddWithValue("@Notes", task.Notes);
                    command.Parameters.AddWithValue("@UserId", task.UserId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Task> GetAllTasks(int userId)
        {
            var tasks = new List<Task>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tasks WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task
                            {
                                TaskId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Status = reader.IsDBNull(3) ? null : reader.GetString(3),
                                EstimatedTime = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                Notes = reader.IsDBNull(5) ? null : reader.GetString(5),
                                UserId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            return tasks;
        }

        public List<Task> GetAllTasks()
        {
            var tasks = new List<Task>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tasks";
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tasks.Add(new Task
                            {
                                TaskId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Status = reader.IsDBNull(3) ? null : reader.GetString(3),
                                EstimatedTime = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                Notes = reader.IsDBNull(5) ? null : reader.GetString(5),
                                UserId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6)
                            });
                        }
                    }
                }
            }
            return tasks;
        }

        public Task GetTaskById(int taskId)
        {
            Task task = null;
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tasks WHERE TaskId = @TaskId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TaskId", taskId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            task = new Task
                            {
                                TaskId = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                                Status = reader.IsDBNull(3) ? null : reader.GetString(3),
                                EstimatedTime = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4),
                                Notes = reader.IsDBNull(5) ? null : reader.GetString(5),
                                UserId = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6)
                            };
                        }
                    }
                }
            }
            return task;
        }

        public void UpdateTask(Task task)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                UPDATE Tasks 
                SET Title = @Title, Description = @Description, Status = @Status, 
                    EstimatedTime = @EstimatedTime, Notes = @Notes 
                WHERE TaskId = @TaskId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", (object)task.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", (object)task.Status ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EstimatedTime", (object)task.EstimatedTime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Notes", (object)task.Notes ?? DBNull.Value);
                    command.Parameters.AddWithValue("@TaskId", task.TaskId);
                    command.Parameters.AddWithValue("@UserId", (object)task.UserId ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void DeleteTask(int taskId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TaskId", taskId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
