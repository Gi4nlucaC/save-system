using System.Collections.Generic;

namespace SaveSystem
{
    /// <summary>
    /// Data container for cloud save storage
    /// </summary>
    [System.Serializable]
    public class CloudData
    {
        /// <summary>Name/identifier of the save slot</summary>
        public string nameSlot;
        
        /// <summary>Save file metadata</summary>
        public MetaData header;
        
        /// <summary>List of all saved data objects</summary>
        public List<PureRawData> values;
    }
}