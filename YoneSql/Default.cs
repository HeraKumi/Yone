using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace YoneSql
{
    public class Default
    {
        public static async Task CreateDatabase()
        {
            try
            {
                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_YoneDefault");
                var collection = database.GetCollection<DefaultSettings>("DefaultSettings");
                await collection.Indexes.CreateOneAsync(Builders<DefaultSettings>.IndexKeys.Ascending(_ => _.Id));
                var settings = new DefaultSettings
                {
                    botToken = "nothing",
                    ipHub = "nothing",
                    discordBotOrg = "nothing",
                    kitsuClientId = "nothing",
                    kitsuClientSecret = "nothing",
                    twitchClientId = "nothing",
                    FirstRun = true
                };
                await collection.InsertManyAsync(new[] {settings});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task botToken(string token)
        {
            await insertCollection("botToken", $"{token}");
        }

        public static async Task ipHub(string token)
        {
            await insertCollection("ipHub", $"{token}");
        }

        public static async Task discordBotOrg(string token)
        {
            await insertCollection("discordBotOrg", $"{token}");
        }

        public static async Task kitsuCLientId(string Id)
        {
            await insertCollection("kitsuClientId", $"{Id}");
        }

        public static async Task kitsuClientSecret(string IdSecret)
        {
            await insertCollection("kitsuClientSecret", $"{IdSecret}");
        }

        public static async Task twitchClientId(string Id)
        {
            await insertCollection("twitchClientId", $"{Id}");
        }

        public static async Task FirstRun(bool trueORfalse)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneDefault");
            var collection = database.GetCollection<BsonDocument>($"DefaultSettings");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var update = Builders<BsonDocument>.Update.Set("FirstRun", trueORfalse);
            await collection.UpdateOneAsync(filter, update);
        }

        private static async Task insertCollection(string tableName, string tableMessage)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneDefault");
            var collection = database.GetCollection<BsonDocument>($"DefaultSettings");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var update = Builders<BsonDocument>.Update.Set($"{tableName}", $"{tableMessage}");
            await collection.UpdateOneAsync(filter, update);
        }

        private class DefaultSettings
        {
            public int Id { get; set; }
            public string botToken { get; set; }

            // API Keys
            public string ipHub { get; set; }

            public string discordBotOrg { get; set; }

            // Kitsu
            public string kitsuClientId { get; set; }
            public string kitsuClientSecret { get; set; }

            //Twitch
            public string twitchClientId { get; set; }

            //OneTimers
            public bool FirstRun { get; set; }
        }
    }
}