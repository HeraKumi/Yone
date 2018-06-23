using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Yone.Api;
using YoneLib.Attribute;

namespace Yone.Components
{
    [Group("fun")]
    [Description(
        "Below you will see all available commands, there are in this group to use them, please use `>fun command` the `command` will the one of the commands listed below\n\n" +
        "if you need help with one of the commands please use the follow command `>help fun command` **Remember** if you use \"command\" it will not work you have to use one of the commands below")]
    [IsUserBlacklisted]
    public class Amusement : BaseCommandModule
    {
        [Command("urban")]
        [Description("Want to find a word in the urban dictionary?")]
        public async Task Urban(CommandContext ctx,
            [Description(
                " For the string in this command, you will have to supply a word, if the word you have supplied is invalid or is not found an error will be thrown to let you know")]
            [RemainingText]
            string wordToDefine)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(wordToDefine))
                {
                    await ctx.RespondAsync("You cannot leave the word to define blank!");
                    return;
                }

                var r = (HttpWebRequest) WebRequest.Create(
                    $"http://api.urbandictionary.com/v0/define?term={wordToDefine}");
                r.Method = "GET";

                var rs = (HttpWebResponse) r.GetResponse();
                string result;

                using (var sr = new StreamReader(rs.GetResponseStream() ?? throw new InvalidOperationException()))
                {
                    result = sr.ReadToEnd();
                }

                var obj = JsonConvert.DeserializeObject<funAPI.UrbanApi>(result);

                var defined = new DiscordEmbedBuilder()
                    .WithColor(new DiscordColor(0xAA00FF))
                    .WithAuthor($"{obj.List[0].Author}", $"{obj.List[0].Permalink}")
                    .AddField($"{obj.List[0].Word}", $"{obj.List[0].Definition}", true)
                    .AddField("Example", $"{obj.List[0].Example}")
                    .WithFooter($"ID: {obj.List[0].Defid}");

                await ctx.RespondAsync(embed: defined);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Index was outside the bounds of the array."))
                {
                    var error = new DiscordEmbedBuilder()
                        .WithTitle("Error: See below")
                        .WithColor(new DiscordColor(0xD32F2F))
                        .AddField("Message",
                            $"You either entered an invalid word or the word you have entered(**{wordToDefine}**) couldn't be found",
                            true)
                        .AddField("Information",
                            "Please do `>help info urban`\nReference: `string` = A word or sentence you found.");
                    await ctx.RespondAsync(embed: error);
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [Command("comic")]
        [Description("want to read dumb comics well here you go.")]
        public async Task Comic(CommandContext ctx)
        {
            var rnd = new Random();

            var r = (HttpWebRequest) WebRequest.Create($"https://xkcd.com/{rnd.Next(0, 1935)}/info.0.json");
            r.Method = "GET";

            var rs = (HttpWebResponse) r.GetResponse();
            string result;

            using (var sr = new StreamReader(rs.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                result = sr.ReadToEnd();
            }

            var obj = JsonConvert.DeserializeObject<funAPI.Comics>(result);

            var comiC = new DiscordEmbedBuilder()
                .WithTitle($"| {obj.Title} |")
                .WithColor(new DiscordColor())
                .WithUrl($"{obj.Link}")
                .WithImageUrl($"{obj.Img}")
                .AddField("Alt", $"{obj.Alt}")
                .WithFooter($"{obj.Day}/{obj.Month}/{obj.Year}");
            await ctx.RespondAsync(embed: comiC);
        }

        [Command("checkinvis")]
        [Description("Tries to find invisible people on the server")]
        public async Task CheckInvisible(CommandContext c)
        {
            var invis = c.Guild.Members
                .Where(x => c.Client.Presences.ContainsKey(x.Id) &&
                            c.Client.Presences[x.Id].Status == UserStatus.Offline)
                .Select(x => x.Nickname ?? x.Username)
                .ToList();
            var are = invis.Count > 1 ? "are" : "is";
            var nameLine = "";

            if (invis.Count == 1)
                nameLine = invis[0];
            else if (invis.Count > 1)
                nameLine = $"{string.Join(", ", invis.Take(invis.Count - 1))} and {invis.Last()}";

            var checkPeople = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.HotPink)
                .WithDescription(Formatter.BlockCode(invis.Any()
                    ? $"{nameLine} {are} probably invisible"
                    : "Nobody appears to be invisible"));

            await c.RespondAsync(embed: checkPeople);
        }
    }
}