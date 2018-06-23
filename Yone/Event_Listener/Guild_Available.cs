using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Extended.AsyncListeners;
using YoneLib;
using YoneSql;

namespace Yone.Event_Listener
{
    public class Guild_Available
    {
        public class EventListeners
        {
            [AsyncListener(EventTypes.GuildAvailable)]
            public static async Task GuildAvailable(DiscordClient y, GuildCreateEventArgs e)
            {
                await y.UpdateStatusAsync(new DiscordActivity(type: ActivityType.Watching,
                    name: $"{y.CurrentApplication.Name}({y.Guilds.Count} servers)"));

                var AppName = $"[{e.Client.CurrentApplication.Name}]";
                var DateTimeNow = $"[{DateTime.Now}]";
                var DateGuild = $"Guild Name: {e.Guild.Name}\n" + $"Guild ID:   {e.Guild.Id}\n" +
                                $"Users:      {e.Guild.MemberCount}\n" + $"Large:      {e.Guild.IsLarge}\n" +
                                "---------------------------------------";

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"{AppName} {DateTimeNow} \n{DateGuild}");
                Console.ResetColor();
                try
                {
                    var data = new Global().GetDBRecords(e.Guild.Id);

                    if (data.Id == 0)
                    {
                        //this just checks if all guilds have a database entry if not then proceed to the next code block to create a new database
                    }
                    else
                    {
                        await Database.CreateDatabase(e.Guild.Id, $"{e.Guild.Owner}");
                    }
                }
                catch (Exception exception)
                {
                    if (exception.Message.Contains("Sequence contains no elements"))
                    {
                        await Database.CreateDatabase(e.Guild.Id, $"{e.Guild.Owner}");
                    }
                    else
                    {
                        Console.WriteLine(exception);
                        throw;
                    }
                }
            }
        }
    }
}