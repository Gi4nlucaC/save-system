using System.Collections.Generic;

namespace SaveSystem
{
    /// <summary>
    /// Container that holds both header metadata and save data
    /// </summary>
    [System.Serializable]
    public class SaveContainer
    {
        /// <summary>Save file header with metadata</summary>
        public MetaData Header;
        
        /// <summary>List of all saved data objects</summary>
        public List<PureRawData> Data;
    }
}