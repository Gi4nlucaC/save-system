namespace SaveSystem
{
    /// <summary>
    /// Interface for objects that can be saved and loaded by the Save System
    /// </summary>
    public interface ISavable
    {
        /// <summary>
        /// Unique identifier for this savable object
        /// </summary>
        string PersistentId { get; }
        
        /// <summary>
        /// Save the current state of this object
        /// </summary>
        /// <returns>Raw data representing the object's state</returns>
        public PureRawData SaveData();
        
        /// <summary>
        /// Load data into this object
        /// </summary>
        public void LoadData();
        
        /// <summary>
        /// Delete saved data for this object
        /// </summary>
        public void DeleteData();
        
        /// <summary>
        /// Create a snapshot of current data without saving
        /// </summary>
        public void SnapshotData();
    }
}