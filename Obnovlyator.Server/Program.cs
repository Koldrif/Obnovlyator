using Newtonsoft.Json;
using Obnovlyator.Shared;
using Obnovlyator.Server;

var manifest = await ManifestCreator.Create(Settings.OperatingDirectory);

var manifestJson = JsonConvert.SerializeObject(manifest, Formatting.Indented);
await File.WriteAllTextAsync(Path.Combine(Settings.OperatingDirectory, Settings.ManifestFileName), manifestJson);
