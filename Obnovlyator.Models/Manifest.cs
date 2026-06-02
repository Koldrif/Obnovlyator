namespace Obnovlyator.Models;

public record Manifest
{
	public Manifest() : this(Guid.NewGuid())
	{
		
	}

	public Manifest(Guid guid)
	{
		Version = guid.ToString();
		Files = new();
	}
	public string Version { get; set; }
	public List<ManifestFileInfo> Files { get; set; }
}