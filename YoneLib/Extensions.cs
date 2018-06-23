using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Flurl.Http;
using static System.String;

namespace YoneLib
{
    public static class Extensions
    {
        public static ulong TestSpamChannel => 402460240022470668;

        public static bool StartsWithUpper(this string str)
        {
            if (IsNullOrWhiteSpace(str))
                return false;

            var ch = str[0];
            return char.IsUpper(ch);
        }

        public static string Write(this string str)
        {
            Console.WriteLine(str);
            var write = str;
            return write;
        }

        public static async Task<string> MeApi(this CommandContext c, string s)
        {
            var Results = await $"https://rra.ram.moe/i/r/?type={c.Command.Name}"
                .GetStringAsync();
            return Results;
        }

        public static string Owner(this DiscordClient owner)
        {
            return "- ̗̀Hera ̖́-#0002(140066850556870656)";
        }
        
        public static bool CheckUrlValid(this string source)
        {
            return Uri.TryCreate(source, UriKind.Absolute, out var uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
        }

        public static string Truncate(this string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        public static string BlockCode_ASCIIDOC(this string content)
        {
            return Formatter.BlockCode($"{content}", "asciidoc");
        }

        public static string BlockCode_DIFF(this string content)
        {
            return Formatter.BlockCode($"{content}", "diff");
        }

        public static string FullDiscordName(this DiscordUser user)
        {
            return $"{user.Username}#{user.Discriminator}";
        }

        public static bool NonCaseSensitive(this string a, string b)
        {
            return a.Equals(b, StringComparison.OrdinalIgnoreCase);
        }
    }
}