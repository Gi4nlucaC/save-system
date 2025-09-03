using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public static class SaveSystemManager
{

    static List<ISavable> _savableItems = new();
    static List<PureRawData> _savedEntities = new();

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
        List<PureRawData> datas = new();

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

        if (loadedString == null) return;

        _savedEntities = _dataSerializer.Deserialize(loadedString);
    }

    public static int ExistData(string id)
    {
        for (int i = 0; i < _savedEntities.Count; i++)
        {
            PureRawData item = _savedEntities[i];
            if (item._id == id)
                return i;
        }

        return -1;
    }

    public static PureRawData GetData(string id)
    {
        int index = ExistData(id);
        if (index >= 0)
            return _savedEntities[index];

        return null;
    }

    public static void DeleteData(string id)
    {

    }
}
