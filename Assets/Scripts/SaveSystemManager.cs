using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{
    List<ISavableData> _savableItems = new();

    FileSaveStorage _fileSaveStorage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fileSaveStorage = new("Saves");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSaveData(string slotId)
    {
        StringBuilder stringBuilder = new();

        foreach (var item in _savableItems)
        {
            stringBuilder.Append(item.SaveData());
        }

        _fileSaveStorage.Write(slotId, stringBuilder.ToString());
    }
}
