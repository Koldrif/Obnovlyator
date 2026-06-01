using System.Security.Cryptography;
using Obnovlyator.Models;

namespace Obnovlyator.Server;

public static class ManifestCreator
{
	public static async Task<Manifest> Create(string? exeDirectory = null)
	{
		var manifest = new Manifest();

		if (string.IsNullOrWhiteSpace(exeDirectory))
		{
			exeDirectory = Environment.CurrentDirectory;
		}

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
		return manifest;
	}
}