using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public static class SaveStorage
{
    private static string _rootPath;

    public static void Init(string path)
    {
        _rootPath = String.IsNullOrEmpty(path) ? "Saves" : path;
        if (!Directory.Exists(_rootPath))
            Directory.CreateDirectory(_rootPath);
    }

    public static FileInfo[] CheckSaves(string extension)
    {
        if (!Directory.Exists(_rootPath))
            throw new DirectoryNotFoundException($"La cartella '{_rootPath}' non esiste.");

        DirectoryInfo dir = new DirectoryInfo(_rootPath);
        return dir.GetFiles($"*.{extension}", SearchOption.TopDirectoryOnly);
    }

    private static string PathFor(string slotId, string extension) =>
        Path.Combine(_rootPath, $"{slotId}.{extension}");

    public static void Write(string slotId, List<PureRawData> datas)
    {
        using BinaryWriter writer = new(File.Open(PathFor(slotId, "bin"), FileMode.Create));
        writer.Write(datas.Count);
        DataSerializer.BytesSerialize(writer, datas);
    }

    public static void Write(string slotId, string content) =>
        File.WriteAllText(PathFor(slotId, "sav"), content);

    public static async Task WriteAsync(string slotId, byte[] content) =>
        await File.WriteAllBytesAsync(PathFor(slotId, "bin"), content);

    public static async Task WriteAsync(string slotId, string content) =>
        await File.WriteAllTextAsync(PathFor(slotId, "sav"), content);

    public static List<PureRawData> ReadBytes(string slotId)
    {
        using BinaryReader reader = new(File.Open(PathFor(slotId, "bin"), FileMode.OpenOrCreate));
        return DataSerializer.BinaryDeserialize<List<PureRawData>>(reader);
    }

    public static string ReadJson(string slotId) =>
        File.Exists(PathFor(slotId, "sav")) ? File.ReadAllText(PathFor(slotId, "sav")) : null;

    public static async Task<byte[]> ReadBytesAsync(string slotId)
    {
        string p = PathFor(slotId, "bin");

        if (!File.Exists(p))
            return null;

        return await File.ReadAllBytesAsync(p);
    }

    public static async Task<string> ReadJsonAsync(string slotId)
    {
        string p = PathFor(slotId, "sav");

        if (!File.Exists(p))
            return null;

        return await File.ReadAllTextAsync(p);
    }


    public static bool Exists(string slotId, string extension) =>
        File.Exists(PathFor(slotId, extension));

    public static void Delete(string slotId, string extension)
    {
        string p = PathFor(slotId, extension);
        if (File.Exists(p)) File.Delete(p);
    }

    public static void WriteJsonWithHeader(string slotId, MetaData header, List<PureRawData> datas)
    {
        string path = PathFor(slotId, "json");

        var json = DataSerializer.JsonSerializeWithHeader(header, datas);
        File.WriteAllText(path, json);
    }

    public static SaveContainer ReadJsonWithHeader(string slotId)
    {
        string path = PathFor(slotId, "json");
        if (!File.Exists(path)) return null;

        string raw = File.ReadAllText(path);
        return DataSerializer.DeserializeWithHeader(raw);
    }
    public static void WriteWithHeader(string slotId, MetaData header, List<PureRawData> datas)
    {
        using FileStream fs = new(PathFor(slotId, "bin"), FileMode.Create, FileAccess.Write);
        using BinaryWriter writer = new(fs);

        DataSerializer.BytesSerialize(writer, header);
        DataSerializer.BytesSerialize(writer, datas);
    }
    public static MetaData ReadHeader(string slotId)
    {
        string path = PathFor(slotId, "bin");
        if (!File.Exists(path)) return null;

        using FileStream fs = new(path, FileMode.Open, FileAccess.Read);
        using BinaryReader reader = new(fs);

        int headerSize = reader.ReadInt32();
        byte[] headerBytes = reader.ReadBytes(headerSize);
        string headerJson = System.Text.Encoding.UTF8.GetString(headerBytes);
        return DataSerializer.Deserialize<MetaData>(headerJson);
    }
}