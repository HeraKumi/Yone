using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace YoneAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public sealed class DoesUserHave : CheckBaseAttribute
    {
        public DoesUserHave(Permissions permissions)
        {
            Permissions = permissions;
        }

        public Permissions Permissions { get; }
        public bool IgnoreDms { get; } = true;

        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            if (ctx.Guild == null)
                return Task.FromResult(IgnoreDms);

            var usr = ctx.Member;
            if (usr == null)
                return Task.FromResult(false);

            if (usr.Id == ctx.Guild.Owner.Id)
                return Task.FromResult(true);

            var pusr = ctx.Channel.PermissionsFor(usr);

            return (pusr & Permissions.Administrator) != 0
                ? Task.FromResult(true)
                : Task.FromResult((pusr & Permissions) == Permissions);

            /*if ((pusr & this.Permissions) != this.Permissions)
            {
                var DoesntHavePerms = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(0xFF8A80))
                    .WithDescription($"{ctx.User.Mention} you need `{Permissions}` in order to use this command");
                ctx.RespondAsync(embed: DoesntHavePerms);
            }*/
        }
    }
}