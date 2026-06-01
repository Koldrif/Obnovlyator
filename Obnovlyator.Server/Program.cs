using System.Security.Cryptography;
using Newtonsoft.Json;
using Obnovlyator.Models;

var exeDirectory = Environment.CurrentDirectory;
var manifest = new Manifest();

var tasks = new List<Task<ManifestFileInfo>>();
foreach (var filePath in Directory.EnumerateFiles(exeDirectory, "*", SearchOption.AllDirectories))
{
	tasks.Add(Task.Run(() =>
	{
		var fileInfo = File.OpenRead(filePath);
		var sha = Convert.ToHexString(SHA256.HashData(fileInfo));
		return new ManifestFileInfo()
		{
			Path = Path.GetRelativePath(exeDirectory, filePath),
			SHA = sha
		};
	}));
}

var fileInfos = await Task.WhenAll(tasks);

manifest.Files = fileInfos.ToList();

// Console.WriteLine(JsonSerializer.Serialize(manifest, new JsonSerializerOptions() {WriteIndented = true, IncludeFields = true}));
Console.WriteLine(JsonConvert.SerializeObject(manifest, Formatting.Indented));
