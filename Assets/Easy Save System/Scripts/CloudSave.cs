using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace PizzaCompany.SaveSystem
{
    public static class CloudSave
    {
        private static IMongoDatabase _database;
        private static string _userId;
        private static List<CloudData> _cloudDatas;

        public static List<CloudData> CloudDatas => _cloudDatas;

        public struct CloudConfig
        {
            public string Username;
            public string Password;
            public string ClusterName;
            public string DatabaseName;
            public string UserId;
        }

        /// <summary>
        /// Initializes cloud save with provided configuration. Credentials are NOT stored permanently.
        /// </summary>
        public static void Init(CloudConfig config)
        {
            if (string.IsNullOrEmpty(config.Username) || string.IsNullOrEmpty(config.Password) || string.IsNullOrEmpty(config.ClusterName))
            {
                UnityEngine.Debug.LogWarning("CloudSave Init skipped: missing credentials.");
                _cloudDatas = new();
                return;
            }

            var client = new MongoClient($"mongodb+srv://{config.Username}:{config.Password}@{config.ClusterName}.mongodb.net/");
            _database = client.GetDatabase(string.IsNullOrEmpty(config.DatabaseName) ? "SaveSystem" : config.DatabaseName);
            _userId = config.UserId;
            RegisterClassMap();

            _cloudDatas = LoadBinariesData();
        }

        private static void RegisterClassMap()
        {
            var typesToRegister = DataSerializer.GetTypesToRegister();
            for (int i = 0; i < typesToRegister.Length; i++)
            {
                Type type = typesToRegister[i];
                if (type == null) break;
                if (!BsonClassMap.IsClassMapRegistered(type))
                {
                    BsonClassMap.LookupClassMap(type);
                }
            }
        }

        public static List<BsonDocument> LoadData()
        {
            EnsureInitialized();
            var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");
            var sortDefinition = Builders<BsonDocument>.Sort.Ascending("_id");
            return collection.Find(new BsonDocument()).Sort(sortDefinition).ToList();
        }

        public static List<CloudData> LoadBinariesData()
        {
            EnsureInitialized();
            var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");
            var sortDefinition = Builders<BsonDocument>.Sort.Ascending("nameSlot");
            var docs = collection.Find(new BsonDocument()).Sort(sortDefinition).ToList();

            List<CloudData> datas = new();

            foreach (var item in docs)
            {
                CloudData data = new()
                {
                    nameSlot = item.GetElement("nameSlot").Value.AsString
                };

                var headerBytes = item.GetElement("header").Value.AsBsonBinaryData.Bytes;
                using MemoryStream headerStream = new(headerBytes);
                using BinaryReader headerReader = new(headerStream);
                data.header = DataSerializer.BinaryDeserialize<MetaData>(headerReader, false);

                var dataBytes = item.GetElement("values").Value.AsBsonBinaryData.Bytes;
                using MemoryStream dataStream = new(dataBytes);
                using BinaryReader dataReader = new(dataStream);
                data.values = DataSerializer.BinaryDeserialize<List<PureRawData>>(dataReader, false);

                datas.Add(data);
            }

            return datas;
        }

        public static void SaveData(CloudData data)
        {
            EnsureInitialized();
            var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");
            var filter = Builders<BsonDocument>.Filter.Eq("nameSlot", data.nameSlot);
            var update = Builders<BsonDocument>.Update.Set("values", data.values);
            var options = new UpdateOptions { IsUpsert = true };
            collection.UpdateOne(filter, update, options);
        }

        public static void SaveDataAsBinary(string slotId, byte[] header, byte[] data)
        {
            EnsureInitialized();
            var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");
            var filter = Builders<BsonDocument>.Filter.Eq("nameSlot", slotId);
            var update = Builders<BsonDocument>.Update.Set("header", new BsonBinaryData(header)).Set("values", new BsonBinaryData(data));
            var options = new UpdateOptions { IsUpsert = true };
            collection.UpdateOne(filter, update, options);
        }

        private static void EnsureInitialized()
        {
            if (_database == null)
                throw new InvalidOperationException("CloudSave not initialized. Call CloudSave.Init with valid CloudConfig first.");
        }
    }
}