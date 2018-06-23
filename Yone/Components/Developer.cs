using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using MongoDB.Bson;
using MongoDB.Driver;
using Yone.Api;
using YoneAttributes;
using YoneLib;
using YoneLib.Api;
using YoneSql;

namespace Yone.Components
{
    [Group("dev")]
    [IsUserOwner]
    public class Developer : BaseCommandModule
    {
        [Command("eval")]
        [Aliases("evalcs", "cseval", "roslyn")]
        [Description("Evaluates C# code.")]
        public async Task EvalCs(CommandContext ctx, [RemainingText] string code)
        {
            /////////////////////////////////////////////////////////////
            //Code provided by i forgot                               ///
            // If you are the owner the of the code below please pm at///
            // - ̗̀Hera ̖́-#0002 on discord                               ///
            /////////////////////////////////////////////////////////////

            DiscordMessage msg;

            var cs1 = code.IndexOf("```", StringComparison.Ordinal) + 3;
            cs1 = code.IndexOf('\n', cs1) + 1;
            var cs2 = code.LastIndexOf("```", StringComparison.Ordinal);

            if (cs1 == -1 || cs2 == -1)
            {
                var erorr = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(0xF4FF81))
                    .WithDescription("`You need to wrap the code into a code block. ```codehere ```  `");

                await ctx.RespondAsync(embed: erorr);
                throw new ArgumentException("You need to wrap the code into a code block.");
            }

            var cs = code.Substring(cs1, cs2 - cs1);

            msg = await ctx.RespondAsync("", embed: new DiscordEmbedBuilder()
                .WithColor(new DiscordColor(0xCCFF90))
                .WithDescription("Evaluating...")
                .Build()).ConfigureAwait(false);

            try
            {
                var globals = new TestVariables(ctx.Message, ctx.Client, ctx);

                var sopts = ScriptOptions.Default;
                sopts = sopts.WithImports("System", "System.Collections.Generic", "System.Linq", "System.Net.Http",
                    "System.Net.Http.Headers", "System.Reflection", "System.Text", "System.Threading.Tasks",
                    "DSharpPlus", "DSharpPlus.CommandsNext", "DSharpPlus.Interactivity");
                sopts = sopts.WithReferences(AppDomain.CurrentDomain.GetAssemblies()
                    .Where(xa => !xa.IsDynamic && !string.IsNullOrWhiteSpace(xa.Location)));

                var script = CSharpScript.Create(cs, sopts, typeof(TestVariables));
                script.Compile();
                var result = await script.RunAsync(globals).ConfigureAwait(false);

                if (result != null && result.ReturnValue != null &&
                    !string.IsNullOrWhiteSpace(result.ReturnValue.ToString()))
                {
                    var results = new DiscordEmbedBuilder()
                        .WithTitle($"{ctx.Member.Username}: Result")
                        .AddField("Results", $"{result.ReturnValue}")
                        .AddField("Return Type", $"{result.ReturnValue.GetType()}")
                        .WithColor(new DiscordColor(0xB9F6CA));
                    await msg.ModifyAsync(embed: new DSharpPlus.Entities.Optional<DiscordEmbed>(results))
                        .ConfigureAwait(false);
                }

                else
                {
                    var successful = new DiscordEmbedBuilder()
                        .WithTitle($"{ctx.Member.Username}: Successful")
                        .AddField("Results = False",
                            $"No result was returned. To have a results please make sure you use `return`")
                        .WithColor(new DiscordColor(0xB9F6CA));
                    await msg.ModifyAsync(embed: new DSharpPlus.Entities.Optional<DiscordEmbed>(successful))
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                var failure = new DiscordEmbedBuilder()
                    .WithTitle($"{ctx.Member.Username}: Failure")
                    .AddField($"{ex.GetType()}", $"**{ex.Message}**")
                    .WithColor(new DiscordColor(0xFF8A80));

                await msg.ModifyAsync(embed: new DSharpPlus.Entities.Optional<DiscordEmbed>(failure))
                    .ConfigureAwait(false);
            }
        }

        [Command("sudoowner")]
        [Aliases("so")]
        public async Task SudoOwnerAsync(CommandContext ctx,
            [RemainingText] [Description("Command to sudo")]
            string command)
        {
            await ctx.CommandsNext.SudoAsync(ctx.Guild.Owner, ctx.Channel, command);
        }

