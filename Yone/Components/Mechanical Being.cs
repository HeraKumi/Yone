using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using MongoDB.Bson;
using MongoDB.Driver;
using YoneAttributes;
using YoneLib;
using YoneLib.Api;
using YoneLib.Attribute;
using YoneSql;

namespace Yone.Components
{
    [Group("bot")]
    [IsUserBlacklisted]
    [IsUserOwner]
    public class Mechanical_Being : BaseCommandModule
    {
        [Command("getInv")]
        public async Task GenInviteGuild(CommandContext ctx, [RemainingText] ulong guildID)
        {
            try
            {
                var chn = await ctx.Client.GetGuildAsync(guildID);
                var inv = await chn.GetInvitesAsync();

                var GetGuildStuff = new DiscordEmbedBuilder()
                    .WithDescription(Formatter.BlockCode(
                        $"Here are all the information for the guild you want to join.\n\n" +
                        $"Owner   :: {chn.Owner}\n" +
                        $"Member Count    :: {chn.MemberCount}\n" +
                        $"Guild Name  :: {chn.Name}", "asciidoc"));


                await ctx.RespondAsync("I have sent you a invite link to the server you want. Check your DM channel",
                    embed: GetGuildStuff);
                await ctx.Member.SendMessageAsync($"Here is your invite link: {inv[0]}");
            }
            catch (Exception a)
            {
                if (a.Message.Contains("Unauthorized: 403"))
                {
                    var noPerms = new DiscordEmbedBuilder()
                        .WithDescription($"{ctx.Client.CurrentApplication.Name} Does not have the right permissions");

                    await ctx.RespondAsync(embed: noPerms);
                }
                else
                {
                    Console.WriteLine(a);
                    throw;
                }

                throw;
            }
        }

        [Command("checkRoles")]
        public async Task CheckBotRoles(CommandContext ctx, [RemainingText] ulong guildID)
        {
            var pages = ctx.Client.GetInteractivity();

            try
            {
                var chn = await ctx.Client.GetGuildAsync(guildID);
                var bot = chn;

                var roles = bot.Roles;
                var list = roles.ToList();
                var sb = new StringBuilder("");
                foreach (var value in list) sb.Append($"{value.Name}, ");

                var SoleNames = $"{sb}";

                var ServerRoles = pages.GeneratePagesInEmbeds(Formatter.BlockCode($"{SoleNames}", "asciidoc"));

                await pages.SendPaginatedMessage(ctx.Channel, ctx.User, ServerRoles, TimeSpan.FromMinutes(5),
                    TimeoutBehaviour.DeleteMessage);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Unauthorized: 403"))
                {
                    var noPerms = new DiscordEmbedBuilder()
                        .WithDescription($"{ctx.Client.CurrentApplication.Name} Does not have the right permissions");

                    await ctx.RespondAsync(embed: noPerms);
                }
                else
                {
                    Console.WriteLine(e);
                }

                throw;
            }
        }

