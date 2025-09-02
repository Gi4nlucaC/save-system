using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public static class SaveSystemManager
{

    static List<ISavable> _savableItems = new();


    static FileSaveStorage _fileSaveStorage;

    private static readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Init()
    {
        _fileSaveStorage = new("Saves");
        OnLoadData("firstTest");
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
            var x = item.SaveData();
            stringBuilder.Append(x);
        }

        _fileSaveStorage.Write(slotId, stringBuilder.ToString());
    }

    public static void OnLoadData(string slotId)
    {
        var loadedString = _fileSaveStorage.Read(slotId);
        var x = JsonConvert.DeserializeObject<GameData>(loadedString, _settings);
    }
}
