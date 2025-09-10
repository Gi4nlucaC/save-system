using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

public static class CloudSave
{
    private static IMongoDatabase _database;
    private static string _userId;

    public static void Init(string username = "cloudsavesystem", string password = "vqppphJRNpG2vEIR", string clusterName = "modulicluster.h6cfk1e", string userId = "")
    {
        var client = new MongoClient($"mongodb+srv://{username}:{password}@{clusterName}.mongodb.net/");
        _database = client.GetDatabase("SaveSystem");
        _userId = userId;
        RegisterClassMap();
    }

    private static void RegisterClassMap()
    {
        var typesToRegister = DataSerializer.GetTypesToRegister();
        for (int i = 0; i < typesToRegister.Length; i++)
        {
            Type type = typesToRegister[i];
            if (type == null) break;

            // Verifica che il tipo non sia già stato registrato per evitare errori
            if (!BsonClassMap.IsClassMapRegistered(type))
            {
                BsonClassMap.LookupClassMap(type); // Registra automaticamente il tipo
            }
        }
    }

    public static List<BsonDocument> LoadData()
    {
        var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");
        var sortDefinition = Builders<BsonDocument>.Sort.Descending("_id");
        var docs = collection.Find(new BsonDocument()).Sort(sortDefinition).ToList();

        /* BsonDocument doc = docs[slotId];
        // Converto Bson -> JSON string
        string json = doc.ToJson(); */

        // Deserializzo in oggetto C#
        //return DataSerializer.Deserialize<List<CloudData>>(docs.ToJson());
        return docs;
    }

    public static void SaveData(CloudData data)
    {
        var collection = _database.GetCollection<BsonDocument>($"{_userId}_Slots");

        var filter = Builders<BsonDocument>.Filter.Eq("nameSlot", data.nameSlot);

        var update = Builders<BsonDocument>.Update.Set($"values", data.values);

        // `IsUpsert = true` → se non trova nulla, crea un nuovo documento
        var options = new UpdateOptions { IsUpsert = true };

        collection.UpdateOne(filter, update, options);
    }
}