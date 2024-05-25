using System;
using System.Collections.Generic;
using System.Data.SQLite;
using TaskLibrary;

namespace DataBaseDLL
{
    /// <summary>
    /// Clasă care se ocupă de operațiile legate de task-uri în baza de date.
    /// </summary>

    public class TaskRepository
    {
        private string connectionString = "Data Source=database.sqlite;Version=3;";

        /// <summary>
        /// Creează un nou task în baza de date.
        /// </summary>
        /// <param name="task">Task-ul de creat.</param>
        public void CreateTask(TaskLibrary.Task task)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    INSERT INTO Tasks (NumeTask, DataAsignarii, OreLogate, DescriereTask, NumeAssigner) 
                    VALUES (@NumeTask, @DataAsignarii, @OreLogate, @DescriereTask, @NumeAssigner)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NumeTask", task.NumeTask);
                        command.Parameters.AddWithValue("@DataAsignarii", DateTime.Now);
                        command.Parameters.AddWithValue("@OreLogate", 0);
                        command.Parameters.AddWithValue("@DescriereTask", task.DescriereTask);
                        command.Parameters.AddWithValue("@NumeAssigner", task.NumeAssigner);
                    }

                }
                Console.WriteLine("Task created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the task: {ex.Message}");
            }
        }

        /// <summary>
        /// Obține o listă cu toate task-urile din baza de date.
        /// </summary>
        /// <returns>O listă de task-uri.</returns>
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
                                var task = new Task(reader.GetString(1), reader.GetString(5))
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
                Console.WriteLine($"An error occurred while retrieving all tasks: {ex.Message}");
            }
            return tasks;
        }

        /// <summary>
        /// Obține un task din baza de date în funcție de ID-ul său.
        /// </summary>
        /// <param name="taskId">ID-ul task-ului.</param>
        /// <returns>Task-ul găsit sau null dacă nu există.</returns>
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
                                task = new Task(reader.GetString(1), reader.GetString(5))
                                {
                                    OreLogate = reader.GetDouble(3),
                                    DescriereTask = reader.GetString(4)
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

        /// <summary>
        /// Actualizează informațiile unui task în baza de date.
        /// </summary>
        /// <param name="task">Task-ul cu noile informații.</param>
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

        /// <summary>
        /// Șterge un task din baza de date împreună cu toate atribuirile acestuia către utilizatori.
        /// </summary>
        /// <param name="taskId">ID-ul task-ului de șters.</param>
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
