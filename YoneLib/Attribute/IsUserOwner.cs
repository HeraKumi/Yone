using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace YoneAttributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public sealed class IsUserOwner : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            var app = ctx.Client.CurrentApplication;
            var me = ctx.Client.CurrentUser;
            ulong sec = 278409771319820299;

            if (app != null)
                return await Task.FromResult(ctx.User.Id == app.Owner.Id || ctx.User.Id == sec);

            return await Task.FromResult(ctx.User.Id == me.Id || ctx.User.Id == sec);
        }
    }
}