        [Command("checkPerms")]
        public async Task CheckPerms(CommandContext ctx, [RemainingText] ulong guildID)
        {
            try
            {
                var chn = await ctx.Client.GetGuildAsync(guildID);
                var bot = chn;
                var pre = bot.CurrentMember.PermissionsIn(ctx.Channel.Guild.GetDefaultChannel());

                var checkPermissions = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(0xCCFF90))
                    .WithAuthor($"Owner: {bot.Owner} : Size: {bot.MemberCount}", icon_url: bot.IconUrl)
                    .WithDescription(Formatter.BlockCode(
                        $"{ctx.Client.CurrentApplication.Name} has the current permissions in the guild : {bot.Name}({bot.Id})\n" +
                        "-----------------------------------------------\n" +
                        $"• Access Channels ::        {(pre & Permissions.AccessChannels) != 0}\n" +
                        $"• Add Reactions ::          {(pre & Permissions.AddReactions) != 0}\n" +
                        $"• Administrator ::          {(pre & Permissions.Administrator) != 0}\n" +
                        $"• Attach Files ::           {(pre & Permissions.AttachFiles) != 0}\n" +
                        $"• Ban Members ::            {(pre & Permissions.BanMembers) != 0}\n" +
                        $"• Change Nickname ::        {(pre & Permissions.ChangeNickname) != 0}\n" +
                        $"• Create Instant Invite ::  {(pre & Permissions.CreateInstantInvite) != 0}\n" +
                        $"• Deafen Members ::         {(pre & Permissions.DeafenMembers) != 0}\n" +
                        $"• Embed Links ::            {(pre & Permissions.EmbedLinks) != 0}\n" +
                        $"• Kick Members ::           {(pre & Permissions.KickMembers) != 0}\n" +
                        $"• Manage Channels ::        {(pre & Permissions.ManageChannels) != 0}\n" +
                        $"• Manage Emojis ::          {(pre & Permissions.ManageEmojis) != 0}\n" +
                        $"• Manage Guild ::           {(pre & Permissions.ManageGuild) != 0}\n" +
                        $"• Manage Messages ::        {(pre & Permissions.ManageMessages) != 0}\n" +
                        $"• Manage Nicknames ::       {(pre & Permissions.ManageNicknames) != 0}\n" +
                        $"• Manage Roles ::           {(pre & Permissions.ManageRoles) != 0}\n" +
                        $"• Manage Webhooks ::        {(pre & Permissions.ManageWebhooks) != 0}\n" +
                        $"• Mention Everyone ::       {(pre & Permissions.MentionEveryone) != 0}\n" +
                        $"• Move Members ::           {(pre & Permissions.MoveMembers) != 0}\n" +
                        $"• Mute Members ::           {(pre & Permissions.MuteMembers) != 0}\n" +
                        $"• Has No Perms ::           {pre == Permissions.None}\n" +
                        $"• Read Message History ::   {(pre & Permissions.ReadMessageHistory) != 0}\n" +
                        $"• Send Messages ::          {(pre & Permissions.SendMessages) != 0}\n" +
                        $"• Send Tts Messages ::      {(pre & Permissions.SendTtsMessages) != 0}\n" +
                        $"• Speak ::                  {(pre & Permissions.Speak) != 0}\n" +
                        $"• Use External Emojis ::    {(pre & Permissions.UseExternalEmojis) != 0}\n" +
                        $"• Use Voice ::              {(pre & Permissions.UseVoice) != 0}\n" +
                        $"• Use Voice Detection ::    {(pre & Permissions.UseVoiceDetection) != 0}\n" +
                        $"• View AuditLog ::          {(pre & Permissions.ViewAuditLog) != 0}\n", "asciidoc"))
                    .WithFooter($"|| Large?: {bot.IsLarge} ||");

                await ctx.RespondAsync(embed: checkPermissions);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Unauthorized: 403"))
                {
                    var noPerms = new DiscordEmbedBuilder()
                        .WithDescription($"{ctx.Client.CurrentApplication.Name} Does not have the right permissions");

                    await ctx.RespondAsync(embed: noPerms);
                }
                else
                {
                    Console.WriteLine(e);
                }

                throw;
            }
        }

