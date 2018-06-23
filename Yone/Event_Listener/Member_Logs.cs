using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Extended.AsyncListeners;
using DSharpPlus.ModernEmbedBuilder;
using YoneLib;
using static System.IO.Path;

namespace Yone.Event_Listener
{
    public class Member_logs
    {
        [AsyncListener(EventTypes.GuildMemberAdded)]
        public static async Task YoneSendWelcomeMessage(DiscordClient y, GuildMemberAddEventArgs e)
        {
            try
            {
                var data = new Global().GetDBRecords(e.Guild.Id);

                #region main

                var channelID = Convert.ToUInt64(data.WelcomeChannel);

                var m = e.Member;

                var s = data.WelcomeMessage;

                var sb = new StringBuilder(s);

                #endregion

                #region spam

                sb.Replace("{Guild_Member_Count}", $"{e.Guild.MemberCount}");
                sb.Replace("{Guild_Verification_Level}", $"{e.Guild.VerificationLevel}");
                sb.Replace("{Guild_Member_Count}", $"{e.Guild.MemberCount}");
                sb.Replace("{Guild_Name}", $"{e.Guild.Name}");
                sb.Replace("{Guild_Id}", $"{e.Guild.Id}");
                sb.Replace("{Guild_Owner_Username}", $"{e.Guild.Owner.Username}#{e.Guild.Owner.Discriminator}");
                sb.Replace("{Guild_Owner_Mention}", $"{e.Guild.Owner.Mention}");
                sb.Replace("{Member_Mention}", $"{m.Mention}");
                sb.Replace("{Member_Username}", $"{m.Username}#{m.Discriminator}");
                sb.Replace("{Member_Id}", $"{m.Id}");
                sb.Replace("{Member_Discriminator}", $"{m.Discriminator}");
                sb.Replace("{Member_AvatarHash}", $"{m.AvatarHash}");
                sb.Replace("{Member_Color}", $"{m.Color}");
                sb.Replace("{Member_DisplayName}", $"{m.DisplayName}");

                #endregion

                if (e.Member.IsBot)
                {
                    var welcomeMessageEmbed = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xB9F6CA))
                        .WithAuthor(icon_url: m.AvatarUrl, name: "Bot Joined.")
                        .WithDescription($"{sb}")
                        .WithFooter(
                            $"Guild Member Count: {e.Guild.MemberCount} - {m.Id} - Joined {m.JoinedAt.DayOfWeek} at {m.JoinedAt:hh:mm:ss tt} or {m.JoinedAt.Date:dd.MM.yyyy}");
                    await e.Guild.GetChannel(channelID).SendMessageAsync(embed: welcomeMessageEmbed);
                }
                else if (e.Member.IsBot == false)
                {
                    /*var welcomeMessageEmbed = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xB9F6CA))
                        .WithAuthor(icon_url: m.AvatarUrl, name: "Member Joined.")
                        .WithDescription($"{sb}")
                        .WithFooter(
                            $"Guild Member Count: {e.Guild.MemberCount} - {m.Id} - Joined {m.JoinedAt.DayOfWeek} at {m.JoinedAt:hh:mm:ss tt} or {m.JoinedAt.Date:dd.MM.yyyy}");
                    await e.Guild.GetChannel(channelID).SendMessageAsync(embed: welcomeMessageEmbed);*/ // For Main Bot

                    #region MyRegion

                    var responses = new List<string>();
                    responses.AddRange(new[]
                    {
                        "https://a.safe.moe/kmG0xLD.gif",
                        "https://a.safe.moe/yaOf5Ma.gif",
                        "https://a.safe.moe/cTOZb5I.gif",
                        "https://a.safe.moe/6daGDAc.gif",
                        "https://a.safe.moe/tnCTpac.gif",
                        "https://a.safe.moe/7pCGOvD.gif",
                        "https://a.safe.moe/3XRaewJ.gif",
                        "https://a.safe.moe/wrhueTD.gif",
                        "https://a.safe.moe/J8tNZ6J.gif"
                    });
                    var random = new Random();
                    await new ModernEmbedBuilder
                    {
                        Description =
                            $"Welcome to `{e.Guild.Name}`, Thanks for joining! Please make sure to read the **rules**! <#454187686157221899>",
                        Color = 0xF48FB1,
                        ImageUrl = responses[random.Next(0, responses.Count)],
                        Content = $"{data.WelcomeMessage}",
                        FooterText = $"Owner is: {e.Guild.Owner.FullDiscordName()}"
                    }.Send(e.Guild.GetChannel(channelID));

                    #endregion
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        [AsyncListener(EventTypes.GuildMemberRemoved)]
        public static async Task YoneSendLeaveMessage(DiscordClient y, GuildMemberRemoveEventArgs e)
        {
            try
            {
                var data = new Global().GetDBRecords(e.Guild.Id);


                #region main

                var channelID = Convert.ToUInt64(data.WelcomeChannel);

                var m = e.Member;

                var s = data.WelcomeMessage;

                var sb = new StringBuilder(s);

                #endregion

                #region spam

                sb.Replace("{Guild_Member_Count}", $"{e.Guild.MemberCount}");
                sb.Replace("{Guild_Verification_Level}", $"{e.Guild.VerificationLevel}");
                sb.Replace("{Guild_Member_Count}", $"{e.Guild.MemberCount}");
                sb.Replace("{Guild_Name}", $"{e.Guild.Name}");
                sb.Replace("{Guild_Id}", $"{e.Guild.Id}");
                sb.Replace("{Guild_Owner_Username}", $"{e.Guild.Owner.Username}#{e.Guild.Owner.Discriminator}");
                sb.Replace("{Guild_Owner_Mention}", $"{e.Guild.Owner.Mention}");
                sb.Replace("{Member_Mention}", $"{m.Mention}");
                sb.Replace("{Member_Username}", $"{m.Username}#{m.Discriminator}");
                sb.Replace("{Member_Id}", $"{m.Id}");
                sb.Replace("{Member_Discriminator}", $"{m.Discriminator}");
                sb.Replace("{Member_AvatarHash}", $"{m.AvatarHash}");
                sb.Replace("{Member_Color}", $"{m.Color}");
                sb.Replace("{Member_DisplayName}", $"{m.DisplayName}");

                #endregion


                if (e.Member.IsBot)
                {
                    var welcomeMessageEmbed = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xB9F6CA))
                        .WithAuthor(icon_url: m.AvatarUrl, name: "Bot left.")
                        .WithDescription($"{sb}")
                        .WithFooter(
                            $"Guild Member Count: {e.Guild.MemberCount} - {m.Id} - Left {DateTime.Now.DayOfWeek} at {DateTime.Now:hh:mm:ss tt} or {DateTime.Now.Date:dd.MM.yyyy}");
                    await e.Guild.GetChannel(channelID).SendMessageAsync(embed: welcomeMessageEmbed);
                }
                else if (e.Member.IsBot == false)
                {
                    var LeaveMessageEmbed = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xFF8A80))
                        .WithAuthor(icon_url: m.AvatarUrl, name: "Member left.")
                        .WithDescription($"{sb}")
                        .WithFooter(
                            $"Guild Member Count: {e.Guild.MemberCount} - {m.Id} - Left {DateTime.Now.DayOfWeek} at {DateTime.Now:hh:mm:ss tt} or {DateTime.Now.Date:dd.MM.yyyy}");
                    await e.Guild.GetChannel(channelID).SendMessageAsync(embed: LeaveMessageEmbed);
                }
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("Input string was not in a correct format"))
                {
                }
                else
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        private string ReactionGifRandom()
        {
            string file = null;


            var path = GetDirectoryName(Assembly.GetEntryAssembly().Location);
            path = Combine(path, "Yone Resources/EventLogs/Welcome");

            var extensions = new[] {".png", ".jpg", ".gif"};
            {
                var di = new DirectoryInfo(path);
                var rgFiles = di.GetFiles("*.*").Where(f => extensions.Contains(f.Extension.ToLower()));

                var R = new Random();

                file = rgFiles.ElementAt(R.Next(0, rgFiles.Count())).FullName;
            }

            return file;
        }
    }
}