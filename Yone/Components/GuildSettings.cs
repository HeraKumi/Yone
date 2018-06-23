using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.ModernEmbedBuilder;
using MongoDB.Bson;
using MongoDB.Driver;
using YoneAttributes;
using YoneLib;
using YoneLib.Attribute;
using YoneSql;

namespace Yone.Components
{
    [Group("guild")]
    [DoesUserHave(Permissions.Administrator)]
    [IsUserBlacklisted]
    [Description(
        description: "the group holds all the commands retaining to server configuration. If you need help please do `guild help`")]
    public class GuildSettings : BaseCommandModule
    {
        [Command("Create")]
        [Description("This command will create the server settings entry.")]
        public async Task CreateGuildSettings(CommandContext c)
        {
            #region database connection

            var data = new Global().GetDBRecords(c.Guild.Id);

            #endregion

            try
            {
                if (data.Id == 0)
                {
                    await c.RespondAsync($"{c.User.Mention} your Guild settings is already created! `-- meeeh`");
                }
                else
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync("I have created your server settings entry");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("list")]
        [Description("Get your server settings.")]
        public async Task ListGuildSettings(CommandContext c)
        {
            try
            {
                var data = new Global().GetDBRecords(c.Guild.Id);

                //Roles
                var autorole = data.Autorole == "No roles are going to be applied to new users."
                    ? "Hasn't been given any role in this server to give to new user!"
                    : $"<@&{data.Autorole}>";
                var botautorole =
                    data.BotAutoRole == "No roles are going to be applied to new bot accounts that have joined!"
                        ? "Hasn't been given any role in this server to give to new bots!"
                        : $"<@&{data.BotAutoRole}>";
                var adminrole = data.AdminRole == "No admin role for this guild."
                    ? "No admin role for this guild."
                    : $"<@&{data.AdminRole}>";
                var modrole = data.ModRole == "No mod role for this guild."
                    ? "No mod role for this guild."
                    : $"<@&{data.ModRole}>";
                var muterole = data.MuteRole == "No mute role for this guild."
                    ? "No mute role for this guild."
                    : $"<@&{data.MuteRole}>";

                //Channels
                var announcementchannel = data.AnnouncementChannel == "Announcement channel hasn't been set up yet."
                    ? "No announcement channel has been configured in this guild."
                    : $"<#{data.AnnouncementChannel}>";
                var welcomechannel = data.WelcomeChannel == "Welcome channel hasn't been set up yet."
                    ? "No welcome channel has been configured in this guild."
                    : $"<#{data.WelcomeChannel}>";
                var leavechannel = data.LeaveChannel == "leave channel hasn't been set up yet."
                    ? "No leave channel has been configured in this guild."
                    : $"<#{data.LeaveChannel}>";
                var modchannel = data.ModerationChannel == "Moderation channel hasn't been set up yet."
                    ? "No moderation channel has been configured in this guild."
                    : $"<#{data.ModerationChannel}>";
                var loggingchannel = data.LogChannel == "Guild log channel hasn't been set up yet."
                    ? "No logging channel has been configured in this guild."
                    : $"<#{data.LogChannel}>";

                //Messages
                var welcomemessage = data.WelcomeMessage == "Welcome {Member_Mention} to {Guild_Name}!"
                    ? "Default welcome message."
                    : $"{data.WelcomeMessage}";
                var leavemessage = data.LeaveMessage == "{Member_Mention} has left {Guild_Name}."
                    ? "Default leave message."
                    : $"{data.LeaveMessage}";
                var botwelcomemessage = data.BotWelcomeMessage == "Bot: {Member_Mention} has joined!"
                    ? "Default bot welcome message."
                    : $"{data.BotWelcomeMessage}";
                var botleavemessage = data.BotLeaveMessage == "Bot: {Member_Mention} has left the guild, What a shame."
                    ? "Default bot leave message."
                    : $"{data.BotLeaveMessage}";
                await new ModernEmbedBuilder
                {
                    Color = 0xCE93D8,
                    Fields =
                    {
                        ("Default", $"`Prefix`: {data.GuildPrefix}"),
                        ("Roles", $"`AutoRole`: {autorole}\n" +
                                  $"`BotAutoRole`: {botautorole}"),
                        ("Channels", $"`AnnouncementChannel`: {announcementchannel}\n" +
                                     $"`WelcomeChannel`: {welcomechannel}\n" +
                                     $"`LeaveChannel`: {leavechannel}\n" +
                                     $"`LoggingChannel`: {loggingchannel}\n" +
                                     $"`ModerationChannel`: {modchannel}"),
                        ("Message", $"`WelcomeMessage`: {welcomemessage}\n" +
                                    $"`BotWelcomeMessage`: {botwelcomemessage}\n" +
                                    $"`LeaveMessage`: {leavemessage}\n" +
                                    $"`BotLeaveMessage`: {botleavemessage}")
                    }
                }.Send(c.Channel);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setAnnouncementChannel")]
        public async Task InsertAnnounceChannel(CommandContext c, [Description("chn will be a channel pretty easy eeh?")] DiscordChannel chn)
        {
            try
            {
                await Database.InsertAnnouncementChannel(c.Guild.Id, $"{chn.Id}");

                await c.RespondAsync($"I have set your `Announcement Channel` to {chn.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setLogChannel")]
        public async Task InsertLogChannel(CommandContext c, [Description("chn will be a channel pretty easy eeh?")] DiscordChannel chn)
        {
            try
            {
                await Database.InsertLogChannel(c.Guild.Id, $"{chn.Id}");

                await c.RespondAsync($"I have set your `Guild Logging Channel` to {chn.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setWelcomeChannel")]
        public async Task InsertWelcomeChannel(CommandContext c, [Description("chn will be a channel pretty easy eeh?")] DiscordChannel chn)
        {
            try
            {
                await Database.InsertWelcomeChannel(c.Guild.Id, $"{chn.Id}");

                await c.RespondAsync($"I have set your `Welcome Channel` to {chn.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setLeaveChannel")]
        public async Task InsertLeaveChannel(CommandContext c, [Description("chn will be a channel pretty easy eeh?")] DiscordChannel chn)
        {
            try
            {
                await Database.InsertLeaveChannel(c.Guild.Id, $"{chn.Id}");

                await c.RespondAsync($"I have set your `Leave Channel` to {chn.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setModChannel")]
        public async Task InsertModerationChannel(CommandContext c, [Description("chn will be a channel pretty easy eeh?")] DiscordChannel chn)
        {
            try
            {
                await Database.InsertModerationChannel(c.Guild.Id, $"{chn.Id}");

                await c.RespondAsync($"I have set your `Moderation Channel` to {chn.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setWelcomeMessage")]
        public async Task InsertWelcomeMessages(CommandContext c, [Description("Message  will be a message you want to display when a new member joins")] [RemainingText] string Message)
        {
            try
            {
                await Database.InsertWelcomeMessage(c.Guild.Id, $"{Message}");

                await c.RespondAsync($"I have set your `Welcome message` to ```{Message}```");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setAutorole")]
        public async Task InsertAutorole(CommandContext c,DiscordRole Role)
        {
            try
            {
                await Database.InsertAutoRole(c.Guild.Id, $"{Role.Id}");

                await c.RespondAsync($"I have set your `Autorole` to {Role.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                }
                else if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("SetBotAutoRole")]
        public async Task BotAutorole(CommandContext x, DiscordRole r)
        {
            try
            {
                await Database.InsertBotRole(x.Guild.Id, $"{r.Id}");
                await x.RespondAsync($"I have set your `Bot Autorole` to {r.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(x.Guild.Id, $"{x.Guild.Owner}");
                    await x.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{x.Prefix}guild {x.Command.Name}`");
                }
                else if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {x.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {x.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setLeaveMessage")]
        public async Task InsertLeaveMessages(CommandContext c, [Description("Message will be a leave message")] [RemainingText] string Message)
        {
            try
            {
                await Database.InsertLeaveMessage(c.Guild.Id, $"{Message}");

                await c.RespondAsync($"I have set your `Leave message` to ```{Message}```");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setBotLeaveMessage")]
        public async Task InsertBotLeaveMessages(CommandContext c, [Description("Message will be a leave message")] [RemainingText] string Message)
        {
            try
            {
                await Database.InsertBotLeaveMessage(c.Guild.Id, $"{Message}");

                await c.RespondAsync($"I have set your `Leave message` to ```{Message}```");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setBotWelcomeMessage")]
        public async Task InsertBotWelcomeMessages(CommandContext c, [Description("Message will be a leave message")] [RemainingText] string Message)
        {
            try
            {
                await Database.InsertBotWelcomeMessage(c.Guild.Id, $"{Message}");

                await c.RespondAsync($"I have set your `Leave message` to ```{Message}```");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("prefix")]
        public async Task ChangePrefix(CommandContext c, [RemainingText] [Description("change the prefix of the discord bot")] string prefix)
        {
            try
            {
                await Database.ChangePrefix(c.Guild.Id, prefix);
                await c.RespondAsync($"I have change my prefix to: `{prefix}`");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("setadminrole"), Hidden]
        public async Task Setadminrole(CommandContext c, DiscordRole r)
        {
            try
            {
                await Database.InsertAdminRole(c.Guild.Id, $"{r.Id}");
                await c.RespondAsync($"`Role`: {r.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }
        
        [Command("setmodrole"), Hidden]
        public async Task Setmodrole(CommandContext c, DiscordRole r)
        {
            try
            {
                await Database.InsertModRole(c.Guild.Id, $"{r.Id}");
                await c.RespondAsync($"`Role`: {r.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }
        
        [Command("setmuterole"), Hidden]
        public async Task Setmuterole(CommandContext c, DiscordRole r)
        {
            try
            {
                await Database.InsertMuteRole(c.Guild.Id, $"{r.Id}");
                await c.RespondAsync($"`Role`: {r.Mention}");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }
        
        [Command("setstreamrole"), Hidden]
        public async Task Setstreamrole(CommandContext c, DiscordRole r)
        {
            try
            {
                // Placeholder
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `{c.Prefix}guild {c.Command.Name}`");
                    throw;
                }

                if (e.Message.Contains("dup key: { : 0 }"))
                {
                    var client = new MongoClient("mongodb://127.0.0.1:27017/");
                    var database = client.GetDatabase("_YoneGuildSettings");
                    var collection = database.GetCollection<BsonDocument>($"Guild: {c.Guild.Id}");
                    await collection.Database.DropCollectionAsync($"Guild: {c.Guild.Id}");
                    return;
                }

                Console.WriteLine(e);
                throw;
            }
        }
    }
}