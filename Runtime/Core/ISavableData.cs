using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Interface for data objects that can be saved
    /// </summary>
    public interface ISavableData
    {
        /// <summary>
        /// Unique identifier for this savable data
        /// </summary>
        public string SaveableId { get; set; }
    }
}