namespace Obnovlyator.Models;

public static class Settings
{
	public static readonly string ServerUrl = "koldrif.ru";
	public static readonly string ManifestFileName = "manifest.json";
	public static string ExecLocation => Environment.CurrentDirectory;
}