        [Command("sudo")]
        [Aliases("s")]
        public async Task SudoAsync(CommandContext ctx, ulong guildId, [Description("Member to sudo")] DiscordMember m,
            [Description("Command to sudo")] [RemainingText]
            string command)
        {
            await ctx.CommandsNext.SudoAsync(m, ctx.Channel, command);
        }

        [Command("sql")]
        public async Task TestCommandArea(CommandContext c)
        {
            #region database connection

            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneUsers");
            var collection = database.GetCollection<BsonDocument>($"_{c.User.Id}");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var document = collection.Find(filter).First();
            var data = YoneUserAPI.BlacklistAPi.FromJson(document.ToJson());

            #endregion

            if (data.Blocked == false)
            {
                await c.RespondAsync(
                    "You are not blocked from using `Yone`, But as I am testing I will block you from using yone (Not really since it doesn't work it, it is just a test!)");
                await c.RespondAsync("I have blacked listed you now");
                await YoneUsers.BlacklistUser(c.User.Id);
            }
            else if (data.Blocked)
            {
                await c.RespondAsync("I am now going to Whitelist from `Yone`.");
                await YoneUsers.WhitelistUser(c.User.Id);
                await c.RespondAsync("You may now use `Yone` again!.");
            }
        }

        [Command("error")]
        [Description("asdfsadfasd")]
        public async Task ErrorCommand(CommandContext c)
        {
            try
            {
                var i2 = 0;
                var unused = 10 / i2;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("test")]
        public async Task Test(CommandContext c)
        {
        }

        [Command("get")]
        public async Task GetRequest(CommandContext c)
        {
            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_Command");
            var collection = database.GetCollection<BsonDocument>("command");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var document = collection.Find(filter).First();
            
            await c.RespondAsync($"{document.ToJson()}");
        }

        [Command("sql-create-all-guild-db")]
        public async Task CreateDatebases(CommandContext c)
        {
            var list = c.Client.Guilds.ToList();
            foreach (var guild in list)
            {
                var data = new Global().GetDBRecords(c.Guild.Id);

                if (data.Id == 0)
                {
                    await c.RespondAsync($"Guild: `{guild.Value.Name}` already has an entry, skipping over!");
                }
                else
                {
                    await Database.CreateDatabase(guild.Value.Id, $"{guild.Value.Owner}");
                    await c.RespondAsync($"I have created a guild entries for: `{guild.Value.Name}`");
                }
            }
        }

        [Command("AccounceAllToGuilds")]
        public async Task Send(CommandContext x, string message)
        {
            var guild = x.Client.Guilds.ToList();
            foreach (var g in guild)
                try
                {
                    var e = new DiscordEmbedBuilder()
                        .WithTitle($"Announcement from: Bot Owner({x.User.Mention})")
                        .AddField("Message", $"{message}")
                        .WithImageUrl("https://avatars2.githubusercontent.com/u/31966189?s=460&v=4")
                        .WithColor(DiscordColor.DarkRed);
                    await g.Value.GetDefaultChannel().SendMessageAsync(embed: e);
                    await x.RespondAsync($"`{g.Value.Name}`: Message has been sent");
                }
                catch (Exception e)
                {
                    await x.Member.SendMessageAsync($"`{g.Value.Name}` Couldn't accounce message to this guild!");
                    await x.RespondAsync($"```{e}```");
                }
        }

        [Command("fillme")]
        public async Task Fill(CommandContext c)
        {
            const ulong guildId = 402458071349067777;
            //Channels
            const ulong a = 402459781870387214;

            const ulong b = 403692323726426112;
            const ulong cc = 403692323726426112;

            const ulong d = 402459192876597249;
            const ulong e = 402459192876597249;

            //Messages
            const string f =
                "Welcome {Member_Mention}, to `{Guild_Name}`, I hope you have a good time here, Even though this guild is dead :D, `Oh and by the way the owner is:` {Guild_Owner_Mention}";
            const string g = "{Member_Mention} Has left({Guild_Name}), awhh what a shame!";
            //Role
            const ulong h = 403738550861824010;

            await Database.InsertAnnouncementChannel(guildId, $"{a}");
            await Database.InsertWelcomeChannel(guildId, $"{b}");
            await Database.InsertLeaveChannel(guildId, $"{cc}");
            await Database.InsertModerationChannel(guildId, $"{d}");
            await Database.InsertLogChannel(guildId, $"{e}");

            await Database.InsertWelcomeMessage(guildId, $"{f}");
            await Database.InsertLeaveMessage(guildId, $"{g}");

            await Database.InsertAutoRole(guildId, $"{h}");

            await c.RespondAsync("I have updated everything in this guild!");
        }

        [Command("GetGuilds")]
        public async Task GetGuilds(CommandContext c)
        {
            var list = c.Client.Guilds.ToList();
            var sb = new StringBuilder();
            foreach (var g in list)
            {
                #region database connection

                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_YoneDatbase");
                var collection = database.GetCollection<BsonDocument>($"_GuildSettings({g.Value.Id}");

                var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
                var document = collection.Find(filter).First();

                var data = YoneDatabaseAPI.YoneDatabaseApi.FromJson(document.ToJson());

                #endregion

                if (data.Id == 0)
                    sb.Append($"•    GuildName: {g.Value.Name}    ::\n" +
                              $"•    Owned by: {g.Value.Owner}    ::\n" +
                              $"•    Does have db?: true    ::\n\n");
                else
                    sb.Append($"•    GuildName: {g.Value.Name}    ::\n" +
                              $"•    Owned by: {g.Value.Owner}    ::\n" +
                              $"•    Does have db? - false    ::\n\n");
            }

            await c.RespondAsync(Formatter.BlockCode($"{sb}", "asciidoc"));
        }

        [Command("give")]
        public async Task GiveRoles(CommandContext x, DiscordRole r)
        {
            await x.Member.GrantRoleAsync(r, "To give my self admin :3");
            await x.RespondAsync($"{r.Mention}` has been given to `{x.Member.Mention}");
        }

        [Command("blacklist")]
        public async Task Blacklist(CommandContext c, DiscordMember m)
        {
            try
            {
                #region database connection

                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_YoneUsers");
                var collection = database.GetCollection<BsonDocument>($"_{m.Id}");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
                var document = collection.Find(filter).First();
                var data = YoneUserAPI.BlacklistAPi.FromJson(document.ToJson());

                #endregion

                switch (data.Blocked)
                {
                    case true:
                        await c.RespondAsync(
                            $"User({m.Mention}) has already been `blacklisted` from using `{c.Client.CurrentApplication.Name}`.");
                        break;
                    case false:
                        await YoneUsers.BlacklistUser(m.Id);

                        await c.RespondAsync(string.Format("{0} has been blacklisted from using {1}!", m.Mention,
                            c.Client.CurrentUser.Mention));
                        break;
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await YoneUsers.CreateUserEntry(m.Id);
                    await YoneUsers.BlacklistUser(m.Id);
                    await c.RespondAsync(
                        $"{m.Mention} has been blacklisted from using {c.Client.CurrentUser.Mention}!");
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [Command("whitelist")]
        public async Task WhitelistUser(CommandContext c, DiscordMember m)
        {
            try
            {
                #region database connection

                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_YoneUsers");
                var collection = database.GetCollection<BsonDocument>($"_{m.Id}");
                var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
                var document = collection.Find(filter).First();
                var data = YoneUserAPI.BlacklistAPi.FromJson(document.ToJson());

                #endregion

                switch (data.Blocked)
                {
                    case true:
                        await YoneUsers.WhitelistUser(m.Id);

                        await c.RespondAsync(string.Format("{0} has been whitelisted from using {1}!", m.Mention,
                            c.Client.CurrentUser.Mention));
                        break;
                    case false:
                        await c.RespondAsync(
                            $"User({m.Mention}) has already been `whitelisted` from using `{c.Client.CurrentApplication.Name}`.");
                        break;
                }

                await YoneUsers.BlacklistUser(m.Id);

                await c.RespondAsync(string.Format("{0} has been blacklisted from using {1}!", m.Mention,
                    c.Client.CurrentUser.Mention));
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await YoneUsers.CreateUserEntry(m.Id);
                    await YoneUsers.BlacklistUser(m.Id);
                    await c.RespondAsync(
                        $"{m.Mention} has been blacklisted from using {c.Client.CurrentUser.Mention}!");
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }

    public class TestVariables
    {
        public DiscordClient Client;

        public TestVariables(DiscordMessage msg, DiscordClient client, CommandContext ctx)
        {
            Client = client;

            Message = msg;
            Channel = msg.Channel;
            Guild = Channel.Guild;
            User = Message.Author;
            if (Guild != null)
                Member = Guild.GetMemberAsync(User.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            Context = ctx;
        }

        public DiscordMessage Message { get; set; }
        public DiscordChannel Channel { get; set; }
        public DiscordGuild Guild { get; set; }
        public DiscordUser User { get; set; }
        public DiscordMember Member { get; set; }
        public CommandContext Context { get; set; }
    }
}