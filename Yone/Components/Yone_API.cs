using System;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using YoneLib.Attribute;

namespace Yone.Components
{
    [Group("api")]
    [IsUserBlacklisted]
    [Description("Bots internal Api document")]
    public class Yone_API : BaseCommandModule
    {
        [Command("GuildMessageApi")]
        [Aliases("gma")]
        [Description("Shows you what you can add to your welcome / leave guild messages")]
        public async Task Welcome_Leave_Api(CommandContext c)
        {
            try
            {
                var m = c.Member;

                var Welcome_Leave_API = new DiscordEmbedBuilder()
                    .WithAuthor($"「{c.Client.CurrentApplication.Name}'s Welcome/Leave Api」")
                    .WithColor(new DiscordColor(0x84FFFF))
                    .WithTitle($"Information about {c.Client.CurrentApplication.Name}'s Super Simple Api")
                    .WithDescription(
                        "**Please keep in mind this command is to be used as information for the welcome/leave messages**" +
                        "\nThe below will show you everything you can do to customize the welcome/leave messages.\n" +
                        "Example command usage:    `>guild setwelcomemessage Welcome {Member_Mention}`")
                    .AddField("Welcome / Leave",
                        $"**{{Guild_Member_Count}}**  =  *__{c.Guild.MemberCount}__* - This will display the guild member count\n" +
                        $"**{{Guild_Verification_Level}}**  =  *__{c.Guild.VerificationLevel}__* - This will check what verification level the guild is in\n" +
                        $"**{{Guild_Name}}**  =  *__{c.Guild.Name}__* - This will get and display the guild name\n")
                    .AddField("more..",
                        $"**{{Guild_Id}}**  =  *__{c.Guild.Id}__* - I have no idea why you would need this but ti will display the guild id\n" +
                        $"**{{Guild_Owner_Username}}**  =  *__{c.Guild.Owner.Username}#{c.Guild.Owner.Discriminator}__* - This will display the guild owner username\n" +
                        $"**{{Guild_Owner_Mention}}**  =  *__{c.Guild.Owner.Mention}__* - This will mention the guild owner name\n" +
                        $"**{{Member_Mention}}**  =  *__{m.Mention}__* - This will mention the member who joined\n" +
                        $"**{{Member_Username}}**  =  *__{m.Username}__* - This will just get the users username\n" +
                        $"**{{Member_Id}}**  =  *__{m.Id}__* - This will display the user who just joined user id\n" +
                        $"**{{Member_Discriminator}}**  =  *__{m.Discriminator}__* - This will get the users discrm aka #0002\n" +
                        $"**{{Member_AvatarHash}}**  =  *__{m.AvatarHash}__* - This will get the users avatar hash, which I have no idea why you would need this\n" +
                        $"**{{Member_Color}}**  =  *__{m.Color}__* - This will get the users color\n" +
                        $"**{{Member_DisplayName}}**  =  *__{m.DisplayName}__* - This will just display the user display-name\n");
                await c.RespondAsync(embed: Welcome_Leave_API);
            }
            catch (Exception e)
            {
                await c.RespondAsync($"{e}");
                throw;
            }
        }
    }
}