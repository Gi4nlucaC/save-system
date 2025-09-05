using System.IO;
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

    public FileInfo[] GetSlotInfos()
    {
        if (_serializationType == SerializationMode.Json)
            return SaveStorage.CheckSaves("sav");
        else
            return SaveStorage.CheckSaves("bin");
    }

    public string GetLastSlotSaved()
    {
        var slots = GetSlotInfos();
        FileInfo last = slots[0];
        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i].LastWriteTime > last.LastWriteTime)
            {
                last = slots[i];
            }
        }

        return last.FullName;
    }
}