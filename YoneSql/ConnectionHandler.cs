using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace YoneSql
{
    public class Database
    {
        public static async Task CreateDatabase(ulong guildId, string guildOwner)
        {
            try
            {
                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_YoneGuildSettings");
                var collection = database.GetCollection<GuildSettings>($"Guild: {guildId}");
                await collection.Indexes.CreateOneAsync(Builders<GuildSettings>.IndexKeys.Ascending(_ => _.Id));
                var settings = new GuildSettings
                {
                    //Default
                    GuildOwner = guildOwner,
                    GuildPrefix = ">",

                    //Channels
                    AnnouncementChannel = "Announcement channel hasn't been set up yet.",
                    WelcomeChannel = "Welcome channel hasn't been set up yet.",
                    LeaveChannel = "leave channel hasn't been set up yet.",
                    ModerationChannel = "Moderation channel hasn't been set up yet.",
                    LogChannel = "Guild log channel hasn't been set up yet.",

                    //Messages
                    WelcomeMessage = "Welcome {Member_Mention} to {Guild_Name}!",
                    LeaveMessage = "{Member_Mention} has left {Guild_Name}.",
                    BotWelcomeMessage = "Bot: {Member_Mention} has joined!",
                    BotLeaveMessage = "Bot: {Member_Mention} has left the guild, What a shame.",

                    //Roles
                    Autorole = "No roles are going to be applied to new users.",
                    BotAutoRole = "No roles are going to be applied to new bot accounts that have joined!",
                    AdminRole = "No admin role for this guild.",
                    ModRole = "No mod role for this guild.",
                    MuteRole = "No mute role for this guild.",
                    
                    //Future
                    Streamrole  = "No stream role for this guild"
                };
                await collection.InsertManyAsync(new[] {settings});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // Deafult
        public static async Task ChangePrefix(ulong guildId, string prefix)
        {
            var tableName = "GuildPrefix";
            await InsertDB(guildId, tableName, prefix);
        }

        //Channels
        public static async Task InsertAnnouncementChannel(ulong guildId, string channel)
        {
            try
            {
                var tableName = "AnnouncementChannel";
                await InsertDB(guildId, tableName, channel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task InsertWelcomeChannel(ulong guildId, string channel)
        {
            try
            {
                var tableName = "WelcomeChannel";
                await InsertDB(guildId, tableName, channel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task InsertLeaveChannel(ulong guildId, string channel)
        {
            try
            {
                var tableName = "LeaveChannel";
                await InsertDB(guildId, tableName, channel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task InsertModerationChannel(ulong guildId, string channel)
        {
            try
            {
                var tableName = "ModerationChannel";
                await InsertDB(guildId, tableName, channel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static async Task InsertLogChannel(ulong guildId, string channel)
        {
            try
            {
                var tableName = "LogChannel";
                await InsertDB(guildId, tableName, channel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //Messages
        public static async Task InsertWelcomeMessage(ulong guildId, string message)
        {
            var tableName = "WelcomeMessage";
            await InsertDB(guildId, tableName, message);
        }

        public static async Task InsertLeaveMessage(ulong guildId, string message)
        {
            var tableName = "LeaveMessage";
            await InsertDB(guildId, tableName, message);
        }

        public static async Task InsertBotWelcomeMessage(ulong guildId, string message)
        {
            var tableName = "BotWelcomeMessage";
            await InsertDB(guildId, tableName, message);
        }

        public static async Task InsertBotLeaveMessage(ulong guildId, string message)
        {
            var tableName = "BotLeaveMessage";
            await InsertDB(guildId, tableName, message);
        }

        //Roles
        public static async Task InsertAutoRole(ulong guildId, string role)
        {
            var tableName = "Autorole";
            await InsertDB(guildId, tableName, role);
        }

        public static async Task InsertBotRole(ulong guildId, string role)
        {
            var tableName = "BotAutoRole";
            await InsertDB(guildId, tableName, role);
        }

        public static async Task InsertAdminRole(ulong guildid, string role)
        {
            var tableName = "AdminRole";
            await InsertDB(guildid, tableName, role);
        }

        public static async Task InsertModRole(ulong guildid, string role)
        {
            var tableName = "ModRole";
            await InsertDB(guildid, tableName, role);
        }

        public static async Task InsertMuteRole(ulong guildid, string role)
        {
            var tableName = "MuteRole";
            await InsertDB(guildid, tableName, role);
        }
        public static async Task InsertStreamRole(ulong guildid, string role)
        {
            var tableName = "StreamRole";
            await InsertDB(guildid, tableName, role);
        }

        
        private static async Task InsertDB(ulong collectionUlongID, string tableName, string tableMessage)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneGuildSettings");
            var collection = database.GetCollection<BsonDocument>($"Guild: {collectionUlongID}");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var update = Builders<BsonDocument>.Update.Set($"{tableName}", $"{tableMessage}");
            await collection.UpdateOneAsync(filter, update);
        }

        public class GuildSettings
        {
            //Default settings
            public int Id { get; set; }
            public string GuildOwner { get; set; }
            public string GuildPrefix { get; set; }

            // Channels
            public string AnnouncementChannel { get; set; }
            public string WelcomeChannel { get; set; }
            public string LeaveChannel { get; set; }
            public string ModerationChannel { get; set; }
            public string LogChannel { get; set; }

            //Messages
            public string WelcomeMessage { get; set; }
            public string LeaveMessage { get; set; }
            public string BotWelcomeMessage { get; set; }
            public string BotLeaveMessage { get; set; }

            // Roles
            public string ModRole { get; set; }
            public string AdminRole { get; set; }
            public string MuteRole { get; set; }
            public string Autorole { get; set; }
            public string BotAutoRole { get; set; }
            public string Streamrole { get; set; }
        }
    }
}