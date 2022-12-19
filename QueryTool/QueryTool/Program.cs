// See https://aka.ms/new-console-template for more information
using CsvHelper;
using System.Globalization;
using Sharprompt;
using ConsoleApp;


IEnumerable<CPUUsage> records;

using (var reader = new StreamReader("C:\\Users\\Aditya\\source\\repos\\ConsoleApp\\ConsoleApp\\CPUUsage.csv"))
using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
{
    records = csv.GetRecords<CPUUsage>();
}

var name = Prompt.Input<string>("What's your name?");
Console.WriteLine($"Hello, {name}!");

// Password input
var secret = Prompt.Password("Type new password", validators: new[] { Validators.Required(), Validators.MinLength(8) });
Console.WriteLine("Password OK");

// Confirmation
var answer = Prompt.Confirm("Are you ready?", defaultValue: true);
Console.WriteLine($"Your answer is {answer}");