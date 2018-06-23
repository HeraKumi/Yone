using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Extended.AsyncListeners;
using YoneLib;

namespace Yone.Event_Listener
{
    public class SendEventsToMainServer
    {
        [AsyncListener(EventTypes.GuildCreated)]
        public static async Task GuildCreated(DiscordClient y, GuildCreateEventArgs e)
        {
            var firstChannel = e.Guild.Channels.First();


            const ulong guildid = 402458071349067777;
            const ulong
                channelid =
                    405516487332462612; // Test channel: 402460240022470668; // official channel 405516487332462612
            var larsmal = "";
            var chn = await y.GetGuildAsync(guildid);

            var ChannelList = e.Guild.Channels.ToList();
            var EmoteList = e.Guild.Emojis.ToList();
            var RoleList = e.Guild.Roles.ToList();
            var MemberList = e.Guild.Members.ToList();
            var PermissionsList = e.Guild.CurrentMember.PermissionsIn(firstChannel);

            var chns = new StringBuilder();
            var Emotes = new StringBuilder();
            var Roles = new StringBuilder();
            var Members = new StringBuilder();


            try
            {
                foreach (var channel in ChannelList)
                    chns.Append($"{channel.Name}, ".Truncate(550));
                foreach (var emote in EmoteList)
                    Emotes.Append($"{emote.Name}, ".Truncate(550));
                foreach (var role in RoleList)
                    Roles.Append($"{role.Name}, ".Truncate(550));
                foreach (var member in MemberList)
                    Members.Append($"{member.FullDiscordName()}, ".Truncate(550));

                switch (e.Guild.IsLarge)
                {
                    case true:
                        larsmal = "Guild has exceeded 250 members: [Large Guild]";
                        break;
                    case false:
                        larsmal = "Guild has not exceeded 250 members: [Small Guild]";
                        break;
                }

                var emoteSwitch = $"{Emotes.ToString().Truncate(1000)}".BlockCode_ASCIIDOC();

                var NewGuild = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(0x69F0AE))
                    .WithAuthor($"Owner: {e.Guild.Owner.Username}#{e.Guild.Owner.Discriminator}",
                        icon_url: e.Guild.IconUrl)
                    .WithDescription(
                        $"Join date: {e.Guild.JoinedAt.ToLocalTime():hh:mm:ss tt} on {e.Guild.JoinedAt.LocalDateTime.DayOfWeek}")
                    .AddField("General Information", ("== General Information ==\n" +
                                                      $"• Guild_Name::           {e.Guild.Name}\n" +
                                                      $"• Guild_ID::             {e.Guild.Id}\n" +
                                                      $"• Member_Count::         {e.Guild.MemberCount}\n" +
                                                      $"• Default Channel::      {e.Guild.GetDefaultChannel().Name}\n" +
                                                      $"• Channels::             {e.Guild.Channels.Count}\n" +
                                                      $"• Roles::                {e.Guild.Roles.Count}\n" +
                                                      $"• Afk_Channel::          {e.Guild.AfkChannel}\n" +
                                                      $"• Afk_Timeout::          {e.Guild.AfkTimeout}\n" +
                                                      $"• Custom_Emotes::        {e.Guild.Emojis.Count}\n" +
                                                      $"• Ban Member Count::     {await e.Guild.GetBansAsync().ConfigureAwait(false)}\n" +
                                                      $"• Mfa_Level::            {e.Guild.MfaLevel}\n" +
                                                      $"• Verification_Level::   {e.Guild.VerificationLevel}")
                        .BlockCode_ASCIIDOC())
                    .AddField("Channels", ($"== Channels : Count {e.Guild.Channels.Count}==\n" +
                                           $"{chns.ToString().Truncate(1000)}").BlockCode_ASCIIDOC())
                    .AddField("Emotes", ($"== Emotes : Count {e.Guild.Emojis.Count}==\n" +
                                         $"{Emotes.ToString().Truncate(1000)}").BlockCode_ASCIIDOC())
                    .AddField("Roles", ($"== Roles : Count {e.Guild.Roles.Count}==\n" +
                                        $"{Roles.ToString().Truncate(1000)}").BlockCode_ASCIIDOC())
                    .AddField("Members", ($"== Members : Count {e.Guild.MemberCount} ==\n" +
                                          $"{Members.ToString().Truncate(1000)}").BlockCode_ASCIIDOC())
                    .AddField($"Permission(s) {e.Client.CurrentUser.Username} has",
                        $"== Permission(s) ==\n{PermissionsList}".BlockCode_ASCIIDOC())
                    .WithFooter($"{larsmal}");

                await chn.GetChannel(channelid)
                    .SendMessageAsync(embed: NewGuild,
                        content:
                        $"<@278409771319820299> `A new guild has appeared! Leaving the Guild count at: {e.Client.Guilds.Count}`");

                await y.UpdateStatusAsync(new DiscordActivity(type: ActivityType.Watching,
                    name: $"Yone({y.Guilds.Count} servers)"));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        [AsyncListener(EventTypes.GuildDeleted)]
        public static async Task GuildDeleted(DiscordClient y, GuildDeleteEventArgs e)
        {
            const ulong guildid = 402458071349067777;
            const ulong channelid = 405516487332462612;
            try
            {
                var chn = await y.GetGuildAsync(guildid);

                var GuildLeft = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(0xFF8A80))
                    .WithAuthor($"Owner: {e.Guild.Owner.Username}#{e.Guild.Owner.Discriminator}",
                        icon_url: e.Guild.IconUrl)
                    .WithDescription(Formatter.BlockCode($"{e.Guild.Name}({e.Guild.Id}) Left"));

                await chn.GetChannel(channelid).SendMessageAsync(embed: GuildLeft);
                await y.UpdateStatusAsync(new DiscordActivity(type: ActivityType.Watching,
                    name: $"Yone({y.Guilds.Count} servers)"));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);

                throw;
            }
        }

        [AsyncListener(EventTypes.CommandErrored)]
        public static async Task CommandErrored(DiscordClient y, CommandErrorEventArgs e)
        {
            try
            {
                const ulong guildid = 402458071349067777;
                var chn = await y.GetGuildAsync(guildid);
                const ulong channelid = 402460181511929857;
                var stack = e.Exception.StackTrace.Replace("---", "");
                var Embed1 = new DiscordEmbedBuilder()
                    .WithAuthor($"{e.Context.User.FullDiscordName()} has caused a error!",
                        icon_url: e.Context.User.AvatarUrl)
                    .WithColor(DiscordColor.Red)
                    .WithDescription($"**Command used was:** `{e.Command.Parent} Command({e.Command.Name})`\n" +
                                     $"**Guild Command was used in:** `{e.Context.Guild.Name}`")
                    .AddField("Error Message",
                        $"{$"+{e.Exception.Message.Truncate(1000)}".BlockCode_DIFF()}")
                    .AddField("Error Stacktrace", $"{$"-{stack.Truncate(1000)}".BlockCode_DIFF()}")
                    .AddField("Message Content", $"`{e.Context.User.FullDiscordName()} has typed this:` \n" +
                                                 "```diff\n" +
                                                 $"-{e.Context.Message.Content}" +
                                                 $"```");
                await chn.GetChannel(channelid).SendMessageAsync(embed: Embed1);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}