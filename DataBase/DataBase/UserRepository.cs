using DataBase;
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
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (FirstName, LastName, UserStat, TeamLeadId) VALUES (@FirstName, @LastName, @UserStat, @TeamLeadId)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@UserStat", user.UserStat.ToString());
                    command.Parameters.AddWithValue("@TeamLeadId", (object)user.TeamLeadId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<User> GetAllUsers()
        {
            var users = new List<User>();
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
                            users.Add(new User
                            {
                                UserId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                UserStat = (UserStats)Enum.Parse(typeof(UserStats), reader.GetString(3)),
                                TeamLeadId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4)
                            });
                        }
                    }
                }
            }
            return users;
        }

        public User GetUserById(int userId)
        {
            User user = null;
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
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                UserStat = (UserStats)Enum.Parse(typeof(UserStats), reader.GetString(3)),
                                TeamLeadId = reader.IsDBNull(4) ? (int?)null : reader.GetInt32(4)
                            };
                        }
                    }
                }
            }
            return user;
        }

        public void UpdateUser(User user)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
            UPDATE Users 
            SET FirstName = @FirstName, 
                LastName = @LastName, 
                UserStat = @UserStat, 
                TeamLeadId = @TeamLeadId 
            WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", user.FirstName);
                    command.Parameters.AddWithValue("@LastName", user.LastName);
                    command.Parameters.AddWithValue("@UserStat", user.UserStat.ToString());
                    command.Parameters.AddWithValue("@TeamLeadId", (object)user.TeamLeadId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@UserId", user.UserId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteUser(int userId)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE UserId = @UserId";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}