using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using YoneLib;

namespace Yone.Components
{
    public class Administrator : BaseCommandModule
    {
        [Command("ban")]
        public async Task BanMember(CommandContext c, DiscordMember m, [RemainingText] string reason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reason))
                {
                    await c.RespondAsync($"Please make sure you state a reason for the ban: {c.User.FullDiscordName()}");
                    return;
                }

                if (m.Id == c.User.Id)
                {
                    await c.RespondAsync("Cannot ban yourself.");
                    return;
                }
    
                var data = new Global().GetDBRecords(c.Guild.Id);
                var channelID = Convert.ToUInt64(data.ModerationChannel);

                if (data.ModerationChannel != "Moderation channel hasn't been set up yet.")
                {
                    await c.Guild.GetChannel(channelID).SendMessageAsync($"`{c.User.FullDiscordName()}`: Banned {m.FullDiscordName()} for `{reason}`");
                    await m.RemoveAsync();
                } else if (data.ModerationChannel == "Moderation channel hasn't been set up yet.")
                {
                    await c.RespondAsync($"`Banned`: {m.DisplayName}");
                    await m.RemoveAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("idban")]
        public async Task IdBanUser(CommandContext c, [RemainingText] ulong id)
        {
            try
            {
                if (id == c.User.Id)
                {
                    await c.RespondAsync("Cannot ban yourself.");
                    return;
                }
    
                var data = new Global().GetDBRecords(c.Guild.Id);
                var channelID = Convert.ToUInt64(data.ModerationChannel);

                if (data.ModerationChannel != "Moderation channel hasn't been set up yet.")
                {
                    await c.Guild.GetChannel(channelID).SendMessageAsync($"`{c.User.FullDiscordName()}`: Banned id `\"{id}\"`");
                    await c.Guild.BanMemberAsync(id, 7);
                } else if (data.ModerationChannel == "Moderation channel hasn't been set up yet.")
                {
                    await c.RespondAsync($"`Banned`: {id} from server");
                    await c.Guild.BanMemberAsync(id, 7);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}