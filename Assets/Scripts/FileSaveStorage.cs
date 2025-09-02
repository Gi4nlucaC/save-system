using System.IO;
using System.Threading.Tasks;

public class FileSaveStorage : ISaveStorage<string>
{
    private readonly string _rootPath;

    public FileSaveStorage(string rootPath)
    {
        _rootPath = rootPath;
        if (!Directory.Exists(rootPath))
            Directory.CreateDirectory(_rootPath);
    }

    private string PathFor(string slotId) => Path.Combine(_rootPath, $"{slotId}.sav");

    public void Write(string slotId, string content) =>
        File.WriteAllText(PathFor(slotId), content);

    public string Read(string slotId) =>
        File.Exists(PathFor(slotId)) ? File.ReadAllText(PathFor(slotId)) : null;

    public async Task WriteAsync(string slotId, string content) =>
        await File.WriteAllTextAsync(PathFor(slotId), content);

    public async Task<string> ReadAsync(string slotId)
    {
        string p = PathFor(slotId);

        if (!File.Exists(p))
            return null;

        return await File.ReadAllTextAsync(p);
    }

    public bool Exists(string slotId) =>
        File.Exists(PathFor(slotId));

    public void Delete(string slotId)
    {
        string p = PathFor(slotId);
        if (File.Exists(p)) File.Delete(p);
    }
}