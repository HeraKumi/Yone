using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Extended.AsyncListeners;
using YoneLib;

namespace Yone.Event_Listener
{
    public class Messages
    {
        //Channels
        [AsyncListener(EventTypes.ChannelCreated)]
        public static async Task NewChannelCreated(DiscordClient _, ChannelCreateEventArgs c)
        {
            var data = new Global().GetDBRecords(c.Guild.Id);

            var channelId = Convert.ToUInt64(data.LogChannel);

            var channelCreatedEmbed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Azure)
                .WithDescription($"New Channel: {c.Channel.Mention} Has been **created**!");
            await c.Guild.GetChannel(channelId).SendMessageAsync(embed: channelCreatedEmbed);
        }

        [AsyncListener(EventTypes.ChannelDeleted)]
        public static async Task ChannelDeleted(DiscordClient _, ChannelDeleteEventArgs c)
        {
            var data = new Global().GetDBRecords(c.Guild.Id);

            var channelId = Convert.ToUInt64(data.LogChannel);

            var channelDeletedEmbed = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Red)
                .WithDescription($"`Channel:` **{c.Channel.Name}** Has been **deleted**!\n");
            await c.Guild.GetChannel(channelId).SendMessageAsync(embed: channelDeletedEmbed);
        }

        //Messages
        [AsyncListener(EventTypes.MessageUpdated)]
        public static async Task MessageBeenUpdated(DiscordClient _, MessageUpdateEventArgs e)
        {
            try
            {
                var data = new Global().GetDBRecords(e.Guild.Id);


                var channelId = Convert.ToUInt64(data.LogChannel);

                var embed1 = new DiscordEmbedBuilder()
                    .WithAuthor($"{e.Author.FullDiscordName()} Edited a message!", icon_url: e.Author.AvatarUrl)
                    .WithColor(DiscordColor.Orange)
                    .WithDescription($"**Channel message edited in:** {e.Channel.Mention}\n" +
                                     $"**Time Message Created:** {e.Message.CreationTimestamp.ToLocalTime():hh:mm:ss tt} on {e.Message.CreationTimestamp.LocalDateTime.DayOfWeek}\n" +
                                     $"**Time Message Edited:** {e.Message.EditedTimestamp:hh:mm:ss tt} on {e.Message.EditedTimestamp.LocalDateTime.DayOfWeek}")
                    .AddField("Message", $"**Before: {e.MessageBefore.Content.Length}**\n" +
                                         $"{$"-{e.MessageBefore.Content.Truncate(250)}".BlockCode_DIFF()}\n" +
                                         $"**After: {e.Message.Content.Length}**\n" +
                                         $"{$"+{e.Message.Content.Truncate(250)}".BlockCode_DIFF()}");
                await e.Guild.GetChannel(channelId).SendMessageAsync(embed: embed1);
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("Input string was not in a correct format."))
                {
                    
                }
                else
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        [AsyncListener(EventTypes.MessageDeleted)]
        public static async Task MessageBeenDeleted(DiscordClient _, MessageDeleteEventArgs e)
        {
            var data = new Global().GetDBRecords(e.Guild.Id);


            var channelId = Convert.ToUInt64(data.LogChannel);

            var embed1 = new DiscordEmbedBuilder()
                .WithAuthor($"{e.Message.Author.FullDiscordName()} Deleted a message!",
                    icon_url: e.Message.Author.AvatarUrl)
                .WithColor(DiscordColor.IndianRed)
                .WithDescription($"**Channel message deleted in:** {e.Channel.Mention}\n" +
                                 $"**Time Message Created:** {e.Message.CreationTimestamp.ToLocalTime():hh:mm:ss tt} on {e.Message.CreationTimestamp.LocalDateTime.DayOfWeek}\n" +
                                 $"**Time Message Deleted:** {DateTime.Now.ToLocalTime():hh:mm:ss tt} on {DateTime.Now.ToLocalTime().DayOfWeek}")
                .AddField("Message", $"{$"-{e.Message.Content.Truncate(250)}".BlockCode_DIFF()}");
            await e.Guild.GetChannel(channelId).SendMessageAsync(embed: embed1);
        }

        [AsyncListener(EventTypes.MessageCreated)]
        public static async Task MessageLogged(DiscordClient _, MessageCreateEventArgs msg)
        {
            await YoneSql.Messages.CreateDatabase(msg.Message.Author.FullDiscordName(), msg.Guild.Name,
                msg.Message.Content);
        }
    }
}