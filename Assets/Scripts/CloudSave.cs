using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

public static class CloudSave
{
    private static IMongoDatabase _database;

    public static void Init(string username = "cloudsavesystem", string password = "vqppphJRNpG2vEIR", string clusterName = "modulicluster.h6cfk1e")
    {
        var client = new MongoClient($"mongodb+srv://{username}:{password}@{clusterName}.mongodb.net/");
        _database = client.GetDatabase("SaveSystem");

    }

    public static List<PureRawData> LoadData(int slotId)
    {
        var collection = _database.GetCollection<BsonDocument>("Slots");
        var docs = collection.Find(new BsonDocument()).ToList();

        BsonDocument doc = docs[slotId];
        // Converto Bson -> JSON string
        string json = doc.GetValue(2).ToJson();

        // Deserializzo in oggetto C#
        return DataSerializer.Deserialize(json);
    }

    public static void SaveData(List<PureRawData> data)
    {
        /* var filter = Builders<SlotDocument>.Filter.Eq("_id", someObjectId);

        var update = Builders<SlotDocument>.Update.Set("Slots", data);

        // `IsUpsert = true` → se non trova nulla, crea un nuovo documento
        var options = new UpdateOptions { IsUpsert = true };

        collection.UpdateOne(filter, update, options); */
    }


}