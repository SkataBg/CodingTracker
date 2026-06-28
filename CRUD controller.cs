using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker;
internal class CRUD_controller
{
    static string connectionString = DatabaseHelper.GetConnectionString();
    internal static void Insert()
    {
        string startTime;
        string endTime;
        string duration;
        while (true) {
            startTime = DatabaseHelper.GetTimeInput(TimeType.StartTime);
            endTime = DatabaseHelper.GetTimeInput(TimeType.EndTime);
            if (startTime == null || endTime == null)
            {
                return;
            }

            duration = DatabaseHelper.GetDuration(startTime, endTime);
            if (duration != null)
            {
                break;
            }
        }
        using (var connection = new SqliteConnection(connectionString))
        {
            var sql = "INSERT INTO coding_sessions(StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
            {
                var newEntry = new { StartTime = startTime, EndTime = endTime, Duration = duration };

                var rowsAffected = connection.Execute(sql, newEntry);
            }
        }
        AnsiConsole.MarkupLine($"Successfuly inserted the following session:\n" +
             $"[green]Start: {startTime}[/] " +
             $"[yellow]End: {endTime}[/] " +
             $"[blue]Duration: {duration}[/] ");
    }
}