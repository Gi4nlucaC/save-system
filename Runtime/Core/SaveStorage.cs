using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Static utility class for handling local file system storage operations
    /// </summary>
    public static class SaveStorage
    {
        private static string _rootPath;

        /// <summary>
        /// Initialize the storage system with specified root path
        /// </summary>
        /// <param name="path">Root directory path for save files</param>
        public static void Init(string path)
        {
            _rootPath = string.IsNullOrEmpty(path) ? "Saves" : path;
            
            // Use persistent data path for better cross-platform support
            _rootPath = Path.Combine(Application.persistentDataPath, _rootPath);
            
            if (!Directory.Exists(_rootPath))
                Directory.CreateDirectory(_rootPath);
        }

        /// <summary>
        /// Get information about all save files with specified extension
        /// </summary>
        /// <param name="extension">File extension to search for</param>
        /// <returns>Array of FileInfo objects</returns>
        public static FileInfo[] CheckSaves(string extension)
        {
            if (!Directory.Exists(_rootPath))
                throw new DirectoryNotFoundException($"The folder '{_rootPath}' does not exist.");

            DirectoryInfo dir = new(_rootPath);
            return dir.GetFiles($"*.{extension}", SearchOption.TopDirectoryOnly);
        }

        /// <summary>
        /// Build full path for a save file
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="extension">File extension</param>
        /// <returns>Full file path</returns>
        private static string PathFor(string slotId, string extension) =>
            Path.Combine(_rootPath, $"{slotId}.{extension}");

        /// <summary>
        /// Write binary data to file
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="datas">Data to write</param>
        public static void Write(string slotId, List<PureRawData> datas)
        {
            using BinaryWriter writer = new(File.Open(PathFor(slotId, "bin"), FileMode.Create));
            writer.Write(datas.Count);
            DataSerializer.BytesSerialize(writer, datas);
        }

        /// <summary>
        /// Write text content to file
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="content">Text content to write</param>
        public static void Write(string slotId, string content) =>
            File.WriteAllText(PathFor(slotId, "sav"), content);

        /// <summary>
        /// Write binary content to file asynchronously
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="content">Binary content to write</param>
        /// <returns>Task representing the async operation</returns>
        public static async Task WriteAsync(string slotId, byte[] content) =>
            await File.WriteAllBytesAsync(PathFor(slotId, "bin"), content);

        /// <summary>
        /// Write text content to file asynchronously
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="content">Text content to write</param>
        /// <returns>Task representing the async operation</returns>
        public static async Task WriteAsync(string slotId, string content) =>
            await File.WriteAllTextAsync(PathFor(slotId, "sav"), content);

        /// <summary>
        /// Read binary data from file
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <returns>List of deserialized data objects</returns>
        public static List<PureRawData> ReadBytes(string slotId)
        {
            string filePath = PathFor(slotId, "bin");
            if (!File.Exists(filePath)) return new List<PureRawData>();
            
            using BinaryReader reader = new(File.Open(filePath, FileMode.Open));
            return DataSerializer.BinaryDeserialize<List<PureRawData>>(reader, true);
        }

        /// <summary>
        /// Read JSON text from file
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <returns>JSON string, null if file doesn't exist</returns>
        public static string ReadJson(string slotId) =>
            File.Exists(PathFor(slotId, "sav")) ? File.ReadAllText(PathFor(slotId, "sav")) : null;

        /// <summary>
        /// Read binary data from file asynchronously
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <returns>Task containing byte array, null if file doesn't exist</returns>
        public static async Task<byte[]> ReadBytesAsync(string slotId)
        {
            string p = PathFor(slotId, "bin");

            if (!File.Exists(p))
                return null;

            return await File.ReadAllBytesAsync(p);
        }

        /// <summary>
        /// Read JSON text from file asynchronously
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <returns>Task containing JSON string, null if file doesn't exist</returns>
        public static async Task<string> ReadJsonAsync(string slotId)
        {
            string p = PathFor(slotId, "sav");

            if (!File.Exists(p))
                return null;

            return await File.ReadAllTextAsync(p);
        }

        /// <summary>
        /// Check if a save file exists
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="extension">File extension</param>
        /// <returns>True if file exists, false otherwise</returns>
        public static bool Exists(string slotId, string extension) =>
            File.Exists(PathFor(slotId, extension));

        /// <summary>
        /// Delete a save file
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="extension">File extension</param>
        public static void Delete(string slotId, string extension)
        {
            string p = PathFor(slotId, extension);
            if (File.Exists(p)) File.Delete(p);
        }

        /// <summary>
        /// Write JSON data with header metadata
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="header">Header metadata</param>
        /// <param name="data">Save data</param>
        public static void WriteJsonWithHeader(string slotId, MetaData header, List<PureRawData> data)
        {
            string json = DataSerializer.JsonSerializeWithHeader(header, data);
            Write(slotId, json);
        }

        /// <summary>
        /// Read JSON data with header metadata
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <returns>SaveContainer with header and data, null if file doesn't exist</returns>
        public static SaveContainer ReadJsonWithHeader(string slotId)
        {
            string json = ReadJson(slotId);
            if (json == null) return null;
            return DataSerializer.DeserializeWithHeader(json);
        }

        /// <summary>
        /// Write binary data with header metadata
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <param name="header">Header metadata</param>
        /// <param name="data">Save data</param>
        public static void WriteBinariesWithHeader(string slotId, MetaData header, List<PureRawData> data)
        {
            using BinaryWriter writer = new(File.Open(PathFor(slotId, "bin"), FileMode.Create));

            byte[] headerBytes = DataSerializer.ToByteArray(header);
            writer.Write(headerBytes.Length);
            writer.Write(headerBytes);

            DataSerializer.BytesSerialize(writer, data);
        }

        /// <summary>
        /// Read header metadata from binary file
        /// </summary>
        /// <param name="slotId">Save slot identifier</param>
        /// <returns>MetaData object, null if file doesn't exist</returns>
        public static MetaData ReadBinaryHeader(string slotId)
        {
            string filePath = PathFor(slotId, "bin");
            if (!File.Exists(filePath)) return null;
            
            using BinaryReader reader = new(File.Open(filePath, FileMode.Open));

            int headerSize = reader.ReadInt32();
            byte[] headerBytes = reader.ReadBytes(headerSize);
            string headerJson = System.Text.Encoding.UTF8.GetString(headerBytes);

            return DataSerializer.Deserialize<MetaData>(headerJson);
        }
    }
}