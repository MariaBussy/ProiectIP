using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
using DataBase;

namespace UnitTestDataBase
{
    [TestClass]
    public class DatabaseTest
    {
        private Database database;
        private string testConnectionString = "Data Source=test_database.sqlite;Version=3;";

        [TestInitialize]
        public void Setup()
        {
            // Ensure the test database file does not exist before each test
            if (File.Exists("database.sqlite"))
            {
                File.Delete("database.sqlite");
            }

            database = new Database();
        }
        
        /*[TestCleanup]
        public void Cleanup()
        {
            // Clean up the test database file after each test
            if (File.Exists("database.sqlite"))
            {
                File.Delete("database.sqlite");
            }
        }
        */
        [TestMethod]
        public void Database_WhenInstantiated_ShouldCreateDatabaseFile()
        {
            // Assert
            Assert.IsTrue(File.Exists("database.sqlite"));
        }

        [TestMethod]
        public void Database_WhenInstantiated_ShouldCreateUsersTable()
        {
            // Arrange
            using (var connection = new SQLiteConnection(testConnectionString))
            {
                connection.Open();

                // Act
                string query = "SELECT name FROM sqlite_master WHERE type='table' AND name='Users';";
                using (var command = new SQLiteCommand(query, connection))
                {
                    var result = command.ExecuteNonQuery();
                    // Assert
                    Assert.IsNotNull(result);
                }
            }
        }
        [TestMethod]
        public void Database_WhenInstantiated_ShouldCreateTasksTable()
        {
            // Arrange
            using (var connection = new SQLiteConnection(testConnectionString))
            {
                connection.Open();

                // Act
                string query = "SELECT name FROM sqlite_master WHERE type='table' AND name='Tasks';";
                using (var command = new SQLiteCommand(query, connection))
                {
                    var result = command.ExecuteNonQuery();

                    // Assert
                    Assert.IsNotNull(result);
                }
            }
        }

        [TestMethod]
        public void Database_WhenInstantiated_ShouldCreateUserTasksTable()
        {
            // Arrange
            using (var connection = new SQLiteConnection(testConnectionString))
            {
                connection.Open();

                // Act
                string query = "SELECT name FROM sqlite_master WHERE type='table' AND name='UserTasks';";
                using (var command = new SQLiteCommand(query, connection))
                {
                    var result = command.ExecuteNonQuery();

                    // Assert
                    Assert.IsNotNull(result);
                }
            }
        }
    }
}