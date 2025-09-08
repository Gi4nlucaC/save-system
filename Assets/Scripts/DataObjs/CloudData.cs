using System.Collections.Generic;
using MongoDB.Bson;

public class CloudData
{
    public ObjectId id;
    public string type;
    public List<PureRawData> values;
}