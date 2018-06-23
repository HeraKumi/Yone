using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.ModernEmbedBuilder;
using MongoDB.Bson;
using MongoDB.Driver;
using YoneAttributes;
using YoneLib;
using YoneLib.Api;
using YoneLib.Attribute;

namespace Yone.Components
{
    [Group("mod")]
    [IsUserBlacklisted]
    public class Moderator : BaseCommandModule
    {
        [Command("announce")]
        [Description("Sends a announcement")]
        [DoesBotHave(Permissions.SendMessages)]
        [DoesUserHave(Permissions.SendMessages)]
        public async Task AnnouceToChannel(CommandContext c,
            [Description(" the message you want to announce to everyone")] [RemainingText]
            string AnnouceMessage)
        {
            var data = new Global().GetDBRecords(c.Guild.Id);

            var channelID = Convert.ToUInt64(data.AnnouncementChannel);
            try
            {
                var annoucementMessage = new DiscordEmbedBuilder()
                    .WithAuthor($"New announcement by: {c.User.Username}", icon_url: c.User.AvatarUrl)
                    .WithDescription($"{AnnouceMessage}")
                    .WithColor(DiscordColor.HotPink)
                    .WithFooter($"Time sent at: {DateTimeOffset.UtcNow} : Universal Time", c.Guild.IconUrl);

                try
                {
                    await c.Guild.GetChannel(channelID)
                        .SendMessageAsync(embed: annoucementMessage, content: "@everyone");
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Unauthorized: 403"))
                        await c.RespondAsync(
                            "Please make sure the bot has permissions to send a announcement to that channel.");

                    if (e.Message.Contains("Input string was not in a correct format."))
                    {
                        await c.RespondAsync(
                            "Please make sure you have set a channel to send announcements to.\n" +
                            "To check if you have set on or not please do `>guild list`");
                    }
                    else
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                await c.RespondAsync("sent");
            }
            catch (Exception e)
            {
                if (e.Message.Contains($"no such table: Guild: {c.Guild.Id}"))
                {
                    await c.RespondAsync(
                        "You must create the guild settings file to use this command, To create the file please use >guild create");
                }
                else if (e.Message.Contains("No current row"))
                {
                    await c.RespondAsync(
                        "You must set up a announcement channel, To setup the channel please do >guild SetAnnounceChannel #channelName");
                }
                else if (!e.Message.Contains($"no such table: Guild: {c.Guild.Id}"))
                {
                    Console.WriteLine(e);
                    throw;
                }
                else if (e.Message.Contains("Unauthorized: 403"))
                {
                    await c.RespondAsync("Bot doesn't have permissions. :sob:");
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [Command("clean")]
        [DoesBotHave(Permissions.ManageMessages)]
        [DoesUserHave(Permissions.ManageMessages)]
        [Description("Delete an amount of messages from the current channel.")]
        public async Task CleanMessageFromaChannel(CommandContext x,
            [Description("Amount of messages to remove (max 100)")]
            int limit = 10, [Description("Amount of messages to skip")]
            int skip = 0)
        {
            try
            {
                var data = new Global().GetDBRecords(x.Guild.Id);
                var channelID = Convert.ToUInt64(data.ModerationChannel);

                if (data.ModerationChannel != "Moderation channel hasn't been set up yet.")
                {
                    var i = 0;
                    var ms = await x.Channel.GetMessagesBeforeAsync(x.Message.Id, limit);
                    var deletThis = new List<DiscordMessage>();
                    foreach (var m in ms)
                        if (i < skip)
                            i++;
                        else
                            deletThis.Add(m);
                    if (deletThis.Any())
                        await x.Channel.DeleteMessagesAsync(deletThis);
                    await x.Guild.GetChannel(channelID).SendMessageAsync($"`{x.User.FullDiscordName()}` deleted: {limit} messages and skipped over {skip} messages.");
                } else if (data.ModerationChannel == "Moderation channel hasn't been set up yet.")
                {
                    var i = 0;
                    var ms = await x.Channel.GetMessagesBeforeAsync(x.Message.Id, limit);
                    var deletThis = new List<DiscordMessage>();
                    foreach (var m in ms)
                        if (i < skip)
                            i++;
                        else
                            deletThis.Add(m);
                    if (deletThis.Any())
                        await x.Channel.DeleteMessagesAsync(deletThis);
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Bad request: 400"))
                {
                    var error400 = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xFF8A80))
                        .WithDescription(
                            $"`{x.Client.CurrentApplication.Name} can only delete messages that are < 7 days old` :sob:");
                    await x.RespondAsync(embed: error400);
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [Command("kick")]
        public async Task KickMember(CommandContext c, DiscordMember m, [RemainingText]string reason)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(reason))
                {
                    await c.RespondAsync($"Please make sure you state a reason for the kick: {c.User.FullDiscordName()}");
                    return;
                }

                if (m.Id == c.User.Id)
                {
                    await c.RespondAsync("Cannot kick yourself.");
                    return;
                }
    
                var data = new Global().GetDBRecords(c.Guild.Id);
                var channelID = Convert.ToUInt64(data.ModerationChannel);

                if (data.ModerationChannel != "Moderation channel hasn't been set up yet.")
                {
                    await c.Guild.GetChannel(channelID).SendMessageAsync($"`{c.User.FullDiscordName()}`: kicked {m.FullDiscordName()} for `{reason}`");
                    await m.RemoveAsync();
                } else if (data.ModerationChannel == "Moderation channel hasn't been set up yet.")
                {
                    await c.RespondAsync($"`Kicked`: {m.DisplayName}");
                    await m.RemoveAsync();
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