using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Extended.AsyncListeners;
using Flurl.Http;
using YoneLib;
using YoneSql;

namespace Yone.Event_Listener
{
    public class _GuildAdded
    {
        [AsyncListener(EventTypes.GuildCreated)]
        public static async Task GuildCreated(DiscordClient y, GuildCreateEventArgs g)
        {
            try
            {
                var data = new Global().DefaultDatabase();
                await $"https://discordbots.org/api/bots/{g.Client.CurrentUser.Id}/stats"
                    .WithHeader("Authorization", data.dboToken)
                    .PostUrlEncodedAsync(new {server_count = $"{g.Client.Guilds.Count}"});
            }
            catch (FlurlHttpException e)
            {
                const string discordBotsorgAuthKeyError =
                    "1) You have either NOT have created a account on https://www.discordbots.org\n" +
                    "2) Api auth code is not right, You can edit the configuration file in the folder named: \"Configuration\" file name \"config.json\" line: \"3\" ";
                Console.WriteLine(discordBotsorgAuthKeyError);
            }

            await y.UpdateStatusAsync(new DiscordActivity(type: ActivityType.Watching,
                name: $"{y.CurrentApplication.Name}({y.Guilds.Count} servers)"));
            var AppName = $"[{g.Client.CurrentApplication.Name}]";
            var DateTimeNow = $"[{DateTime.Now}]";
            var DateGuild = $"Guild Name: {g.Guild.Name}\n" + $"Guild ID:   {g.Guild.Id}\n" +
                            $"Users:      {g.Guild.MemberCount}\n" + $"Large:      {g.Guild.IsLarge}\n" +
                            "---------------------------------------";

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{AppName} {DateTimeNow} \n{DateGuild}");
            Console.ResetColor();

            try
            {
                await Database.CreateDatabase(g.Guild.Id, $"{g.Guild.Owner}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}