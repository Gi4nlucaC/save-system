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
        {
            Directory.CreateDirectory(_rootPath);
            throw new DirectoryNotFoundException("cartella non esiste la creo");
        }
    }

    #region WRITE

    public static void WriteJsonWithHeader(string slotId, MetaData header, List<PureRawData> datas)
    {
        string path = PathFor(slotId, "sav");

        var json = DataSerializer.JsonSerializeWithHeader(header, datas);
        File.WriteAllText(path, json);
    }

    public static void WriteBinariesWithHeader(string slotId, MetaData header, List<PureRawData> datas)
    {
        using FileStream fs = new(PathFor(slotId, "bin"), FileMode.Create, FileAccess.Write);
        using BinaryWriter writer = new(fs);

        var bytesHeader = DataSerializer.BytesSerialize(writer, header);
        var bytesData = DataSerializer.BytesSerialize(writer, datas);

        //CloudSave.SaveDataAsBinary(slotId, bytesHeader, bytesData);
    }

    #endregion

    #region READ

    public static SaveContainer ReadJsonWithHeader(string slotId)
    {
        string path = PathFor(slotId, "sav");
        if (!File.Exists(path)) return null;

        string raw = File.ReadAllText(path);
        return DataSerializer.DeserializeWithHeader(raw);
    }

    public static MetaData ReadBinaryHeader(string slotId)
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

    public static List<PureRawData> ReadBytes(string slotId)
    {
        using BinaryReader reader = new(File.Open(PathFor(slotId, "bin"), FileMode.OpenOrCreate));
        return DataSerializer.BinaryDeserialize<List<PureRawData>>(reader, true);
    }

    #endregion

    public static FileInfo[] CheckSaves(string extension)
    {
        if (!Directory.Exists(_rootPath))
            throw new DirectoryNotFoundException($"La cartella '{_rootPath}' non esiste.");

        DirectoryInfo dir = new(_rootPath);
        return dir.GetFiles($"*.{extension}", SearchOption.TopDirectoryOnly);
    }

    private static string PathFor(string slotId, string extension) =>
        Path.Combine(_rootPath, $"{slotId}.{extension}");

    public static bool Exists(string slotId, string extension) =>
        File.Exists(PathFor(slotId, extension));

    public static void Delete(string slotId, string extension)
    {
        string p = PathFor(slotId, extension);
        if (File.Exists(p)) File.Delete(p);
    }



}