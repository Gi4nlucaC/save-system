using System.Collections.Generic;
using MongoDB.Bson;

namespace PeraphsPizza.SaveSystem
{
    public class CloudData
    {
        public string nameSlot;
        public MetaData header;
        public List<PureRawData> values;
    }
}