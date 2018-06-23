using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using YoneLib.Attribute;

namespace Yone.Components
{
    [Group("facts")]
    [Description("Just random facts about random things")]
    [IsUserBlacklisted]
    public class Facts : BaseCommandModule
    {
        [Command("number")]
        [Description("Random facts about any number")]
        public async Task NumberFacts(CommandContext ctx,
            [Description(
                " The string in this case will either be any number or the word random, This will display a random fact about a random number")]
            [RemainingText]
            string number)
        {
            var r = (HttpWebRequest) WebRequest.Create($"http://numbersapi.com/{number}");
            r.Method = "GET";

            var rs = (HttpWebResponse) r.GetResponse();
            string result;

            using (var sr = new StreamReader(rs.GetResponseStream() ?? throw new InvalidOperationException()))
            {
                result = sr.ReadToEnd();
            }

            var numFacts = new DiscordEmbedBuilder()
                .WithDescription($"{result}");
            await ctx.RespondAsync(embed: numFacts);
        }

        [Command("year")]
        [Description("Random facts about years")]
        public async Task YearFacts(CommandContext ctx,
            [Description(
                " To use this please follow the format `>facts year {year or random}` the year will be formatted like so: `1999 or 2 <-- for the year 0002` ")]
            [RemainingText]
            string yearOrRandom)
        {
            var r = (HttpWebRequest) WebRequest.Create($"http://numbersapi.com/{yearOrRandom}/year");
            r.Method = "GET";

            var rs = (HttpWebResponse) r.GetResponse();
            string result = null;

            using (var sr = new StreamReader(rs.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            var numFacts = new DiscordEmbedBuilder()
                .WithDescription($"{result}");
            await ctx.RespondAsync(embed: numFacts);
        }

        [Command("date")]
        [Description("Random facts about dates")]
        public async Task DateFacts(CommandContext ctx,
            [Description(
                " The date will either be `random` or a date. `Date` will be formatted like so: `(month 01)/(day 01)`")]
            [RemainingText]
            string dateOrRandom)
        {
            var r = (HttpWebRequest) WebRequest.Create($"http://numbersapi.com/{dateOrRandom}/date");
            r.Method = "GET";

            var rs = (HttpWebResponse) r.GetResponse();
            string result = null;

            using (var sr = new StreamReader(rs.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            var numFacts = new DiscordEmbedBuilder()
                .WithDescription($"{result}");
            await ctx.RespondAsync(embed: numFacts);
        }

        [Command("math")]
        [Description("Random facts about math")]
        public async Task MathFacts(CommandContext ctx)
        {
            var r = (HttpWebRequest) WebRequest.Create("http://numbersapi.com/random/math");
            r.Method = "GET";

            var rs = (HttpWebResponse) r.GetResponse();
            string result = null;

            using (var sr = new StreamReader(rs.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            var numFacts = new DiscordEmbedBuilder()
                .WithDescription($"{result}");
            await ctx.RespondAsync(embed: numFacts);
        }
    }
}