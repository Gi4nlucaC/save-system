using System.Collections.Generic;

namespace PizzaCompany.SaveSystem
{
    [System.Serializable]
    public class SaveContainer
    {
        public MetaData Header;
        public List<PureRawData> Data;
    }
}