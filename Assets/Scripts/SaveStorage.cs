using System.IO;
using System.Threading.Tasks;

public class SaveStorage
{
    private readonly string _rootPath;

    public SaveStorage(string rootPath)
    {
        _rootPath = rootPath;
        if (!Directory.Exists(rootPath))
            Directory.CreateDirectory(_rootPath);
    }

    private string PathFor(string slotId, string extension) => Path.Combine(_rootPath, $"{slotId}.{extension}");

    public void Write(string slotId, byte[] content) =>
        File.WriteAllBytes(PathFor(slotId, "bin"), content);

    public void Write(string slotId, string content) =>
        File.WriteAllText(PathFor(slotId, "sav"), content);

    public async Task WriteAsync(string slotId, byte[] content) =>
        await File.WriteAllBytesAsync(PathFor(slotId, "bin"), content);

    public async Task WriteAsync(string slotId, string content) =>
        await File.WriteAllTextAsync(PathFor(slotId, "sav"), content);

    public byte[] ReadBytes(string slotId) =>
        File.Exists(PathFor(slotId, "bin")) ? File.ReadAllBytes(PathFor(slotId, "bin")) : null;

    public string ReadJson(string slotId) =>
        File.Exists(PathFor(slotId, "sav")) ? File.ReadAllText(PathFor(slotId, "sav")) : null;

    public async Task<byte[]> ReadBytesAsync(string slotId)
    {
        string p = PathFor(slotId, "bin");

        if (!File.Exists(p))
            return null;

        return await File.ReadAllBytesAsync(p);
    }

    public async Task<string> ReadJsonAsync(string slotId)
    {
        string p = PathFor(slotId, "sav");

        if (!File.Exists(p))
            return null;

        return await File.ReadAllTextAsync(p);
    }


    public bool Exists(string slotId, string extension) =>
        File.Exists(PathFor(slotId, extension));

    public void Delete(string slotId, string extension)
    {
        string p = PathFor(slotId, extension);
        if (File.Exists(p)) File.Delete(p);
    }
}