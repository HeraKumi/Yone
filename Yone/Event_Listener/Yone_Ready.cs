using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Extended.AsyncListeners;
using Flurl.Http;
using YoneLib;

namespace Yone.Event_Listener
{
    public class YoneReady
    {
        [AsyncListener(EventTypes.Ready)]
        public static async Task YoneIsReady(DiscordClient y, ReadyEventArgs r)
        {
            await y.UpdateStatusAsync(new DiscordActivity(type: ActivityType.Watching,
                name: $"{y.CurrentApplication.Name}({y.Guilds.Count} servers)"));

            r.Client.DebugLogger.LogMessage(LogLevel.Info, "Yone",
                $"{y.CurrentApplication.Name} is ready, and is in {y.Guilds.Count} servers", DateTime.Now);


            try
            {
                var data = new Global().DefaultDatabase();
                await $"https://discordbots.org/api/bots/{r.Client.CurrentUser.Id}/stats"
                    .WithHeader("Authorization", data.dboToken)
                    .PostUrlEncodedAsync(new {server_count = $"{r.Client.Guilds.Count}"});
            }
            catch (FlurlHttpException e)
            {
                const string discordBotsorgAuthKeyError =
                    "\n1) You have either NOT have created a account on https://www.discordbots.org\n" +
                    "2) Api auth code is not right. Update the settings in the main menu!";
                r.Client.DebugLogger.LogMessage(LogLevel.Critical, "DB Key API Error", discordBotsorgAuthKeyError,
                    DateTime.Now);
            }
        }
    }
}