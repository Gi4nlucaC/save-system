using UnityEngine;

public class SavableMonoBehaviour : MonoBehaviour
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


}
