using UnityEngine;

public class Character : MonoBehaviour, ISavable
{
    CharacterData _characterData;
    MovementComponent _movementComponent = new();

    public string SaveableId { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _movementComponent.HandleMovement();
    }
    void SnapshotData()
    {
        _characterData.UpdateData(transform.position, transform.rotation, transform.localScale);
    }

    public string SaveData()
    {
        SnapshotData();

        return JsonUtility.ToJson(_characterData);
    }

    public ISavableData LoadData(string data)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteData()
    {
        throw new System.NotImplementedException();
    }
}