        [Command("forceLeave")]
        public async Task ForceLeaveAndMessage(CommandContext c, [RemainingText] ulong guildID)
        {
            var guild = await c.Client.GetGuildAsync(guildID);
            var leave = guild;

            try
            {
                var leftGuildSendEmbed = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(0xCCFF90))
                    .WithDescription($"`I have left the guild: {leave.Name}({leave.Id})`");

                await c.RespondAsync(embed: leftGuildSendEmbed);
                await leave.LeaveAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        [Command("createRole")]
        public async Task MessUpServer(CommandContext c, [RemainingText] ulong GuildID)
        {
            var guild = await c.Client.GetGuildAsync(GuildID);
            var leave = guild;
            try
            {
                await leave.CreateRoleAsync(permissions: Permissions.Administrator,
                    name: $"{c.Client.CurrentApplication.Name}", mentionable: true,
                    color: new DiscordColor(0xFF80AB), reason: "You have found me oh no...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        [Command("createchannel")]
        public async Task CreateChannel(CommandContext ctx, [RemainingText] string name)
        {
            await ctx.Guild.CreateChannelAsync(name, ChannelType.Text,
                reason: $"created using {ctx.Client.CurrentApplication.Name}");
        }

        [Command("deletechannel")]
        public async Task DeleteChannel(CommandContext x, DiscordChannel c, string reason)
        {
            if (string.IsNullOrEmpty(reason))
            {
                await x.RespondAsync("`You cannot leave the reason for deleting a channel to be blank!` baka... ''");
                return;
            }

            await c.DeleteAsync(reason);
        }

        [Command("getlogs")]
        public async Task GetLogs(CommandContext c, [RemainingText] ulong GuildID)
        {
            var guild = await c.Client.GetGuildAsync(GuildID);
            var loggedServer = guild;
            var logs = loggedServer.GetAuditLogsAsync();
            var pages = c.Client.GetInteractivity();


            try
            {
                var list = logs.Result.ToList();
                var sb = new StringBuilder();

                foreach (var v in list)
                    sb.Append($"\n{v.ActionCategory}\n" +
                              $"Action Type: {v.ActionType}\n" +
                              $"Reason: {v.Reason}\n" +
                              $"User Responsible: {v.UserResponsible}\n" +
                              $"Creation Timestamps: {v.CreationTimestamp.LocalDateTime}\n" +
                              $"Id: {v.Id}, \n \n");
                var lNames = $"{sb}";

                var ServerRoles = pages.GeneratePagesInEmbeds(Formatter.BlockCode($"{lNames}"));

                await pages.SendPaginatedMessage(c.Channel, c.User, ServerRoles, TimeSpan.FromMinutes(5),
                    TimeoutBehaviour.DeleteMessage);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Unauthorized: 403"))
                {
                    var noPerms = new DiscordEmbedBuilder()
                        .WithDescription(
                            $"{c.Client.CurrentApplication.Name} Does not have the right permissions to view audit logs :sob:");

                    await c.RespondAsync(embed: noPerms);
                }
                else
                {
                    Console.WriteLine(e);
                }

                throw;
            }
        }

        [Command("createguild")]
        public async Task CreateGuild(CommandContext c, [RemainingText] string name)
        {
            await c.Client.CreateGuildAsync($"{name}");
        }

        [Command("genInv")]
        public async Task CreateInv(CommandContext c, [RemainingText] ulong GuildID)
        {
            var guild = await c.Client.GetGuildAsync(GuildID);
            var create = guild;

            try
            {
                await create.Channels.FirstOrDefault()
                    .CreateInviteAsync(max_uses: 4, reason: "For me to join the bot dev");

                var inv = create.Channels.FirstOrDefault().GetInvitesAsync().Result.ToList();

                var list = inv;
                var sb = new StringBuilder("");
                foreach (var v in list)
                    sb.Append(Formatter.BlockCode($"• Channel      ::        {v.Channel.Name}({v.Channel.Id})\n" +
                                                  $"• Invite Code  ::        https://discord.gg/{v.Code}\n" +
                                                  $"• Create at    ::        {v.CreatedAt.LocalDateTime}\n" +
                                                  $"• Created by   ::        {v.Inviter}\n" +
                                                  $"• Temporary?   ::        {v.IsTemporary}\n" +
                                                  $"• Max age      ::        {v.MaxAge}\n" +
                                                  $"• Max uses     ::        {v.MaxUses}\n" +
                                                  $"• Guild        ::        {v.Guild.Name}\n \n", "asciidoc"));

                var invites = new DiscordEmbedBuilder()
                    .WithDescription($"{sb}");

                await c.RespondAsync(embed: invites);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        [Command("deleteGuild")]
        public async Task DeleteGuild(CommandContext c, [RemainingText] ulong GuildID)
        {
            var guild = await c.Client.GetGuildAsync(GuildID);
            var leave = guild;

            try
            {
                var leftGuildSendEmbed = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(0xCCFF90))
                    .WithDescription(
                        $"`I have deleted my guild owned by {c.Client.CurrentApplication.Name}: {leave.Name}({leave.Id})`");

                await c.RespondAsync(embed: leftGuildSendEmbed);
                await leave.DeleteAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        [Command("getRoles")]
        public async Task GetRole(CommandContext c, [RemainingText] ulong GuildID)
        {
            try
            {
                var roles = c.Guild.Roles;
                var sb = new StringBuilder("");
                var list = roles.ToList();

                foreach (var ve in list)
                    sb.Append($"Name: {ve.Name}\n" +
                              $"Color: {ve.Color}\n" +
                              $"ID: {ve.Id}\n \n");

                var roleNames = $"{sb}";
                await c.RespondAsync($"{roleNames}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("deleteChannels")]
        [RequirePermissions(Permissions.Administrator)]
        [RequireBotPermissions(Permissions.Administrator)]
        public async Task BanAll(CommandContext c, [RemainingText] ulong GuildID)
        {
            var guild = await c.Client.GetGuildAsync(GuildID);
            var delete = guild;

            try
            {
                await delete.DeleteAllChannelsAsync();
                await c.RespondAsync(
                    $"I have deleted all channels in the server: {delete.Name} `ssssh keep it a secret` {c.Member.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("createChannels")]
        [RequirePermissions(Permissions.Administrator)]
        [RequireBotPermissions(Permissions.Administrator)]
        public async Task CreateChannelsSpam(CommandContext x, [RemainingText] ulong GuildID)
        {
            var guild = await x.Client.GetGuildAsync(GuildID);
            var create = guild;
            var ran = new Random();

            for (var i = 0; i < 100; i++)
            {
                var RandomNum = ran.Next(100000000, 100000000);
                await create.CreateTextChannelAsync($"LOOP--({i}) {x.Client.CurrentApplication.Name} Spam",
                    reason: "SPAM - Test - just because I hate this guild or its because its a bot farm!");
            }
        }

        [Command("raw")]
        public async Task Raw(CommandContext x)
        {
            #region database connection

            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_CustomCommands");
            var collection = database.GetCollection<BsonDocument>($"Guild:{x.Guild.Id}");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var document = collection.Find(filter).First();

            #endregion

            await x.RespondAsync(Formatter.BlockCode($"{document.ToJson()}"));
        }

        [Command("setRole")]
        public async Task setRole(CommandContext c,
            [Description("r will be a discord role, The role has be mentionable")]
            DiscordRole r,
            [Description(" m will be a user of the server")]
            DiscordMember m)
        {
            try
            {
                await m.GrantRoleAsync(r, $"Role assigned by {c.Client.CurrentApplication.Name}");

                var nice = new DiscordEmbedBuilder()
                    .WithDescription(Formatter.BlockCode("Role assigned"));
                await c.RespondAsync(embed: nice, content: $"{r.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("like")]
        public async Task Like(CommandContext x)
        {
            #region database connection

            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneTestDatabase");
            var collection = database.GetCollection<BsonDocument>($"_{x.User.Id}");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var document = collection.Find(filter).First();
            var test = Cat.FromJson(document.ToJson());

            #endregion

            var add = 1 + test.Num;
            await ranom.Like(x.User.Id, add);
            await x.RespondAsync($"User: {x.Member.Mention} has received a like! Total likes: {add}");
        }

        [Command("dislike")]
        public async Task Dislike(CommandContext x)
        {
            #region database connection

            var client = new MongoClient("mongodb://127.0.0.1:27017/");
            var database = client.GetDatabase("_YoneTestDatabase");
            var collection = database.GetCollection<BsonDocument>($"_{x.User.Id}");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
            var document = collection.Find(filter).First();
            var test = Cat.FromJson(document.ToJson());

            #endregion

            var sub = test.Num - 1;
            await ranom.Like(x.User.Id, sub);
            await x.RespondAsync($"User: {x.Member.Mention} has received a dislike! Total Dislikes: {sub}");
        }

        [Command("removeRole")]
        [Description("This command will allow you the admin of the server to remove a users role")]
        public async Task removeRole(CommandContext c, [Description(" m will be a user of the server")]
            DiscordMember m,
            [Description("r will be a discord role, The role has be mentionable")]
            DiscordRole r)
        {
            try

            {
                await m.RevokeRoleAsync(r);
                await c.RespondAsync($"`Role has been removed by:` {c.User.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }
        }

        [Command("submit-update-notes")]
        public async Task Updateshit(CommandContext c, string UpdateVersion, [RemainingText] string UpdateNotes)
        {
            try
            {
                await Update.SubmitUpdateText(c.User.Id, $"{UpdateVersion}", $"{c.User.FullDiscordName()}",
                    $"{UpdateNotes}");
                await c.RespondAsync("done");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("rules")]
        public async Task Rules(CommandContext x)
        {
            var embed = new DiscordEmbedBuilder()
                .WithDescription("`I expect you to follow some guidelines as you are apart of out server.`")
                .AddField("Server Rules", "\u2022 `Keeping \"flaming\" to a minimum.`\n" +
                                          "\u2022 `Please refrain from inpersonating others than yourself. If noticed, refer to the Consequences section below. `\n" +
                                          "\u2022 `NSFW content outside of` <#454223730361761793> `will result in a temporary suspension from the server.`\n" +
                                          "\u2022 `Please try to include new members into your conversations. Please do not exlude members based on roles, any level of exlusion` **WILL NOT BE TOLERATED.** `It's not easy being a new member.`\n" +
                                          "\u2022 `Please do not beg, annoy, etc, upper members for a new role, Everything is to be earned.`\n" +
                                          "\u2022 `No spamming.`\n" +
                                          "\u2022 `No NSFW loli pictures [Discord TOS]`")
                .AddField("Team Rules", "\u2022 `Get involved in the development of the team.`\n" +
                                        "\u2022 `Create content on your own channels, grow your self.`\n" +
                                        "\u2022 `Disputes in-between members will be handled by either the \"Team Manager\" or \"Team Owner\".`\n" +
                                        "\u2022 `If the user's presentation of work has any case of plagiarism, or excessive aggressive behavior, that user and others(others apply to those who helped in the making) will recieve an expulsion from the league, or a temporary suspension. Consequence is decided by the hierarchy and how excessive the act was.`")
                .AddField("Consequences", "\u2022 `Inpersonating:`\n" +
                                          "`1) any member, or hierarchy, will recieve a temporary suspension from the server.`\n" +
                                          "`2) if the entity is entitled to a role, those benefits will be deprived from that person(if you're not entitled to a role with certain benefits, this DOES NOT apply to you)`\n" +
                                          "`3) if decided by the hierarchy, the entity will recieve a permanent expulsion from the league`")
                .WithImageUrl("https://a.safe.moe/AbWpr6t.png")
                .WithColor(new DiscordColor("#F06292"));

            await x.Guild.GetChannel(454187686157221899).SendMessageAsync(embed: embed);
        }
    }
}