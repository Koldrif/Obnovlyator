namespace Obnovlyator.Shared;

public static class Settings
{
	public static readonly string ServerUrl = "http://multiplayerlearning.koldrif.ru:3333";
	public static readonly string ManifestFileName = "manifest.json";
	public static string ExecLocation => Environment.CurrentDirectory;
	public static string OperatingDirectory => Path.GetFullPath("../", Environment.CurrentDirectory);
}