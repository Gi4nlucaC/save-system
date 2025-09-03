using System.Collections.Generic;
using Newtonsoft.Json;

public class DataSerializer
{
    private readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    public List<PureRawData> Deserialize(string raw)
    {
        return JsonConvert.DeserializeObject<List<PureRawData>>(raw, _settings);
    }

    public string JsonSerialize(List<PureRawData> data)
    {
        return JsonConvert.SerializeObject(data, _settings);
    }

    public List<PureRawData> Deserialize(byte[] raw)
    {
        throw new System.NotImplementedException();
    }

    public byte[] BytesSerialize(List<PhysicalEntityData> data)
    {
        throw new System.NotImplementedException();
    }
}