using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using Yone.Api;
using YoneSql;

namespace YoneLib.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public sealed class IsUserBlacklisted : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            try
            {
                #region database connection

                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_YoneUsers");
                var collection = database.GetCollection<BsonDocument>($"_{ctx.User.Id}");

                var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
                var document = collection.Find(filter).First();

                var data = YoneUserAPI.BlacklistAPi.FromJson(document.ToJson());

                #endregion

                if (data.Blocked)
                    return false;
                if (data.Blocked == false) return true;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await YoneUsers.CreateUserEntry(ctx.User.Id);
                    return true;
                }

                Console.WriteLine(e);
                throw;
            }

            return false;
        }
    }
}