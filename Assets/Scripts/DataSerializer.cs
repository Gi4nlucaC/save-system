using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

public static class DataSerializer
{

    #region JSON
    private static readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    public static List<PureRawData> Deserialize(string raw)
    {
        return JsonConvert.DeserializeObject<List<PureRawData>>(raw, _settings);
    }

    public static string JsonSerialize(List<PureRawData> data)
    {
        return JsonConvert.SerializeObject(data, _settings);
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

    public static List<PureRawData> BinaryDeserialize(BinaryReader reader)
    {
        int count = reader.ReadInt32();
        List<PureRawData> entities = new(count);

        for (int i = 0; i < count; i++)
        {
            byte id = reader.ReadByte();
            Type type = TypeById[id];

            if (type == null)
                throw new Exception("Nessun tipo registrato per id " + id);

            PureRawData entity = (PureRawData)Activator.CreateInstance(type);
            FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int j = 0; j < fields.Length; j++)
            {
                FieldInfo f = fields[j];

                if (f.FieldType == typeof(int))
                    f.SetValue(entity, reader.ReadInt32());
                else if (f.FieldType == typeof(float))
                    f.SetValue(entity, reader.ReadSingle());
                else if (f.FieldType == typeof(string))
                    f.SetValue(entity, reader.ReadString());
                else if (f.FieldType == typeof(Vector3Data))
                    f.SetValue(entity, reader.ReadVector3());
                else if (f.FieldType == typeof(QuaternionData))
                    f.SetValue(entity, reader.ReadQuaternion());
                else if (f.FieldType.IsEnum)
                {
                    Type underlying = Enum.GetUnderlyingType(f.FieldType);

                    object value = null;

                    if (underlying == typeof(byte))
                        value = reader.ReadByte();

                    f.SetValue(entity, Enum.ToObject(f.FieldType, value));
                }
                else
                    Debug.Log("Tipo non supportato: " + f.FieldType.Name);
                //throw new Exception("Tipo non supportato: " + f.FieldType.Name);
            }

            entities.Add(entity);
        }
        return entities;
    }

    public static void BytesSerialize(BinaryWriter writer, List<PureRawData> data)
    {
        foreach (var item in data)
        {
            var attr = (DataTypeIdAttribute)Attribute.GetCustomAttribute(item.GetType(), typeof(DataTypeIdAttribute));
            if (attr == null)
                throw new Exception("Entity senza EntityTypeId: " + item.GetType().Name);

            writer.Write(attr.Id); // scrive direttamente l'id
            FieldInfo[] fields = item.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < fields.Length; i++)
            {
                FieldInfo f = fields[i];

                if (f.FieldType == typeof(int))
                    writer.Write((int)f.GetValue(item));
                else if (f.FieldType == typeof(float))
                    writer.Write((float)f.GetValue(item));
                else if (f.FieldType == typeof(string))
                    writer.Write((string)f.GetValue(item) ?? "");
                else if (f.FieldType == typeof(Vector3Data))
                    writer.WriteVector3((Vector3Data)f.GetValue(item));
                else if (f.FieldType == typeof(QuaternionData))
                    writer.WriteQuaternion((QuaternionData)f.GetValue(item));
                else if (f.FieldType.IsEnum)
                {
                    Type underlying = Enum.GetUnderlyingType(f.FieldType);
                    if (underlying == typeof(byte))
                        writer.Write((byte)f.GetValue(item));
                }
                else
                    Debug.Log("Tipo non supportato: " + f.FieldType.Name);
                //throw new Exception("Tipo non supportato: " + f.FieldType.Name);
            }
        }
    }
    #endregion
}