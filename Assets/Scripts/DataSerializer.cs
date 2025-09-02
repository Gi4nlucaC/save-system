using System.Collections.Generic;
using Newtonsoft.Json;

public class DataSerializer
{
    private readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    public List<EntityData> Deserialize(string raw)
    {
        return JsonConvert.DeserializeObject<List<EntityData>>(raw, _settings);
    }

    public string JsonSerialize(List<EntityData> data)
    {
        return JsonConvert.SerializeObject(data, _settings);
    }

    public List<EntityData> Deserialize(byte[] raw)
    {
        throw new System.NotImplementedException();
    }

    public byte[] BytesSerialize(List<EntityData> data)
    {
        throw new System.NotImplementedException();
    }
}