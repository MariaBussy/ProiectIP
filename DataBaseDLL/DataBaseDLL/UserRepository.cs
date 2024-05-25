﻿using System;
using System.Collections.Generic;
using System.Data.SQLite;
using TaskLibrary;

namespace DataBaseDLL
{
    /// <summary>
    /// Clasă care se ocupă de operațiile legate de utilizatori în baza de date.
    /// </summary>
    public class UserRepository
    {
        private string connectionString = "Data Source=database.sqlite;Version=3;";

        /// <summary>
        /// Creează un nou utilizator în baza de date.
        /// </summary>
        /// <param name="user">Utilizatorul de creat.</param>
        public void CreateUser(User user)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (Name, UserStat, TeamLeadId, Username, Password) VALUES (@Name, @UserStat, @TeamLeadId, @Username, @Password)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@UserStat", user.UserStat.ToString());
                        command.Parameters.AddWithValue("@TeamLeadId", (object)user.TeamLeadId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("User created successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating the user: {ex.Message}");
            }
        }

        /// <summary>
        /// Obține o listă cu toți utilizatorii din baza de date.
        /// </summary>
        /// <returns>O listă de utilizatori.</returns>
        public List<User> GetAllUsers()
        {
            var users = new List<User>();
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Users";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                User user = new User
                                {
                                    UserId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    UserStat = (UserStats)Enum.Parse(typeof(UserStats), reader.GetString(2)),
                                    TeamLeadId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                                    Username = reader.GetString(4),
                                    Password = reader.GetString(5)
                                };

                                user.Tasks = GetUserTasks(user.UserId); // This method should return List<TaskLibrary.Task>
                                users.Add(user);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving all users: {ex.Message}");
            }
            return users;
        }

        /// <summary>
        /// Obține lista de task-uri asociate unui anumit utilizator.
        /// </summary>
        /// <param name="userId">ID-ul utilizatorului.</param>
        /// <returns>O listă de task-uri.</returns>
        private List<TaskLibrary.Task> GetUserTasks(int userId) // Ensure correct return type
        {
            var tasks = new List<TaskLibrary.Task>();
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    SELECT t.* 
                    FROM Tasks t
                    INNER JOIN UserTasks ut ON t.TaskId = ut.TaskId
                    WHERE ut.UserId = @UserId";
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
                Console.WriteLine($"An error occurred while retrieving tasks for user with ID {userId}: {ex.Message}");
            }
            return tasks;
        }

        /// <summary>
        /// Obține un utilizator din baza de date în funcție de ID-ul său.
        /// </summary>
        /// <param name="userId">ID-ul utilizatorului.</param>
        /// <returns>Utilizatorul găsit sau null dacă nu există.</returns>
        public User GetUserById(int userId)
        {
            User user = null;
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Users WHERE UserId = @UserId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = new User
                                {
                                    UserId = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    UserStat = (UserStats)Enum.Parse(typeof(UserStats), reader.GetString(2)),
                                    TeamLeadId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the user by ID: {ex.Message}");
            }
            return user;
        }

        /// <summary>
        /// Actualizează informațiile unui utilizator în baza de date.
        /// </summary>
        /// <param name="user">Utilizatorul cu noile informații.</param>
        public void UpdateUser(User user)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                    UPDATE Users 
                    SET Name = @Name, 
                        UserStat = @UserStat, 
                        TeamLeadId = @TeamLeadId,
                        Username = @Username, 
                        Password = @Password 
                    WHERE UserId = @UserId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@UserStat", user.UserStat.ToString());
                        command.Parameters.AddWithValue("@TeamLeadId", (object)user.TeamLeadId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Username", user.Username);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@UserId", user.UserId);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("User updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the user: {ex.Message}");
            }
        }

        /// <summary>
        /// Șterge un utilizator din baza de date împreună cu toate task-urile asociate acestuia.
        /// </summary>
        /// <param name="userId">ID-ul utilizatorului de șters.</param>
        public void DeleteUser(int userId)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Delete user tasks first
                    string deleteTasksQuery = "DELETE FROM UserTasks WHERE UserId = @UserId";
                    using (var deleteTasksCommand = new SQLiteCommand(deleteTasksQuery, connection))
                    {
                        deleteTasksCommand.Parameters.AddWithValue("@UserId", userId);
                        deleteTasksCommand.ExecuteNonQuery();
                    }

                    // Then delete the user
                    string deleteUserQuery = "DELETE FROM Users WHERE UserId = @UserId";
                    using (var command = new SQLiteCommand(deleteUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine("User and associated tasks deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the user: {ex.Message}");
            }
        }
        /// <summary>
        /// Obține o listă cu toți utilizatorii care sunt subordonați unui anumit șef de echipă.
        /// </summary>
        /// <param name="teamLeadId">ID-ul șefului de echipă.</param>
        /// <returns>O listă de utilizatori subordonați.</returns>
        public List<User> GetUsersByTeamLeadId(int teamLeadId)
        {
            var users = new List<User>();
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Users WHERE TeamLeadId = @TeamLeadId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TeamLeadId", teamLeadId);
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
                Console.WriteLine($"An error occurred while retrieving users by TeamLeadId: {ex.Message}");
            }
            return users;
        }

    }
}
