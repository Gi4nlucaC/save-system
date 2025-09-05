using System;
using System.IO;
using UnityEngine;

public class SlotsManager : MonoBehaviour
{
    [SerializeField] SerializationMode _serializationType;
    [SerializeField] string _folderName;

    private string _lastSlotSaved;

    public string LastSlotSaved => _lastSlotSaved;

    private void Awake()
    {
        SaveSystemManager.Init(_serializationType);
        SaveStorage.Init(_folderName);

        _lastSlotSaved = GetLastSlotSaved();
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

        if (slots.Length == 0) return null;

        FileInfo last = slots[0];
        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i].LastWriteTime > last.LastWriteTime)
            {
                last = slots[i];
            }
        }

        return Path.GetFileNameWithoutExtension(last.Name);
    }
}