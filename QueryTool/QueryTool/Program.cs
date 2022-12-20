// See https://aka.ms/new-console-template for more information
using CsvHelper;
using System.Globalization;
using Sharprompt;
using ConsoleApp;
using QueryTool;

// Global Variables

string generate = "Generate CPU Usage";
string query = "Query CPU Usage";
string quit = "Quit";

var generateLogs = new GenerateLogs();
var querlyLogs = new QueryLogs();

Prompt.Symbols.Prompt = new Symbol(">", ">");

while (true)
{
    var action = Prompt.Select("Select your action", new[] { generate, query, quit });

    if (action.Equals(generate))
    {
        generateLogs.Generate();
    }

    if (action.Equals(query))
    {
        querlyLogs.QueryLog();
    }

    if (action.Equals(quit))
    {
        break;
    }

}
