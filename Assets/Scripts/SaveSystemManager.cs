using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class SaveSystemManager
{

    static List<ISavable> _savableItems = new();


    static FileSaveStorage _fileSaveStorage;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Init()
    {
        _fileSaveStorage = new("Saves");
    }

    public static void RegisterSavable(ISavable savable)
    {
        if (savable != null && !_savableItems.Contains(savable))
        {
            _savableItems.Add(savable);
        }
    }

    public static void UnregisterSavable(ISavable savable)
    {
        _savableItems.Remove(savable);
    }

    public static void OnSaveData(string slotId)
    {
        StringBuilder stringBuilder = new();

        foreach (var item in _savableItems)
        {
            stringBuilder.Append(item.SaveData());
        }

        _fileSaveStorage.Write(slotId, stringBuilder.ToString());
    }
}
