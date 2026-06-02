using Newtonsoft.Json;
using Obnovlyator.Client;
using Obnovlyator.Models;


if (!AskDestinationCorrectness(Settings.OperatingDirectory))
{
	Console.WriteLine("Ask Igor what to do");
	Console.ReadLine();
	return;
}

using var updater = new Updater();
await updater.UpdateAsync();

MessageEndProgram();



bool AskDestinationCorrectness(string currentlyRunningDirection)
{
	Console.Clear();
	while (true)
	{
		Console.WriteLine($"Is that direction, where you want to install and update the game?");
		Console.Write("Directory: ");
		Console.ForegroundColor = ConsoleColor.Cyan;
		Console.WriteLine($"{currentlyRunningDirection}");
		Console.ResetColor();
		Console.Write("Press y/n? ");
		var userInput = Console.ReadLine();
		switch (userInput?.ToUpper())
		{
			case "Y":
				return true;
			case "N":
				return false;
			default:
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Please use 'y' or 'n'");
				Console.ResetColor();
				break;
		}
	}
}

void MessageEndProgram()
{
	Console.ForegroundColor = ConsoleColor.Green;
	Console.WriteLine("Game is up to date");
	Console.ForegroundColor = ConsoleColor.Yellow;
	Console.WriteLine("If you want to redownload game just delete manifest.json");
	Console.ResetColor();
	Console.Write("Press enter to exit program. ");
	Console.ReadLine();
}

