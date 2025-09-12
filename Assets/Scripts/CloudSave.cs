using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using PeraphsPizza.SaveSystem;

namespace PeraphsPizza.SaveSystem
{
    public static class CloudSave
    {
        private static IMongoDatabase _database;
        private static string _userId;
        private static List<CloudData> _cloudDatas;

        public static List<CloudData> CloudDatas => _cloudDatas;

        public static void Init(string username = "cloudsavesystem", string password = "vqppphJRNpG2vEIR", string clusterName = "modulicluster.h6cfk1e", string userId = "")
        {
            var client = new MongoClient($"mongodb+srv://{username}:{password}@{clusterName}.mongodb.net/");
            _database = client.GetDatabase("SaveSystem");
            _userId = userId;
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

                // Check that the type has not already been registered to avoid errors
                if (!BsonClassMap.IsClassMapRegistered(type))
                {
                    BsonClassMap.LookupClassMap(type); // Register type automatically
                }
            }
        }

        public static List<BsonDocument> LoadData()
        {
            var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");
            var sortDefinition = Builders<BsonDocument>.Sort.Ascending("_id");
            var docs = collection.Find(new BsonDocument()).Sort(sortDefinition).ToList();

            /* BsonDocument doc = docs[slotId];
            // Convert Bson -> JSON string
            string json = doc.ToJson(); */

            // Deserialize into C# object
            //return DataSerializer.Deserialize<List<CloudData>>(docs.ToJson());
            return docs;
        }

        public static List<CloudData> LoadBinariesData()
        {
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
            var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");

            var filter = Builders<BsonDocument>.Filter.Eq("nameSlot", data.nameSlot);

            var update = Builders<BsonDocument>.Update.Set($"values", data.values);

            // `IsUpsert = true` → if it doesn't find anything, create a new document
            var options = new UpdateOptions { IsUpsert = true };

            collection.UpdateOne(filter, update, options);
        }

        public static void SaveDataAsBinary(string slotId, byte[] header, byte[] data)
        {
            var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");

            var filter = Builders<BsonDocument>.Filter.Eq("nameSlot", slotId);

            var update = Builders<BsonDocument>.Update.Set($"header", new BsonBinaryData(header)).Set($"values", new BsonBinaryData(data));

            // `IsUpsert = true` → if it doesn't find anything, create a new document
            var options = new UpdateOptions { IsUpsert = true };

            collection.UpdateOne(filter, update, options);
        }
    }
}