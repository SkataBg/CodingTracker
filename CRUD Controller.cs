using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using CodingTracker.Model;
using System.Globalization;

namespace CodingTracker;
internal class CRUD_Controller
{
    static string connectionString = DatabaseHelper.GetConnectionString();
    internal static void Insert()
    {
        string startTime;
        string endTime;
        string duration;

        while (true) 
        {
            startTime = DatabaseHelper.GetTimeInput(TimeType.StartTime);
            if (startTime == null) return;

            endTime = DatabaseHelper.GetTimeInput(TimeType.EndTime);
            if (endTime == null) return;

            duration = DatabaseHelper.GetDuration(startTime, endTime);
            if (duration != null)
            {
                break;
            }
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            var sql = "INSERT INTO coding_sessions(startTime, endTime, duration) VALUES (@startTime, @endTime, @duration)";


            var newEntry = new { startTime = startTime, endTime = endTime, duration = duration };

            connection.Execute(sql, newEntry);
            
        }

        AnsiConsole.MarkupLine($"Successfuly inserted the following session:\n" +
             $"[green]Start: {startTime}[/] " +
             $"[yellow]End: {endTime}[/] " +
             $"[blue]Duration: {duration}[/] ");
        Console.ReadKey();
    }

    internal static void DisplayAllSessions()
    {
        Console.Clear();
        List<CodingSession> tableData = DatabaseHelper.GetAllCodingSessions();
            
            if (tableData.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No rows found[/]");
                return;  
            }

            AnsiConsole.MarkupLine("---------------------------------------------------------------------------------");
            foreach (var session in tableData)
            {
                AnsiConsole.MarkupLine($"{session.Id}.Start - {session.StartTime.ToString("dd MMMM yyyy HH:mm:ss")}" +
                    $"  End - {session.EndTime.ToString("dd MMMM yyyy HH:mm:ss")}  Duration - {session.Duration.ToString()}");
            }
        AnsiConsole.MarkupLine("---------------------------------------------------------------------------------");
        Console.ReadKey();
    }


    internal static void Delete()
    {
        CodingSession session = DatabaseHelper.GetSelectedSession();
        if (session == null) return;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var sql = @"DELETE FROM coding_sessions WHERE id= @id";
            var rowsDeleted = connection.Execute(sql, new { id = session.Id});
            AnsiConsole.MarkupLine($"[green]Successfully deleted {rowsDeleted} row(s). Returning to Main Menu.[/]");
            connection.Close();
        }
    }
    internal static void Update()
    {
        CodingSession session = DatabaseHelper.GetSelectedSession();
        if (session == null) return;

        string startTime;
        string endTime;
        string duration;

        while (true)
        {
            startTime = DatabaseHelper.GetTimeInput(TimeType.StartTime);
            if (startTime == null) return;

            endTime = DatabaseHelper.GetTimeInput(TimeType.EndTime);
            if (endTime == null) return;

            duration = DatabaseHelper.GetDuration(startTime, endTime);
            if (duration != null) break;
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var sql = @"UPDATE coding_sessions SET startTime = @startTime, endTime = @endTime, duration = @duration WHERE id = @id";
            var updatedEntry = new { startTime = startTime, endTime = endTime, duration = duration, id = session.Id};
            connection.Execute(sql, updatedEntry);
            connection.Close();

            AnsiConsole.MarkupLine("[green]Successfuly updated the chosen session![/]");
            Console.ReadKey();
        }
    }

    internal static void LiveRecording()
    {

    }
}