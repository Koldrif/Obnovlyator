using Newtonsoft.Json;
using Obnovlyator.Shared;

namespace Obnovlyator.Client;

public sealed class Updater : IDisposable
{
	private readonly HttpClient _client = new HttpClient();
	
	public async Task UpdateAsync()
	{
		var manifestLocation = Path.Combine(Settings.OperatingDirectory, Settings.ManifestFileName);

		Manifest? localManifest = null;
		if (Path.Exists(manifestLocation))
		{
			localManifest = JsonConvert.DeserializeObject<Manifest>(File.ReadAllText(manifestLocation));
		}

		var manifestUri = new Uri($"{Settings.ServerUrl}/{Settings.ManifestFileName}");
		var manifestString = await _client.GetStringAsync(manifestUri);
		var serverManifest = JsonConvert.DeserializeObject<Manifest>(manifestString);

		if (serverManifest is null)
		{
			throw new ArgumentException("Unable to get manifest from server");
		}

		var filesToDownload = new List<ManifestFileInfo>();
		var filesToDelete = new List<ManifestFileInfo>();

		if (localManifest is null)
		{
			filesToDownload = serverManifest.Files;
		}
		else if (localManifest.Version == serverManifest.Version)
		{
			return;
		}
		else if (localManifest.Version != serverManifest.Version)
		{
			filesToDownload.AddRange(serverManifest.Files.Except(localManifest.Files));


			filesToDelete.AddRange(localManifest.Files.Except(serverManifest.Files));
		}

		await DeleteFiles(filesToDownload.Concat(filesToDelete));
		await DownloadFiles(filesToDownload);
		File.WriteAllText(Path.Combine(Settings.OperatingDirectory, Settings.ManifestFileName),
			JsonConvert.SerializeObject(serverManifest, Formatting.Indented));
	}

	async Task DeleteFiles(IEnumerable<ManifestFileInfo> filesToDelete)
	{
		var files = filesToDelete.ToList();
		var tasks = new List<Task>(files.Count);
		foreach (var fileInfo in files)
		{
			tasks.Add(Task.Run(() =>
			{
				var path = $"{Path.Combine(Settings.OperatingDirectory, fileInfo.Path)}";
				if (Path.Exists(path))
				{
					Console.WriteLine($"Deleting file: {path}");
					File.Delete(path);
				}
			}));
		}

		await Task.WhenAll(tasks);
	}

	async Task DownloadFiles(List<ManifestFileInfo> filesToDownload)
	{
		var tasks = new List<Task>(filesToDownload.Count);
		foreach (var file in filesToDownload)
		{
			tasks.Add(Task.Run(async () =>
			{
				var fileUri = new Uri($"{Settings.ServerUrl}/{file.Path}");
				var fileStream = await _client.GetStreamAsync(fileUri);
				var path = Path.Combine(Settings.OperatingDirectory, file.Path);
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				await using var writer = File.Create(path);
				await fileStream.CopyToAsync(writer);
			}));
		}

		await Task.WhenAll(tasks);
	}

	public void Dispose()
	{
		_client.Dispose();
	}
}