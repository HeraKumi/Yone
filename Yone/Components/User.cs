using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Yone.Api;
using YoneLib;
using YoneLib.Attribute;
using static System.IO.Path;

namespace Yone.Components
{
    //Most of the commands here are going to be repetitive as hell :(
    [Group("me")]
    [IsUserBlacklisted]
    [Description("Just some fun cute stuff")]
    public class User : BaseCommandModule
    {
        [Command("hug")]
        [Description("This command will allow you to virtual hug to a user")]
        public async Task HugUser(CommandContext c, [Description("Mention a user")] DiscordUser user)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath,
                    $"{c.Member.Mention} has given you a virtual hug: {user.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("cry")]
        [Description("Start crying :(")]
        public async Task StartCrying(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} has started crying!!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("cuddle")]
        [Description("This command will give you a chance to cuddle a user (virtually)")]
        public async Task CuddleUser(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} has started cuddling with {User.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("kiss")]
        [Description("Sends a virtual kiss to a user")]
        public async Task KissUser(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath,
                    $"{c.Member.Mention} **Gave** {User.Mention} **a kiss** :heart:");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("nom")]
        [Description("Start eating")]
        public async Task Nom(CommandContext c, [Description("Choose a user you want to NOM on")]
            DiscordUser User)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **NOOOMMM!!!** {User.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("nyan")]
        [Description("just NYAN")]
        public async Task Nyan(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **Nayaan!!!**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("pat")]
        [Description("Pats a user you want")]
        public async Task PatUser(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} gave {User.Mention}**a pat on the head**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("pout")]
        [Description("Starts pouting :(")]
        public async Task Pout(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **is pouting....**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("slap")]
        [Description("Just slaps a user")]
        public async Task SlapUser(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **Slapped** {User.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("smirk")]
        [Description("Heh, smirk")]
        public async Task Smug(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **smirk...**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("stare")]
        [Description("Stares at a user")]
        public async Task StaresAtUser(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **chi ~~~**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("tickle")]
        [Description("Starts tickling a user")]
        public async Task TicklesUser(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} has started to tickle {User.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("spank")]
        [Description("Spank another user")]
        public async Task spankUser(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} has **Spanked** {User.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("triggered")]
        [Description("I'm offended, TRIGGERED REEEEEE")]
        public async Task Triggered(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} is **TRIgGEeRRReeeeDDDDDDDD!!!!!!!!**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("disgusted")]
        public async Task Disgusted(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **EWWW**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("cheer")]
        public async Task Cheer(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **YAAAAAAAAY!!!!!!!!!!!**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("heh")]
        public async Task Heh(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **heh**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("laugh")]
        public async Task Laugh(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **AHahhaHAHAHHAAA**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("OMG")]
        public async Task OMG(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("peace")]
        public async Task Peace(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} **Peace**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("sad")]
        public async Task Sad(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention} I am sad...");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("shame")]
        public async Task Shame(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"{c.Member.Mention}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("sigh")]
        public async Task Sigh(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"**Haaaaa.....**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("smile")]
        public async Task Smile(CommandContext c)
        {
            try
            {
                var Filepath = ReactionGifRandom(c.Command.Name);

                await c.RespondWithFileAsync(Filepath, $"**Cheeezzzzzeeeee**");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // External Sources
        [Command("lewd")]
        [Description("That was lewd EWW")]
        public async Task ThatWasLews(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                //Variables to make code cleaner
                var CommandUser = c.User;

                //Get images
                var Results = c.MeApi($"{c.Command.Name}");
                // This the above will get parse into json objects below
                var Data = LewdApi.FromJson(await Results);

                //Results
                var Lewd = new DiscordEmbedBuilder().WithColor(new DiscordColor(0xFF4081))
                    .WithDescription($"{CommandUser.Mention} **That was Lewd** {User.Mention}")
                    .WithImageUrl($"https://rra.ram.moe" + Data.Path);

                //Send message
                await c.RespondAsync(embed: Lewd);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("lick")]
        [Description("Lick a user")]
        public async Task Lickuser(CommandContext c, [Description("Mention a user")] DiscordUser User)
        {
            try
            {
                //Variables to make code cleaner
                var CommandUser = c.User;

                //Get images
                var Results = c.MeApi($"{c.Command.Name}");
                // This the above will get parse into json objects below
                var Data = LickApi.FromJson(await Results);

                //Results
                var LicksUser = new DiscordEmbedBuilder().WithColor(new DiscordColor(0x82B1FF))
                    .WithDescription($"{CommandUser.Mention} **Licked** {User.Mention}")
                    .WithImageUrl($"https://rra.ram.moe" + Data.Path);

                //Send message
                await c.RespondAsync(embed: LicksUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("owo")]
        [Description("OwOoWOwOoWOwOoWOwOoWOwOoW")]
        public async Task Owo(CommandContext c)
        {
            try
            {
                //Variables to make code cleaner
                var CommandUser = c.User;

                //Get images
                var Results = c.MeApi($"{c.Command.Name}");
                // This the above will get parse into json objects below
                var Data = OwoApi.FromJson(await Results);

                //Results
                var Owo = new DiscordEmbedBuilder().WithColor(new DiscordColor(0x3949AB))
                    .WithDescription($"{CommandUser.Mention}")
                    .WithImageUrl($"https://rra.ram.moe" + Data.Path);

                //Send message
                await c.RespondAsync(embed: Owo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        // Internal Repeter image getter
        private string ReactionGifRandom(string CommandName)
        {
            string file = null;


            var path = GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = Combine(path, $"Yone Resources/Me - Reaction Pictures/{CommandName}");

            if (!string.IsNullOrEmpty(CommandName))
            {
                var extensions = new[] {".png", ".jpg", ".gif"};
                {
                    var di = new DirectoryInfo(path);
                    var rgFiles = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));

                    var R = new Random();

                    file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).FullName;
                }
            }

            return file;
        }
    }
}