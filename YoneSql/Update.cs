using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace YoneSql
{
    public class Update
    {
        public static async Task CreateUpdateTask(ulong memberId)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_Update");
            var collection = database.GetCollection<UpdateItems>($"Update:{memberId}");
            await collection.Indexes.CreateOneAsync(Builders<UpdateItems>.IndexKeys.Ascending(_ => _.Id));
            var settings = new UpdateItems
            {
                //Default
                UpdateText = "No update log available yet.",
                UpdateVersion = "No update version.",
                SubmittedBy = "No one submitted anything."
            };
            await collection.InsertManyAsync(new[] {settings});
        }

        public static async Task SubmitUpdateText(ulong memberId, string UpdateVersion, string SubmittedBY,
            string UpdateText)
        {
            try
            {
                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_Update");
                var collection = database.GetCollection<BsonDocument>($"Update:{memberId}");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
                var update1 = Builders<BsonDocument>.Update.Set("UpdateVersion", $"{UpdateVersion}");
                var update2 = Builders<BsonDocument>.Update.Set("SubmittedBy", $"{SubmittedBY}");
                var update = Builders<BsonDocument>.Update.Set("UpdateText", $"{UpdateText}");
                await collection.UpdateOneAsync(filter, update1);
                await collection.UpdateOneAsync(filter, update2);
                await collection.UpdateOneAsync(filter, update);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private class UpdateItems
        {
            public int Id { get; set; }
            public string UpdateText { get; set; }
            public string UpdateVersion { get; set; }
            public string SubmittedBy { get; set; }
        }
    }
}