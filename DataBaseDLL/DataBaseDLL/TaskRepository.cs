using System;
using System.Collections.Generic;
using System.Data.SQLite;
using TaskLibrary;

namespace DataBase
{
    public class TaskRepository
    {
        private string connectionString = "Data Source=database.sqlite;Version=3;";

        public void CreateTask(TaskLibrary.Task task)
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
                        command.Parameters.AddWithValue("@DataAsignarii", task.DataAsignariiAsDateTime);
                        command.Parameters.AddWithValue("@OreLogate", 0);
                        command.Parameters.AddWithValue("@DescriereTask", task.DescriereTask);
                        command.Parameters.AddWithValue("@NumeAssigner", task.NumeAssigner);
                        //task.TaskId = Convert.ToInt32(command.ExecuteScalar());

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

        public List<TaskLibrary.Task> GetAllTasks()
        {
            var tasks = new List<TaskLibrary.Task>();
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
                Console.WriteLine($"An error occurred while retrieving all tasks: {ex.Message}");
            }
            return tasks;
        }

        public int GetTaskId(string numeTask)
        {
            int id = 0;
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Tasks WHERE NumeTask = @NumeTask";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NumeTask", numeTask);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                id = reader.GetInt32(0);
                            }
                        }
                    }
                }
                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the task ID: {ex.Message}");
                return id;
            }
        }

        public TaskLibrary.Task GetTaskByName(string numeTask)
        {
            int taskId = GetTaskId(numeTask);
            TaskLibrary.Task task = new TaskLibrary.Task("newTask", "newAssigner");
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Tasks WHERE NumeTask=@NumeTask";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NumeTask", numeTask);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                task.NumeTask = reader.GetString(1);
                                task.DataAsignariiAsDateTime = reader.GetDateTime(2);
                                task.OreLogate = reader.GetDouble(3);
                                task.DescriereTask = reader.GetString(4);
                                task.NumeAssigner = reader.GetString(5);
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

        public void UpdateTask(TaskLibrary.Task task)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        UPDATE Tasks 
                        SET DataAsignarii = @DataAsignarii, 
                            OreLogate = @OreLogate, 
                            DescriereTask = @DescriereTask, 
                            NumeAssigner = @NumeAssigner 
                        WHERE TaskId = @TaskId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        //command.Parameters.AddWithValue("@NumeTask", task.NumeTask);
                        command.Parameters.AddWithValue("@DataAsignarii", task.DataAsignariiAsDateTime);
                        command.Parameters.AddWithValue("@OreLogate", task.OreLogate);
                        command.Parameters.AddWithValue("@DescriereTask", task.DescriereTask);
                        command.Parameters.AddWithValue("@NumeAssigner", task.NumeAssigner);
                        command.Parameters.AddWithValue("@TaskId", GetTaskId(task.NumeTask));
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

        public void DeleteTask(string numeTask)
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
                        deleteUserTasksCommand.Parameters.AddWithValue("@TaskId", GetTaskId(numeTask));
                        deleteUserTasksCommand.ExecuteNonQuery();
                    }

                    // Then, delete the task itself
                    string deleteTaskQuery = "DELETE FROM Tasks WHERE TaskId = @TaskId";
                    using (var deleteTaskCommand = new SQLiteCommand(deleteTaskQuery, connection))
                    {
                        deleteTaskCommand.Parameters.AddWithValue("@TaskId", GetTaskId(numeTask));
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