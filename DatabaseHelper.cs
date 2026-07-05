using CodingTracker.Model;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Globalization;
using Dapper;

namespace CodingTracker;
internal class DatabaseHelper
{
    static string connectionString = GetConnectionString();

    internal static string GetConnectionString()
    {
        string connectionString;

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        return connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    internal static string GetTimeInput(TimeType timeType)
    {
        string startingOrEndingTime = timeType == TimeType.StartTime ? "starting" : "ending";

        AnsiConsole.MarkupLine($"\n\nPlease insert the date and {startingOrEndingTime} time: [yellow](Format: yyyy-MM-dd HH:mm:ss)[/]");
        while (true)
        {
            AnsiConsole.MarkupLine("[yellow]Example: 2026-06-18 14:23:12[/]");
            AnsiConsole.MarkupLine("[green]Press 1 to input the current date and time.[/] Press 0 to go back to the main menu.");
            string inputTime = Console.ReadLine();

            if (inputTime == "0")
            {
                return null;
            }

            if (inputTime == "1")
            {
                inputTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }

            if (DateTime.TryParseExact(inputTime, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                return inputTime;
            }

            AnsiConsole.Markup("\n\n[red]Invalid Date. Please follow the formatting instructions:[/]");
            AnsiConsole.MarkupLine("[yellow] (Format: yyyy-MM-dd HH: mm: ss)[/]");
        }
    }

    internal static string GetDuration(string startTime, string endTime)
    {
        DateTime start = DateTime.ParseExact(startTime, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
        DateTime end = DateTime.ParseExact(endTime, "yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
        if (start >= end) // if start is later than end.
        {
            AnsiConsole.MarkupLine("[red]Ending Time is earlier that Starting Time. Please input data correctly.[/]");
            return null;
        }
        TimeSpan duration = end - start;
        if (duration.TotalDays > 0)
        {
            return $"{duration.Days}d {duration.Hours}h {duration.Minutes:D2}m {duration.Seconds:D2}s";
        }
        return $"{duration.Hours}h {duration.Minutes:D2}m {duration.Seconds:D2}s";
    }

    internal static List<CodingSession> GetAllCodingSessions()
    {
        List<CodingSession> sessions = new List<CodingSession>();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var sql = "SELECT * FROM coding_sessions";
            sessions = connection.Query<CodingSession>(sql).ToList();
            connection.Close();
        }

        return sessions;
    }

    internal static CodingSession GetSelectedSession()
    {
        List<CodingSession> tableData = DatabaseHelper.GetAllCodingSessions();
        Dictionary<string, CodingSession> promptOptions = new Dictionary<string, CodingSession>();
        foreach (var session in tableData)
        {
            promptOptions[$"#{session.Id} - {session.StartTime:dd MMM yyyy HH:mm:ss} (Duration: {session.Duration})"] = session;
        }
        promptOptions["Go Back"] = null;


        var choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("[yellow]Which session would you like to delete?[/]")
            .AddChoices(promptOptions.Keys));

        if (choice == "Go Back")
        {
            return null;
        }

        var selectedSession = promptOptions[choice];

        return selectedSession;
    }
}