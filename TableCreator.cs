using Microsoft.Data.Sqlite;

namespace CodingTracker;

internal class TableCreator
{
    static string connectionString = DatabaseHelper.GetConnectionString();

    internal static void CreateTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = @"CREATE TABLE IF NOT EXISTS coding_sessions(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTime TEXT,
                Duration TEXT)";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    internal static void DropTable(string tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DROP TABLE IF EXISTS {tableName}";
            tableCmd.ExecuteNonQuery();
            connection.Close() ;
        }
    }
}
