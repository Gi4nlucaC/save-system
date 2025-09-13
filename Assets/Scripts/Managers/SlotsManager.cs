using UnityEngine;

namespace PizzaCompany.SaveSystem
{
    public class SlotsManager : MonoBehaviour
    {
        [SerializeField] UniqueGUID _userId;
        [SerializeField] bool _saveInCloud;
        [SerializeField] CloudSaveCredentials _cloudCredentials; // secure asset (not versioned)
        [SerializeField] string _databaseName = "SaveSystem";
        [SerializeField] SerializationMode _serializationType;
        [SerializeField] string _folderName = "Saves";

        string _lastSlotSaved;
        public string LastSlotSaved => _lastSlotSaved;

        void Awake()
        {
            if (_saveInCloud)
            {
                var cfg = ResolveCloudConfig();
                if (!string.IsNullOrEmpty(cfg.Username) && !string.IsNullOrEmpty(cfg.Password) && !string.IsNullOrEmpty(cfg.ClusterName))
                {
                    CloudSave.Init(cfg);
                }
#if UNITY_EDITOR
                else
                {
                    Debug.LogWarning("[SaveSystem] Cloud credentials missing. CloudSave disabled.");
                }
#endif
            }

            SaveSystemManager.Init(_serializationType, _saveInCloud);
            SaveStorage.Init(_folderName);
            _lastSlotSaved = SaveSystemManager.GetLastSlotSaved();
        }

        CloudSave.CloudConfig ResolveCloudConfig()
        {
            // Priority: ScriptableObject > Environment Variables > empty (disabled)
            if (_cloudCredentials != null && _cloudCredentials.HasValidCredentials)
            {
                return new CloudSave.CloudConfig
                {
                    Username = _cloudCredentials.Username,
                    Password = _cloudCredentials.Password,
                    ClusterName = _cloudCredentials.ClusterName,
                    DatabaseName = _databaseName,
                    UserId = _userId != null ? _userId.Value : string.Empty
                };
            }

            // Environment variables (for CI/CD, Cloud Build, etc.)
            string envUser = System.Environment.GetEnvironmentVariable("SAVE_SYSTEM_DB_USER");
            string envPass = System.Environment.GetEnvironmentVariable("SAVE_SYSTEM_DB_PASS");
            string envCluster = System.Environment.GetEnvironmentVariable("SAVE_SYSTEM_DB_CLUSTER");
            if (!string.IsNullOrEmpty(envUser) && !string.IsNullOrEmpty(envPass) && !string.IsNullOrEmpty(envCluster))
            {
                return new CloudSave.CloudConfig
                {
                    Username = envUser,
                    Password = envPass,
                    ClusterName = envCluster,
                    DatabaseName = _databaseName,
                    UserId = _userId != null ? _userId.Value : string.Empty
                };
            }

            return default; // empty -> skipped
        }

#if UNITY_EDITOR
        [ExecuteInEditMode]
        void OnValidate() { Generate(); }
        void Generate()
        {
            if (_userId == null) _userId = new UniqueGUID();
            if (!_userId.IsValid) _userId.Set(System.Guid.NewGuid().ToString("N"));
        }
#endif
    }
}