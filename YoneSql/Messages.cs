using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace YoneSql
{
    public class Messages
    {
        public static async Task CreateDatabase(string m, string g, string msg)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_Messages");
            var collection = database.GetCollection<MessageSetting>($"{m}");
            await collection.Indexes.CreateOneAsync(Builders<MessageSetting>.IndexKeys.Ascending(_ => _.Id));
            var settings = new MessageSetting
            {
                Username = m,
                Guild = g,
                Content = msg,
                date = DateTime.Now
            };
            await collection.InsertManyAsync(new[] {settings});
        }

        public class MessageSetting
        {
            public ObjectId Id { get; set; }
            public string Username { get; set; }
            public string Guild { get; set; }
            public string Content { get; set; }
            public DateTime date { get; set; }
        }
    }
}