using System.Collections.Generic;

public interface ISerializer<TFormat>
{
    TFormat Serialize<T>(T data) where T : List<EntityData>;
    T Deserialize<T>(TFormat raw) where T : List<EntityData>;
}