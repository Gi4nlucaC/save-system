using System.Collections.Generic;

namespace PeraphsPizza.SaveSystem
{
    [System.Serializable]
    public class SaveContainer
    {
        public MetaData Header;
        public List<PureRawData> Data;
    }
}