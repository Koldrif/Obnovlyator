namespace Obnovlyator.Models;

public record Manifest
{
	public Manifest() : this(DateTime.Now)
	{
		
	}

	public Manifest(DateTime version)
	{
		Version = version.ToString("yyyy-MM-dd-hh-mm-ss");
		Files = new();
	}
	public string Version { get; set; }
	public List<ManifestFileInfo> Files { get; set; }
}

public record ManifestFileInfo
{
	public string Path;
	public string SHA; 
}