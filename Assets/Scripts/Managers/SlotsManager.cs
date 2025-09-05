using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] SerializationMode _serializationType;
    [SerializeField] string _folderName;

    private void Start()
    {
        SaveSystemManager.Init(_serializationType);
        SaveStorage.Init(_folderName);
    }

    public string[] GetSlotNames()
    {
        if (_serializationType == SerializationMode.Json)
            return SaveStorage.CheckSaves("sav");
        else
            return SaveStorage.CheckSaves("bin");
    }
}