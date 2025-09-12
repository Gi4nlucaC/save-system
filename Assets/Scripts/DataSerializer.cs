using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MongoDB.Driver;
using Newtonsoft.Json;
using UnityEngine;
using PeraphsPizza.SaveSystem;

namespace PeraphsPizza.SaveSystem
{
    public static class DataSerializer
    {

        #region JSON
        private static readonly JsonSerializerSettings _settings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };

        public static T Deserialize<T>(string raw)
        {
            return JsonConvert.DeserializeObject<T>(raw, _settings);
        }

        public static string JsonSerialize<T>(T data)
        {
            return JsonConvert.SerializeObject(data, _settings);
        }

        public static string JsonSerializeWithHeader(MetaData header, List<PureRawData> data)
        {
            var container = new SaveContainer
            {
                Header = header,
                Data = data
            };
            return JsonConvert.SerializeObject(container, _settings);
        }

        public static SaveContainer DeserializeWithHeader(string raw)
        {
            return JsonConvert.DeserializeObject<SaveContainer>(raw, _settings);
        }

        #endregion

        #region BYTES

        private static readonly Type[] TypeById = new Type[256];

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

        public static Type[] GetTypesToRegister() => TypeById;

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

        public static byte[] BytesSerialize<T>(BinaryWriter writer, T data)
        {
            byte[] dataBytes = ToByteArray(data);

            writer.Write(dataBytes.Length);
            writer.Write(dataBytes);

            return dataBytes;
        }

        public static byte[] ToByteArray<T>(T data)
        {
            string datas = JsonSerialize(data);
            return System.Text.Encoding.UTF8.GetBytes(datas);
        }
        #endregion
    }
}