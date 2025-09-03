using UnityEngine;

public class SavableMonoBehaviour : MonoBehaviour
{
    [SerializeField] private UniqueGUID _persistentId;
    public string PersistentId => _persistentId != null && _persistentId.IsValid ? _persistentId.Value : null;

}
