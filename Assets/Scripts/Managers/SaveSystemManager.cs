using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

public static class SaveSystemManager
{

    static List<ISavable> _savableItems = new();
    static List<PureRawData> _savedEntities = new();

    static SerializationMode _serializationMode;

    public static event Action OnAllSavablesLoaded;

    public static event Action OnAutoSave;

    private static readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Init(SerializationMode mode)
    {
        _serializationMode = mode;
        OnLoadData("firstTest");
    }

    public static void RegisterSavable(ISavable savable)
    {
        if (savable != null && !_savableItems.Contains(savable))
        {
            _savableItems.Add(savable);
        }
    }

    public static void LoadAllSavable()
    {
        foreach (var savedItem in _savableItems)
        {
            savedItem.LoadData();
        }

        OnAllSavablesLoaded?.Invoke();
    }

    public static void UnregisterSavable(ISavable savable)
    {
        _savableItems.Remove(savable);
    }

    public static void OnSaveData(string slotId)
    {
        List<PureRawData> datas = new();

        if (_serializationMode == SerializationMode.Json)
        {
            for (int i = 0; i < _savableItems.Count; i++)
            {
                ISavable item = _savableItems[i];
                datas.Add(item.SaveData());
            }

            SaveStorage.Write(slotId, DataSerializer.JsonSerialize(datas));
        }
        else
        {
            SaveStorage.Write(slotId, datas);
        }
    }

    public static void OnLoadData(string slotId)
    {
        if (_serializationMode == SerializationMode.Json)
        {
            var loadedString = SaveStorage.ReadJson(slotId);

            if (loadedString == null) return;

            _savedEntities = DataSerializer.Deserialize(loadedString);
        }
        else
        {
            _savedEntities = SaveStorage.ReadBytes(slotId);
        }
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

    public static void AutoSave(string id)
    {
        OnAutoSave?.Invoke();

        OnSaveData(id);
    }
}
