using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Base class for all savable data objects
    /// </summary>
    [System.Serializable]
    [DataTypeId(0)]
    public class PureRawData
    {
        /// <summary>
        /// Unique identifier for this data object
        /// </summary>
        public string _id;
    }
}