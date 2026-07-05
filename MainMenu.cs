using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker;

internal class MainMenu
{
    internal static void StartMenu()
    {
        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.MarkupLine("Welcome to CodingTracker.");
            var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuOption>()
                .Title("[yellow]What would you like to do?[/]")
                .AddChoices(Enum.GetValues<MenuOption>()));

            switch (choice)
            {
                case MenuOption.Insert:
                    CRUD_Controller.Insert();
                    break;
                case MenuOption.View_Sessions:
                    CRUD_Controller.DisplayAllSessions();
                    break;
                case MenuOption.Delete:
                    CRUD_Controller.Delete();
                    break;
                case MenuOption.Update:
                    CRUD_Controller.Update();
                    break;
                case MenuOption.Exit:
                    Environment.Exit(1);
                    break;
            }
        }

    }
}
