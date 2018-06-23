using MongoDB.Bson;
using MongoDB.Driver;
using YoneLib.Api;

namespace YoneLib
{
    public class Global
    {
        public YoneDatabaseAPI.YoneDatabaseApi GetDBRecords(ulong GuildID)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneGuildSettings");
            var collection = database.GetCollection<BsonDocument>($"Guild: {GuildID}");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var document = collection.Find(filter).First();

            var data = YoneDatabaseAPI.YoneDatabaseApi.FromJson(document.ToJson());

            return data;
        }

        public DefaultAPI.DefaultApi DefaultDatabase()
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneDefault");
            var collection = database.GetCollection<BsonDocument>("DefaultSettings");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var document = collection.Find(filter).First();

            var data = DefaultAPI.DefaultApi.FromJson(document.ToJson());

            return data;
        }
    }
}