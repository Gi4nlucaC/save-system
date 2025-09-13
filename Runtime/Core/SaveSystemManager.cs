using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Central manager for the save system, handling registration of savable objects and save/load operations
    /// </summary>
    public static class SaveSystemManager
    {
        #region Private Fields
        
        /// <summary>List of all registered savable objects</summary>
        static List<ISavable> _savableItems = new();
        
        /// <summary>Cache of loaded save data</summary>
        static List<PureRawData> _savedEntities = new();

        /// <summary>Current serialization mode</summary>
        static SerializationMode _serializationMode;
        
        /// <summary>Whether to save data in the cloud</summary>
        static bool _saveInCloud;

        /// <summary>JSON serialization settings</summary>
        private static readonly JsonSerializerSettings _settings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };
        
        #endregion

        #region Events
        
        /// <summary>Triggered when all savable objects have been loaded</summary>
        public static event Action OnAllSavablesLoaded;

        /// <summary>Triggered when an auto-save occurs</summary>
        public static event Action OnAutoSave;

        /// <summary>Triggered when a manual save is completed</summary>
        public static event Action OnGameSavedManually;
        
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Initialize the save system with specified configuration
        /// </summary>
        /// <param name="mode">Serialization mode to use</param>
        /// <param name="saveInCloud">Whether to save data in the cloud</param>
        public static void Init(SerializationMode mode, bool saveInCloud)
        {
            _serializationMode = mode;
            _saveInCloud = saveInCloud;
        }

        /// <summary>
        /// Register a savable object with the save system
        /// </summary>
        /// <param name="savable">Object implementing ISavable</param>
        public static void RegisterSavable(ISavable savable)
        {
            if (savable != null && !_savableItems.Contains(savable))
            {
                _savableItems.Add(savable);
            }
        }

        /// <summary>
        /// Load data into all registered savable objects
        /// </summary>
        public static void LoadAllSavable()
        {
            foreach (var savedItem in _savableItems)
            {
                savedItem.LoadData();
            }

            OnAllSavablesLoaded?.Invoke();
        }

        /// <summary>
        /// Unregister a savable object from the save system
        /// </summary>
        /// <param name="savable">Object to unregister</param>
        public static void UnregisterSavable(ISavable savable)
        {
            _savableItems.Remove(savable);
        }

        /// <summary>
        /// Reset the save system, clearing all registered objects
        /// </summary>
        public static void OnResetGame()
        {
            _savableItems.Clear();
        }

        /// <summary>
        /// Save data for all registered objects to specified slot
        /// </summary>
        /// <param name="slotId">Unique identifier for the save slot</param>
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

        /// <summary>
        /// Load data from specified slot
        /// </summary>
        /// <param name="slotId">Unique identifier for the save slot</param>
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

        /// <summary>
        /// Load data directly from cloud data
        /// </summary>
        /// <param name="data">List of save data from cloud</param>
        public static void OnCloudLoadData(List<PureRawData> data)
        {
            _savedEntities = data;
        }

        /// <summary>
        /// Check if data exists for given ID
        /// </summary>
        /// <param name="id">ID to check</param>
        /// <returns>Index of data if found, -1 otherwise</returns>
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

        /// <summary>
        /// Get saved data by ID
        /// </summary>
        /// <param name="id">ID of data to retrieve</param>
        /// <returns>Data object if found, null otherwise</returns>
        public static PureRawData GetData(string id)
        {
            int index = ExistData(id);
            if (index >= 0)
                return _savedEntities[index];

            return null;
        }

        /// <summary>
        /// Delete saved data by ID
        /// </summary>
        /// <param name="id">ID of data to delete</param>
        public static void DeleteData(string id)
        {
            // TODO: Implement delete functionality
        }

        /// <summary>
        /// Perform an auto-save
        /// </summary>
        /// <param name="id">Save slot ID</param>
        public static void AutoSave(string id)
        {
            OnAutoSave?.Invoke();
            OnSaveData(id);
        }

        /// <summary>
        /// Get information about all save slots
        /// </summary>
        /// <returns>Array of file information for save slots</returns>
        public static FileInfo[] GetSlotInfos()
        {
            if (_serializationMode == SerializationMode.Json)
                return SaveStorage.CheckSaves("sav");
            else
                return SaveStorage.CheckSaves("bin");
        }

        /// <summary>
        /// Get the ID of the most recently saved slot
        /// </summary>
        /// <returns>Slot ID of most recent save, null if no saves exist</returns>
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

        /// <summary>
        /// Get metadata for all save slots
        /// </summary>
        /// <returns>List of metadata for each save slot</returns>
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

        /// <summary>
        /// Build a formatted date string from metadata
        /// </summary>
        /// <param name="meta">Metadata to format</param>
        /// <returns>Formatted date string</returns>
        public static string BuildDateString(MetaData meta)
        {
            if (meta == null)
                return string.Empty;

            return $"{meta.Day} - {meta.Hours:D2}:{meta.Minutes:D2}";
        }

        /// <summary>
        /// Format play time seconds into readable string
        /// </summary>
        /// <param name="playTimeSeconds">Play time in seconds</param>
        /// <returns>Formatted time string (HH:MM:SS)</returns>
        public static string FormatPlayTime(double playTimeSeconds)
        {
            var ts = TimeSpan.FromSeconds(playTimeSeconds);
            return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
        }
        
        #endregion
    }
}