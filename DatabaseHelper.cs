using Spectre.Console;
using Microsoft.Extensions.Configuration;
using System.Globalization;


namespace CodingTracker;
internal class DatabaseHelper
{
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

        AnsiConsole.MarkupLine($"\n\nPlease insert the date and {startingOrEndingTime} time: [yellow](Format: dd MMMM yyyy HH:mm:ss)[/]");
        while (true)
        {
            AnsiConsole.MarkupLine("[yellow]Example: 28 June 2026 14:23:12[/]");
            AnsiConsole.MarkupLine("[green]Press 1 to input the current date and time.[/] Press 0 to go back to the main menu.");
            string inputTime = Console.ReadLine();

            if (inputTime == "0")
            {
                return null;
            }

            if (inputTime == "1")
            {
                inputTime = DateTime.Now.ToString("dd MMMM yyyy HH:mm:ss");
            }

            if (DateTime.TryParseExact(inputTime, "dd MMMM yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                return inputTime;
            }

            AnsiConsole.Markup("\n\n[red]Invalid Date. Please follow the formatting instructions:[/]");
            AnsiConsole.MarkupLine("[yellow] (Format: dd MMMM yyyy HH: mm: ss)[/]");
        }
    }

    internal static string GetDuration(string startTime, string endTime)
    {
        DateTime start = DateTime.ParseExact(startTime, "dd MMMM yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
        DateTime end = DateTime.ParseExact(endTime, "dd MMMM yyyy HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None);
        if (start > end) // if start is later than end.
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
}
