using Newtonsoft.Json;
using Obnovlyator.Models;
using Obnovlyator.Server;

var manifest = await ManifestCreator.Create(Settings.ExecLocation);

var manifestJson = JsonConvert.SerializeObject(manifest, Formatting.Indented);
await File.WriteAllTextAsync(Path.Combine(Settings.ExecLocation, Settings.ManifestFileName), manifestJson);
