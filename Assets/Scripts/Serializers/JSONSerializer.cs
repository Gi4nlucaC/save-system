using System.Collections.Generic;
using Newtonsoft.Json;

public class JSONSerializer : ISerializer<string>
{
    private readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    public T Deserialize<T>(string raw) where T : List<EntityData>
    {
        return JsonConvert.DeserializeObject<T>(raw, _settings);
    }

    public string Serialize<T>(T data) where T : List<EntityData>
    {
        return JsonConvert.SerializeObject(data, _settings);
    }
}