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
            try
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
                Console.WriteLine("Task created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the task: {ex.Message}");
            }
        }

        public List<Task> GetAllTasks(int userId)
        {
            var tasks = new List<Task>();
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving tasks: {ex.Message}");
            }
            return tasks;
        }

        public List<Task> GetAllTasks()
        {
            var tasks = new List<Task>();
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving all tasks: {ex.Message}");
            }
            return tasks;
        }

        public Task GetTaskById(int taskId)
        {
            Task task = null;
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the task by ID: {ex.Message}");
            }
            return task;
        }

        public void UpdateTask(Task task)
        {
            try
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
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("Task updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the task: {ex.Message}");
            }
        }

        public void DeleteTask(int taskId)
        {
            try
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
                Console.WriteLine("Task deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the task: {ex.Message}");
            }
        }
    }
}
