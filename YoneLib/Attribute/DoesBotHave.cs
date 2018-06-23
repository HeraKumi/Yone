using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace YoneAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public sealed class DoesBotHave : CheckBaseAttribute
    {
        public DoesBotHave(Permissions permissions)
        {
            Permissions = permissions;
        }

        public Permissions Permissions { get; }
        public bool IgnoreDms { get; } = true;

        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            // This will ignore the permission check
            if (ctx.Guild == null)
                return IgnoreDms;

            // This will
            var bot = await ctx.Guild.GetMemberAsync(ctx.Client.CurrentUser.Id).ConfigureAwait(false);
            if (bot == null)
                return false;

            // Checks if the bot is equal to owner ID return true meaning the check passes
            if (bot.Id == ctx.Guild.Owner.Id)
                return true;

            // gets permissions for the bot
            var pbot = ctx.Channel.PermissionsFor(bot);

            // true = 0, false = 1 if the bot doesn't equal 1 return check pass;
            if ((pbot & Permissions.Administrator) != 0)
                return true;

            // true = 0, false = 1 if the bot doesn't have that permission(1) return error else
            // return true check passing
            return (pbot & Permissions) != 0;

            /*var DoesntHavePerms = new DiscordEmbedBuilder()
                .WithColor(new DiscordColor(0xFF8A80))
                .WithDescription($"`{ctx.Command.QualifiedName}` Requires the permission `{Permissions}`\n" +
                                 $"Please make sure you have given Yone the right permissions before using this command again");
            await ctx.RespondAsync(embed: DoesntHavePerms);*/
        }
    }
}