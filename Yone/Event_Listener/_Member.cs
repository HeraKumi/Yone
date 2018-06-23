using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.EventArgs;
using DSharpPlus.Extended.AsyncListeners;
using YoneLib;

namespace Yone.Event_Listener
{
    public class _Member
    {
        [AsyncListener(EventTypes.GuildMemberAdded)]
        public static async Task NewMember(DiscordClient y, GuildMemberAddEventArgs e)
        {
            try
            {
                var data = new Global().GetDBRecords(e.Guild.Id);

                if (e.Member.IsBot)
                {
                    var m = e.Member;
                    var p = Convert.ToUInt64(data.BotAutoRole);
                    var c = e.Member.Guild.GetRole(p);
                    await m.GrantRoleAsync(c);
                }
                else if (e.Member.IsBot == false)
                {
                    var p = Convert.ToUInt64(data.Autorole);
                    var c = e.Member.Guild.GetRole(p);
                    var m = e.Member;
                    await m.GrantRoleAsync(c);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }
    }
}