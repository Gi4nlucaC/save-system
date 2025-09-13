using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Static utility class for serializing and deserializing save data
    /// </summary>
    public static class DataSerializer
    {
        #region JSON
        
        /// <summary>
        /// JSON serialization settings
        /// </summary>
        private static readonly JsonSerializerSettings _settings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };

        /// <summary>
        /// Deserialize JSON string to object
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="raw">JSON string</param>
        /// <returns>Deserialized object</returns>
        public static T Deserialize<T>(string raw)
        {
            return JsonConvert.DeserializeObject<T>(raw, _settings);
        }

        /// <summary>
        /// Serialize object to JSON string
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="data">Object to serialize</param>
        /// <returns>JSON string</returns>
        public static string JsonSerialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, _settings);
        }

        /// <summary>
        /// Serialize header and data together to JSON
        /// </summary>
        /// <param name="header">Metadata header</param>
        /// <param name="data">Save data</param>
        /// <returns>JSON string containing both header and data</returns>
        public static string JsonSerializeWithHeader(MetaData header, List<PureRawData> data)
        {
            var container = new SaveContainer
            {
                Header = header,
                Data = data
            };
            return JsonConvert.SerializeObject(container, _settings);
        }

        /// <summary>
        /// Deserialize JSON string to SaveContainer
        /// </summary>
        /// <param name="raw">JSON string</param>
        /// <returns>SaveContainer with header and data</returns>
        public static SaveContainer DeserializeWithHeader(string raw)
        {
            return JsonConvert.DeserializeObject<SaveContainer>(raw, _settings);
        }

        #endregion

        #region BYTES

        /// <summary>
        /// Array mapping type IDs to actual types for binary serialization
        /// </summary>
        private static readonly Type[] TypeById = new Type[256];

        /// <summary>
        /// Static constructor to initialize type mappings
        /// </summary>
        static DataSerializer()
        {
            Type[] allTypes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (Type t in allTypes)
            {
                if (typeof(PureRawData).IsAssignableFrom(t) && !t.IsAbstract)
                {
                    object[] attrs = t.GetCustomAttributes(typeof(DataTypeIdAttribute), false);
                    if (attrs.Length > 0)
                    {
                        var attr = (DataTypeIdAttribute)attrs[0];
                        TypeById[attr.Id] = t;
                    }
                }
            }
        }

        /// <summary>
        /// Get all registered types for binary serialization
        /// </summary>
        /// <returns>Array of registered types</returns>
        public static Type[] GetTypesToRegister() => TypeById;

        /// <summary>
        /// Deserialize binary data to object
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="reader">Binary reader</param>
        /// <param name="hasHeader">Whether the data has a header</param>
        /// <returns>Deserialized object</returns>
        public static T BinaryDeserialize<T>(BinaryReader reader, bool hasHeader)
        {
            if (hasHeader)
            {
                int headerSize = reader.ReadInt32();
                reader.BaseStream.Seek(headerSize, SeekOrigin.Current);
            }

            int dataSize = reader.ReadInt32();
            byte[] dataBytes = reader.ReadBytes(dataSize);
            string dataJson = System.Text.Encoding.UTF8.GetString(dataBytes);

            if (!dataJson.StartsWith("{"))
            {
                dataJson = dataJson.Insert(0, "{\r\n");
            }

            return Deserialize<T>(dataJson);
        }

        /// <summary>
        /// Serialize object to binary format
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="writer">Binary writer</param>
        /// <param name="data">Object to serialize</param>
        /// <returns>Serialized byte array</returns>
        public static byte[] BytesSerialize<T>(BinaryWriter writer, T data)
        {
            byte[] dataBytes = ToByteArray(data);

            writer.Write(dataBytes.Length);
            writer.Write(dataBytes);

            return dataBytes;
        }

        /// <summary>
        /// Convert object to byte array
        /// </summary>
        /// <typeparam name="T">Source type</typeparam>
        /// <param name="data">Object to convert</param>
        /// <returns>Byte array representation</returns>
        public static byte[] ToByteArray<T>(T data)
        {
            string datas = JsonSerialize(data);
            return System.Text.Encoding.UTF8.GetBytes(datas);
        }
        
        #endregion
    }
}