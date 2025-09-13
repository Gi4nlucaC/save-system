using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Manager component for configuring and initializing the save system
    /// </summary>
    public class SlotsManager : MonoBehaviour
    {
        [Header("Identity")]
        [SerializeField] private UniqueGUID _userId;
        
        [Header("Storage Configuration")]
        [SerializeField] private bool _saveInCloud;
        [SerializeField] private string _folderName = "SaveData";
        
        [Header("Cloud Settings")]
        [SerializeField] private string _username;
        [SerializeField] private string _password;
        [SerializeField] private string _clusterName;
        
        [Header("Local Settings")]
        [SerializeField] private SerializationMode _serializationType = SerializationMode.Json;

        private string _lastSlotSaved;

        /// <summary>
        /// Get the ID of the last saved slot
        /// </summary>
        public string LastSlotSaved => _lastSlotSaved;

        private void Awake()
        {
            InitializeSaveSystem();
        }

        /// <summary>
        /// Initialize the save system with current configuration
        /// </summary>
        private void InitializeSaveSystem()
        {
            if (_saveInCloud)
            {
                CloudSave.Init(
                    username: _username, 
                    password: _password, 
                    clusterName: _clusterName, 
                    userId: _userId.Value
                );
            }

            SaveSystemManager.Init(_serializationType, _saveInCloud);
            SaveStorage.Init(_folderName);

            _lastSlotSaved = SaveSystemManager.GetLastSlotSaved();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Generate GUID when values change in editor
        /// </summary>
        [ExecuteInEditMode]
        private void OnValidate()
        {
            GenerateGuidIfNeeded();
        }

        /// <summary>
        /// Generate a new GUID if one doesn't exist
        /// </summary>
        [ExecuteInEditMode]
        private void GenerateGuidIfNeeded()
        {
            if (_userId == null)
                _userId = new UniqueGUID();

            if (!_userId.IsValid)
                _userId.Set(System.Guid.NewGuid().ToString("N"));
        }
#endif
    }
}