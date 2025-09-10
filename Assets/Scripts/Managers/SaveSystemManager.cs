using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class SaveSystemManager
{
    static List<ISavable> _savableItems = new();
    static List<PureRawData> _savedEntities = new();

    static SerializationMode _serializationMode;

    public static event Action OnAllSavablesLoaded;

    public static event Action OnAutoSave;

    public static event Action OnGameSavedManually;

    private static readonly JsonSerializerSettings _settings = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Formatting = Formatting.Indented
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static void Init(SerializationMode mode)
    {
        _serializationMode = mode;
        //OnLoadData("firstTest");
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
        CloudData datas = new()
        {
            nameSlot = slotId,
            values = new()
        };

        for (int i = 0; i < _savableItems.Count; i++)
        {
            ISavable savable = _savableItems[i];
            datas.values.Add(savable.SaveData());
        }

        if (_serializationMode == SerializationMode.Json)
        {
            // Costruisci i dati da salvare
            List<PureRawData> datas = new();
            for (int i = 0; i < _savableItems.Count; i++)
                datas.Add(_savableItems[i].SaveData());

            // Costruisci un header MetaData
            MetaData header = new MetaData
            {
                SlotId = slotId,
                PlayerName = "Unknown",
                PlayTimeSeconds = 0,
                Day = Days.Monday,
                Hours = 0,
                Minutes = 0
            };

            int canBreak = 0;
            foreach (var data in datas)
            {
                if (data is CharacterData character && !string.IsNullOrEmpty(character._name))
                {
                    header.PlayerName = character._name;
                    canBreak++;
                }
                if (data is TimeDateData time)
                {
                    header.Day = time._day;
                    header.Hours = time._hours;
                    header.Minutes = time._minutes;
                    canBreak++;
                }
                if (canBreak == 2) break;
            }

            SaveStorage.WriteJsonWithHeader(slotId, header, datas);
            OnGameSavedManually?.Invoke();
        }
        else
        {

            MetaData header = new MetaData
            {
                SlotId = slotId,
                PlayerName = "Unknown",
                PlayTimeSeconds = 0,
                Day = Days.Monday,
                Hours = 0,
                Minutes = 0
            };

            int canBreak = 0;

            foreach (var data in datas.values)
            {
                if (data is CharacterData character && !string.IsNullOrEmpty(character._name))
                {
                    header.PlayerName = character._name;
                    canBreak++;
                }

                if (data is TimeDateData time)
                {
                    header.Day = time._day;
                    header.Hours = time._hours;
                    header.Minutes = time._minutes;

                    canBreak++;
                }

                if (canBreak == 2) break;
            }

            SaveStorage.WriteWithHeader(slotId, header, datas);

            OnGameSavedManually?.Invoke();
        }
    }

    public static void OnLoadData(string slotId)
    {
        if (_serializationMode == SerializationMode.Json)
        {
            var container = SaveStorage.ReadJsonWithHeader(slotId);

            if (container == null) return;

            _savedEntities = container.Data;
            MetaData header = container.Header;

        }
        else
        {
            _savedEntities = SaveStorage.ReadBytes(slotId);
        }
    }

    public static void OnCloudLoadData(List<PureRawData> data)
    {
        _savedEntities = data;
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

    public static FileInfo[] GetSlotInfos()
    {
        if (_serializationMode == SerializationMode.Json)
            return SaveStorage.CheckSaves("sav");
        else
            return SaveStorage.CheckSaves("bin");
    }

    public static string GetLastSlotSaved()
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

    public static List<MetaData> GetSlotMetaInfo()
    {
        var saveFiles = GetSlotInfos();

        List<MetaData> headers = new List<MetaData>();

        // Leggi tutti gli header dai file
        foreach (var file in saveFiles)
        {
            string slotId = Path.GetFileNameWithoutExtension(file.Name);
            MetaData header = null;
            if (_serializationMode == SerializationMode.Json)
            {
                var container = SaveStorage.ReadJsonWithHeader(slotId);
                if (container != null) header = container.Header;
            }
            else
            {
                header = SaveStorage.ReadHeader(slotId);
            }

            if (header != null)
                headers.Add(header);
        }

            return headers;
    }

    public static string BuildDateString(MetaData meta)
    {
        if (meta == null)
            return string.Empty;

        return $"{meta.Day} - {meta.Hours:D2}:{meta.Minutes:D2}";
    }
    public static string FormatPlayTime(double playTimeSeconds)
    {
        var ts = TimeSpan.FromSeconds(playTimeSeconds);
        return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
    }
}
