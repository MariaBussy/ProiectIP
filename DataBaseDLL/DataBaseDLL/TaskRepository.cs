using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DataBaseDLL
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
                    INSERT INTO Tasks (NumeTask, DataAsignarii, OreLogate, DescriereTask, NumeAssigner) 
                    VALUES (@NumeTask, @DataAsignarii, @OreLogate, @DescriereTask, @NumeAssigner);
                    SELECT last_insert_rowid()";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NumeTask", task.NumeTask);
                        command.Parameters.AddWithValue("@DataAsignarii", DateTime.Now);
                        command.Parameters.AddWithValue("@OreLogate", 0);
                        command.Parameters.AddWithValue("@DescriereTask", task.DescriereTask);
                        command.Parameters.AddWithValue("@NumeAssigner", task.NumeAssigner);
                        task.TaskId = Convert.ToInt32(command.ExecuteScalar());
                    }

                }
                Console.WriteLine("Task created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the task: {ex.Message}");
            }
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
                                    NumeTask = reader.GetString(1),
                                    DataAsignarii = reader.GetDateTime(2),
                                    OreLogate = reader.GetDouble(3),
                                    DescriereTask = reader.GetString(4),
                                    NumeAssigner = reader.GetString(5)
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
                                    NumeTask = reader.GetString(1),
                                    DataAsignarii = reader.GetDateTime(2),
                                    OreLogate = reader.GetDouble(3),
                                    DescriereTask = reader.GetString(4),
                                    NumeAssigner = reader.GetString(5)
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
                        SET NumeTask = @NumeTask, 
                            DataAsignarii = @DataAsignarii, 
                            OreLogate = @OreLogate, 
                            DescriereTask = @DescriereTask, 
                            NumeAssigner = @NumeAssigner 
                        WHERE TaskId = @TaskId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NumeTask", task.NumeTask);
                        command.Parameters.AddWithValue("@DataAsignarii", task.DataAsignarii);
                        command.Parameters.AddWithValue("@OreLogate", task.OreLogate);
                        command.Parameters.AddWithValue("@DescriereTask", task.DescriereTask);
                        command.Parameters.AddWithValue("@NumeAssigner", task.NumeAssigner);
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

                    // First, delete user tasks associated with the task
                    string deleteUserTasksQuery = "DELETE FROM UserTasks WHERE TaskId = @TaskId";
                    using (var deleteUserTasksCommand = new SQLiteCommand(deleteUserTasksQuery, connection))
                    {
                        deleteUserTasksCommand.Parameters.AddWithValue("@TaskId", taskId);
                        deleteUserTasksCommand.ExecuteNonQuery();
                    }

                    // Then, delete the task itself
                    string deleteTaskQuery = "DELETE FROM Tasks WHERE TaskId = @TaskId";
                    using (var deleteTaskCommand = new SQLiteCommand(deleteTaskQuery, connection))
                    {
                        deleteTaskCommand.Parameters.AddWithValue("@TaskId", taskId);
                        deleteTaskCommand.ExecuteNonQuery();
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
