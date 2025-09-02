using System.IO;
using System.Threading.Tasks;

public class BinarySaveStorage : ISaveStorage<byte[]>
{
    private readonly string _rootPath;

    public BinarySaveStorage(string rootPath)
    {
        _rootPath = rootPath;
        if (!Directory.Exists(rootPath))
            Directory.CreateDirectory(_rootPath);
    }

    private string PathFor(string slotId) => Path.Combine(_rootPath, $"{slotId}.bin");

    public void Write(string slotId, byte[] content) =>
        File.WriteAllBytes(PathFor(slotId), content);

    public byte[] Read(string slotId) =>
        File.Exists(PathFor(slotId)) ? File.ReadAllBytes(PathFor(slotId)) : null;

    public async Task WriteAsync(string slotId, byte[] content) =>
        await File.WriteAllBytesAsync(PathFor(slotId), content);

    public async Task<byte[]> ReadAsync(string slotId)
    {
        string p = PathFor(slotId);

        if (!File.Exists(p))
            return null;

        return await File.ReadAllBytesAsync(p);
    }

    public bool Exists(string slotId) =>
        File.Exists(PathFor(slotId));

    public void Delete(string slotId)
    {
        string p = PathFor(slotId);
        if (File.Exists(p)) File.Delete(p);
    }
}