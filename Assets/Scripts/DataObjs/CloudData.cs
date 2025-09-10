using System.Collections.Generic;
using MongoDB.Bson;

public class CloudData
{
    public string nameSlot;
    public MetaData header;
    public List<PureRawData> values;
}