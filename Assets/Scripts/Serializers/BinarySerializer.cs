using System.Collections.Generic;

public class BinarySerializer : ISerializer<byte[]>
{
    public T Deserialize<T>(byte[] raw) where T : List<EntityData>
    {
        throw new System.NotImplementedException();
    }

    public byte[] Serialize<T>(T data) where T : List<EntityData>
    {
        throw new System.NotImplementedException();
    }
}