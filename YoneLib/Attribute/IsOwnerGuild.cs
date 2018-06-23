using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace YoneLib.Attribute
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public sealed class IsOwnerGuild : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext c, bool help)
        {
            const ulong OwnerGuild = 402458071349067777;
            const ulong officalOwnerTestGuild = 404816627243417601;

            if (c.Guild.Id != OwnerGuild || c.Guild.Id != officalOwnerTestGuild)
                return await Task.FromResult(c.Guild.Id == OwnerGuild || c.Guild.Id == officalOwnerTestGuild);
            return await Task.FromResult(c.Guild.Id == OwnerGuild || c.Guild.Id == officalOwnerTestGuild);
        }
    }
}