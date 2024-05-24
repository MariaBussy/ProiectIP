using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using DataBase;
using System.Data.SQLite;

namespace UnitTestDataBase
{
    [TestClass]
    public class UserRepositoryTests
    {
        private UserRepository userRepository;
        private Database database;
        private string testConnectionString = "Data Source=test_database.sqlite;Version=3;";

        [TestInitialize]
        public void Setup()
        {
            userRepository = new UserRepository();
            //database = new Database();
        }

        [TestMethod]
        public void CreateUser_WhenCalled_ShouldCreateUser()
        {
            // Arrange
            User user = new User
            {
                Name = "TestUser",
                UserStat = UserStats.Developer
            };

            // Act
            userRepository.CreateUser(user);

            // Assert
            bool userExistsInDatabase = false;
            using (var connection = new SQLiteConnection(testConnectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Users WHERE Name = @Name AND UserStat = @UserStat;";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@UserStat", user.UserStat.ToString());

                    int count = Convert.ToInt32(command.ExecuteScalar());
                    userExistsInDatabase = count > 0;
                }
            }

            Assert.IsTrue(userExistsInDatabase);
        }

        [TestMethod]
        public void GetAllUsers_WhenCalled_ShouldReturnAllUsers()
        {
            // Arrange - Insert test users into the database
            User user1 = new User { Name = "TestUser1", UserStat = UserStats.Manager };
            User user2 = new User { Name = "TestUser2", UserStat = UserStats.Developer };
            userRepository.CreateUser(user1);
            userRepository.CreateUser(user2);

            // Act
            List<User> users = userRepository.GetAllUsers();

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count);
        }

        [TestMethod]
        public void GetUserById_WhenValidIdPassed_ShouldReturnCorrectUser()
        {
            // Arrange - Insert test user into the database
            User user = new User { Name = "TestUser", UserStat = UserStats.Manager };
            userRepository.CreateUser(user);

            // Act
            User retrievedUser = userRepository.GetUserById(user.UserId);

            // Assert
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual(user.Name, retrievedUser.Name);
            Assert.AreEqual(user.UserStat, retrievedUser.UserStat);
        }

        [TestMethod]
        public void UpdateUser_WhenCalled_ShouldUpdateUser()
        {
            // Arrange - Insert test user into the database
            User user = new User { Name = "TestUser", UserStat = UserStats.Manager };
            userRepository.CreateUser(user);

            // Update user's properties
            user.Name = "UpdatedName";
            user.UserStat = UserStats.Developer;

            // Act
            userRepository.UpdateUser(user);
            User updatedUser = userRepository.GetUserById(user.UserId);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual(user.Name, updatedUser.Name);
            Assert.AreEqual(user.UserStat, updatedUser.UserStat);
        }

        [TestMethod]
        public void DeleteUser_WhenCalled_ShouldDeleteUser()
        {
            // Arrange - Insert test user into the database
            User user = new User { Name = "TestUser", UserStat = UserStats.Manager };
            userRepository.CreateUser(user);

            // Act
            userRepository.DeleteUser(user.UserId);
            User deletedUser = userRepository.GetUserById(user.UserId);

            // Assert
            Assert.IsNull(deletedUser);
        }

        [TestMethod]
        public void GetUsersByTeamLeadId_WhenValidIdPassed_ShouldReturnCorrectUsers()
        {
            // Arrange - Insert test users into the database with a specific TeamLeadId
            int teamLeadId = 1;
            User user1 = new User { Name = "TestUser1", UserStat = UserStats.Manager, TeamLeadId = teamLeadId };
            User user2 = new User { Name = "TestUser2", UserStat = UserStats.Developer, TeamLeadId = teamLeadId };
            userRepository.CreateUser(user1);
            userRepository.CreateUser(user2);

            // Act
            List<User> users = userRepository.GetUsersByTeamLeadId(teamLeadId);

            // Assert
            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count);
            // You can add more assertions to check if the retrieved users match the expected users
        }
    }
}
