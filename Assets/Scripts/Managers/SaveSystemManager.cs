using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using PeraphsPizza.SaveSystem;

namespace PeraphsPizza.SaveSystem
{
    public static class SaveSystemManager
    {
        static List<ISavable> _savableItems = new();
        static List<PureRawData> _savedEntities = new();

        static SerializationMode _serializationMode;
        static bool _saveInCloud;

        public static event Action OnAllSavablesLoaded;

        public static event Action OnAutoSave;

        public static event Action OnGameSavedManually;

        private static readonly JsonSerializerSettings _settings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public static void Init(SerializationMode mode, bool saveInCloud)
        {
            _serializationMode = mode;
            _saveInCloud = saveInCloud;
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

        public static void OnResetGame()
        {
            _savableItems.Clear();
        }

        public static void OnSaveData(string slotId)
        {

            List<PureRawData> allData = new();
            MetaData header = new()
            {
                SlotId = slotId,
                PlayerName = "Unknown",
                PlayTimeSeconds = 0,
                Day = Days.Monday,
                Hours = 0,
                Minutes = 0
            };

            foreach (var savable in _savableItems)
            {
                // save data
                var data = savable.SaveData();
                allData.Add(data);

                if (savable is IHeaderSavable headerSavable)
                {
                    var metaPart = headerSavable.GetMetaDataPart();
                    if (metaPart != null)
                        header.Merge(metaPart);
                }
            }

            if (_saveInCloud)
            {
                CloudSave.SaveDataAsBinary(slotId,
                    DataSerializer.ToByteArray<MetaData>(header),
                    DataSerializer.ToByteArray<List<PureRawData>>(allData));
            }
            else
            {
                if (_serializationMode == SerializationMode.Json)
                    SaveStorage.WriteJsonWithHeader(slotId, header, allData);
                else
                    SaveStorage.WriteBinariesWithHeader(slotId, header, allData);
            }

            OnGameSavedManually?.Invoke();
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
            List<MetaData> headers = new();

            if (_saveInCloud)
            {
                foreach (var item in CloudSave.CloudDatas)
                {
                    headers.Add(item.header);
                }
            }
            else
            {
                var saveFiles = GetSlotInfos();

                // Read all headers from files
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
                        header = SaveStorage.ReadBinaryHeader(slotId);
                    }

                    if (header != null)
                        headers.Add(header);
                }
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
}
