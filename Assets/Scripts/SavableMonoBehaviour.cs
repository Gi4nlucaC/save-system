using UnityEngine;

public class SavableMonoBehaviour : MonoBehaviour, ISavable
{
    [SerializeField] protected UniqueGUID _persistentId;
    public string PersistentId => _persistentId != null && _persistentId.IsValid ? _persistentId.Value : null;

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void OnValidate()
    {
        Generate();
    }

    [ExecuteInEditMode]
    private void Generate()
    {
        if (_persistentId == null)
            _persistentId = new UniqueGUID();

        if (!_persistentId.IsValid)
            _persistentId.Set(System.Guid.NewGuid().ToString("N"));
    }
#endif

    protected void Initialize()
    {
        LoadData();

        if (!string.IsNullOrEmpty(PersistentId))
            SaveSystemManager.RegisterSavable(this);
        else
            Debug.LogWarning($"{name}: PersistentId non generato. Premi 'Generate' sul campo UniqueGUID o usa il context menu.");
    }

    public virtual PureRawData SaveData()
    {
        throw new System.NotImplementedException();
    }

    public virtual void LoadData()
    {
        throw new System.NotImplementedException();
    }

    public virtual void DeleteData()
    {
        throw new System.NotImplementedException();
    }

    public virtual void SnapshotData()
    {
        throw new System.NotImplementedException();
    }


}
