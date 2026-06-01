using System.Net.Http.Json;
using Newtonsoft.Json;
using Obnovlyator.Models;

var execDirectory = Environment.CurrentDirectory;
var manifestLocation = Path.Combine(Settings.ExecLocation, Settings.ManifestFileName);

// if (!AskDestinationCorrectness(execDirectory))
// {
// 	Console.WriteLine("Ask Igor what to do");
// 	Console.ReadLine();
// 	return;
// }

using var httpClient = new HttpClient();
var manifestUri = new Uri($"http://{Settings.ServerUrl}/{Settings.ManifestFileName}");
var manifest = JsonConvert.DeserializeObject<Manifest>(await httpClient.GetStringAsync(manifestUri));
Console.WriteLine(manifest);

// var manifestFile = File.Exists();

bool AskDestinationCorrectness(string currentlyRunningDirection)
{
	Console.Clear();
	bool bIsCorrectInput = false;
	while (!bIsCorrectInput)
	{
		Console.WriteLine($"Is that direction, where you want to install and update program?");
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

	throw new ArgumentException("Unable to proccess input");
}