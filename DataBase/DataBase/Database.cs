using System.Data.SQLite;

public class Database
{
    private string connectionString = "Data Source=database.sqlite;Version=3;";

    public Database()
    {
        CreateDatabase();
    }

    private void CreateDatabase()
    {
        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            string createUsersTableQuery = @"
                CREATE TABLE IF NOT EXISTS Users (
                    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
                    FirstName TEXT NOT NULL,
                    LastName TEXT NOT NULL, 
                    UserStat TEXT NOT NULL,
                    TeamLeadId INTEGER
                )";

            string createTasksTableQuery = @"
                CREATE TABLE IF NOT EXISTS Tasks (
                    TaskId INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title TEXT NOT NULL,
                    Description TEXT,
                    Status TEXT,
                    EstimatedTime INTEGER,
                    Notes TEXT,
                    UserId INTEGER,
                    FOREIGN KEY (UserId) REFERENCES Users (UserId)
                )";

            using (var command = new SQLiteCommand(createUsersTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            using (var command = new SQLiteCommand(createTasksTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
