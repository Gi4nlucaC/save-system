using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public static class SaveSystemManager
{

    static List<ISavable> _savableItems = new();
    static List<EntityData> _savedEntities;

    static SaveStorage _saveStorage;
    static DataSerializer _dataSerializer;

    private static readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Init()
    {
        _saveStorage = new("Saves");
        _dataSerializer = new();
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
        List<EntityData> datas = new();

        foreach (var item in _savableItems)
        {
            /* var x = item.SaveData();
            stringBuilder.Append(x); */
            datas.Add(item.SaveData());
        }

        _saveStorage.Write(slotId, _dataSerializer.JsonSerialize(datas));
    }

    public static void OnLoadData(string slotId)
    {
        var loadedString = _saveStorage.ReadJson(slotId);
        _savedEntities = _dataSerializer.Deserialize(loadedString);
    }
}
