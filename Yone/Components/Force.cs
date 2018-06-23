using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using YoneLib;
using YoneSql;

namespace Yone.Components
{
    [Group("force")]
    [Description("A series of commands to force a option")]
    [RequireOwner]
    public class Force : BaseCommandModule
    {
        [Command("prefix")]
        public async Task ChangePrefix(CommandContext c,
            [RemainingText] [Description("change the prefix of the discord bot")]
            string prefix)
        {
            try
            {
                await Database.ChangePrefix(c.Guild.Id, prefix);
                await c.RespondAsync($"You have force change my prefix to: `{prefix}` ~~~Rough");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    await Database.CreateDatabase(c.Guild.Id, $"{c.Guild.Owner}");
                    await c.RespondAsync(
                        $"I have created your guild settings, now you can redo the command `guild {c.Command.Name}`");
                    throw;
                }

                Console.WriteLine(e);
                throw;
            }
        }

        [Command("getguildids")]
        [Aliases("gids")]
        [Description(
            "a simple method to get all guild ID's the bot is connected to and list it out in a neat formatted block code!")]
        public Task getGuildList(CommandContext x)
        {
            var glist = x.Client.Guilds.Values.ToList();
            var s = new StringBuilder();
            foreach (var g in glist) s.AppendLine($"+{g.Id}      ::  {g.Name}\n");

            return x.RespondAsync($"{s}".BlockCode_DIFF());
        }
    }
}