using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Base MonoBehaviour class for objects that can be saved and loaded
    /// </summary>
    public class SavableMonoBehaviour : MonoBehaviour, ISavable
    {
        /// <summary>Persistent unique identifier for this object</summary>
        [SerializeField] protected UniqueGUID _persistentId;
        
        /// <summary>
        /// Get the persistent ID value
        /// </summary>
        public string PersistentId => _persistentId != null && _persistentId.IsValid ? _persistentId.Value : null;

#if UNITY_EDITOR
        /// <summary>
        /// Generate GUID when values change in editor
        /// </summary>
        [ExecuteInEditMode]
        private void OnValidate()
        {
            Generate();
        }

        /// <summary>
        /// Generate a new GUID if needed
        /// </summary>
        [ExecuteInEditMode]
        private void Generate()
        {
            if (_persistentId == null)
                _persistentId = new UniqueGUID();

            if (!_persistentId.IsValid)
                _persistentId.Set(System.Guid.NewGuid().ToString("N"));
        }
#endif

        /// <summary>
        /// Register this object with the save system
        /// </summary>
        protected void RegisterForSave()
        {
            if (!string.IsNullOrEmpty(PersistentId))
                SaveSystemManager.RegisterSavable(this);
            else
                Debug.LogWarning($"{name}: PersistentId not generated. Press 'Generate' on the UniqueGUID field or use the context menu.");
        }

        /// <summary>
        /// Save the current state of this object (must be implemented by subclasses)
        /// </summary>
        /// <returns>Serialized data representing this object's state</returns>
        public virtual PureRawData SaveData()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Load data into this object (must be implemented by subclasses)
        /// </summary>
        public virtual void LoadData()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Delete saved data for this object (must be implemented by subclasses)
        /// </summary>
        public virtual void DeleteData()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Create a snapshot of current data without saving (must be implemented by subclasses)
        /// </summary>
        public virtual void SnapshotData()
        {
            throw new System.NotImplementedException();
        }
    }
}