using System.IO;

public class FileSaveStorage : ISaveStorage
{
    private readonly string _rootPath;

    public FileSaveStorage(string rootPath)
    {
        _rootPath = rootPath;
        Directory.CreateDirectory(_rootPath);
    }

    private string PathFor(string slotId) => System.IO.Path.Combine(_rootPath, $"{slotId}.sav");

    public void Write(string slotId, string content) =>
        File.WriteAllText(PathFor(slotId), content);

    public string Read(string slotId) =>
        File.ReadAllText(PathFor(slotId));

    public bool Exists(string slotId) =>
        File.Exists(PathFor(slotId));

    public void Delete(string slotId)
    {
        string p = PathFor(slotId);
        if (File.Exists(p)) File.Delete(p);
    }
}