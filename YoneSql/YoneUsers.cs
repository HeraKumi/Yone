using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace YoneSql
{
    public class YoneUsers
    {
        public static async Task CreateUserEntry(ulong User_ID)
        {
            try
            {
                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_YoneUsers");
                var collection = database.GetCollection<YoneUserSettings>($"_{User_ID}");
                await collection.Indexes.CreateOneAsync(Builders<YoneUserSettings>.IndexKeys.Ascending(_ => _.Id));
                var settings = new YoneUserSettings
                {
                    //Default
                    Blocked = false
                };
                await collection.InsertManyAsync(new[] {settings});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task BlacklistUser(ulong User_ID)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneUsers");
            var collection = database.GetCollection<BsonDocument>($"_{User_ID}");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var update = Builders<BsonDocument>.Update.Set("Blocked", true);
            await collection.UpdateOneAsync(filter, update);
        }

        public static async Task WhitelistUser(ulong User_ID)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneUsers");
            var collection = database.GetCollection<BsonDocument>($"_{User_ID}");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var update = Builders<BsonDocument>.Update.Set("Blocked", false);
            await collection.UpdateOneAsync(filter, update);
        }

        public class YoneUserSettings
        {
            //Default settings
            public int Id { get; set; }
            public bool Blocked { get; set; }
        }
    }
}