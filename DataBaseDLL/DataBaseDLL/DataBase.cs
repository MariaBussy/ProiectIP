using System;
using System.Data.SQLite;
using TaskLibrary;

/// <summary>
/// Clasa responsabilă de gestionarea bazei de date și a tabelelor asociate.
/// </summary>
public class Database
{
    private string connectionString = "Data Source=database.sqlite;Version=3;";

    public Database()
    {
        CreateDatabase();
    }

    /// <summary>
    /// Creează baza de date și tabelele asociate (dacă nu există).
    /// </summary>
    private void CreateDatabase()
    {
        try
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createUsersTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Users (
                        UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        UserStat TEXT NOT NULL,
                        TeamLeadId INTEGER,
                        Username TEXT NOT NULL UNIQUE,
                        Password TEXT NOT NULL
                    )";

                string createTasksTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        TaskId INTEGER PRIMARY KEY AUTOINCREMENT,
                        NumeTask TEXT NOT NULL,
                        DataAsignarii DATETIME,
                        OreLogate REAL,
                        DescriereTask TEXT,
                        NumeAssigner TEXT
                    )";

                string createUserTasksTableQuery = @"
                    CREATE TABLE IF NOT EXISTS UserTasks (
                        UserId INTEGER,
                        TaskId INTEGER,
                        PRIMARY KEY (UserId, TaskId),
                        FOREIGN KEY (UserId) REFERENCES Users (UserId),
                        FOREIGN KEY (TaskId) REFERENCES Tasks (TaskId)
                    )";

                using (var command = new SQLiteCommand(createUsersTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(createTasksTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SQLiteCommand(createUserTasksTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                Console.WriteLine("Database and tables created successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating the database: {ex.Message}");
        }
    }
}
