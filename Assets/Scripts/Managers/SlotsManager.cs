using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] UniqueGUID _userId;
    [SerializeField] bool _saveInCloud;
    [SerializeField] private string _username;
    [SerializeField] private string _password;
    [SerializeField] private string _clusterName;
    [SerializeField] SerializationMode _serializationType;
    [SerializeField] string _folderName;

    private string _lastSlotSaved;

    public string LastSlotSaved => _lastSlotSaved;

    public bool SaveInCloud => _saveInCloud;

    private void Awake()
    {
        if (_saveInCloud)
            CloudSave.Init(userId: _userId.Value);

        SaveSystemManager.Init(_serializationType, _saveInCloud);
        SaveStorage.Init(_folderName);

        _lastSlotSaved = SaveSystemManager.GetLastSlotSaved();
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void OnValidate()
    {
        Generate();
    }

    [ExecuteInEditMode]
    private void Generate()
    {
        if (_userId == null)
            _userId = new UniqueGUID();

        if (!_userId.IsValid)
            _userId.Set(System.Guid.NewGuid().ToString("N"));
    }
#endif

}