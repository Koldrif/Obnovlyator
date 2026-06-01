using Newtonsoft.Json;
using Obnovlyator.Server;

var exeDirectory = Environment.CurrentDirectory;
var manifestFileName = "manifest.json";

var manifest = await ManifestCreator.Create(exeDirectory);

var manifestJson = JsonConvert.SerializeObject(manifest, Formatting.Indented);
await File.WriteAllTextAsync(Path.Combine(exeDirectory, manifestFileName), manifestJson);
