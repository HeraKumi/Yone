using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace YoneSql
{
    public class ranom
    {
        public static async Task create(ulong memberId)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneTestDatabase");
            var collection = database.GetCollection<test>($"_{memberId}");
            await collection.Indexes.CreateOneAsync(Builders<test>.IndexKeys.Ascending(_ => _.Id));
            var Cows = new test
            {
                NUm = 0
            };
            await collection.InsertManyAsync(new[] {Cows});
        }

        public static async Task Like(ulong memberId, int Like)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneTestDatabase");
            var collection = database.GetCollection<BsonDocument>($"_{memberId}");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var update = Builders<BsonDocument>.Update.Set("NUm", $"{Like}");
            await collection.UpdateOneAsync(filter, update);
        }

        private class test
        {
            public int Id { get; set; }
            public int NUm { get; set; }
        }
    }
}