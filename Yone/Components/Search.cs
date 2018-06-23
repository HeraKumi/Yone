using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Newtonsoft.Json;
using Yone.Api;
using YoneLib.Attribute;

namespace Yone.Components
{
    [Group("search")]
    [IsUserBlacklisted]
    [Description("Search for stuff with this command group")]
    public class Search : BaseCommandModule
    {
        [Command("github")]
        [Description("While using the github api, you will be about to search for peoples github profiles")]
        public async Task Github(CommandContext ctx,
            [Description("String in this case will be where the user goes.")] [RemainingText]
            string user)
        {
            try
            {
                user.Replace(" ", "-");

                var r = (HttpWebRequest) WebRequest.Create($"https://api.github.com/users/{user}");
                r.Accept = "application/json";
                r.UserAgent = "Foo";
                r.Method = "GET";

                var rs = (HttpWebResponse) r.GetResponse();
                string result;

                using (var sr = new StreamReader(rs.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }

                var obj = JsonConvert.DeserializeObject<InfoApi.GithubApi>(result);

                if (obj.Message.Contains("Not Found"))
                {
                    user.Replace("-", " ");
                    var NotFound = new DiscordEmbedBuilder()
                        .WithDescription($"`.`");
                    await ctx.RespondAsync(embed: NotFound);
                    return;
                }


                var bio = string.IsNullOrWhiteSpace(obj.Bio) || string.IsNullOrEmpty(obj.Bio)
                    ? $"{obj.Name} has not set a bio yet."
                    : obj.Bio;
                var name = string.IsNullOrWhiteSpace(obj.Name) || string.IsNullOrEmpty(obj.Name)
                    ? "Name could not be resolved!"
                    : obj.Name;

                var githubUser = new DiscordEmbedBuilder()
                    .WithAuthor(icon_url: $"{obj.AvatarUrl}", name: $"{name}", url: $"{obj.HtmlUrl}")
                    .AddField("Public Repository", $"{obj.PublicRepos}", true)
                    .AddField("Bio", $"{bio}", true)
                    .AddField("Account Created at", $"{obj.CreatedAt}", true)
                    .AddField("Followers", $"{obj.Followers}", true)
                    .AddField("Following", $"{obj.Following}", true);

                await ctx.RespondAsync(embed: githubUser);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("The remote server returned an error: (404) Not Found."))
                {
                    var cannotFindUser = new DiscordEmbedBuilder()
                        .AddField("Error",
                            $"The user({user}) you have searched for, could not be found! Please make sure you have the right username!");
                    await ctx.RespondAsync(embed: cannotFindUser);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}