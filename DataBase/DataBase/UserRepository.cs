using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace DataBase
{
    public class UserRepository
    {
        private string connectionString = "Data Source=database.sqlite;Version=3;";

        public void CreateUser(User user)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Users (Name, UserStat, TeamLeadId) VALUES (@Name, @UserStat, @TeamLeadId)";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@UserStat", user.UserStat.ToString());
                        command.Parameters.AddWithValue("@TeamLeadId", (object)user.TeamLeadId ?? DBNull.Value);
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
                                    TeamLeadId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3)
                                };

                                // Fetch tasks associated with this user
                                user.Tasks = GetUserTasks(user.UserId);

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

        // Method to fetch tasks associated with a specific user
        private List<Task> GetUserTasks(int userId)
        {
            var tasks = new List<Task>();
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
                                // Assuming Task object has a constructor that takes relevant parameters
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
                Console.WriteLine($"An error occurred while retrieving tasks for user with ID {userId}: {ex.Message}");
            }
            return tasks;
        }



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
                            TeamLeadId = @TeamLeadId 
                        WHERE UserId = @UserId";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Name", user.Name);
                        command.Parameters.AddWithValue("@UserStat", user.UserStat.ToString());
                        command.Parameters.AddWithValue("@TeamLeadId", (object)user.TeamLeadId ?? DBNull.Value);
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
