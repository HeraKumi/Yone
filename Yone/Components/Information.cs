using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using Flurl.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Yone.Api;
using YoneLib;
using YoneLib.Api;
using YoneLib.Attribute;

namespace Yone.Components
{
    [Group("info")]
    [IsUserBlacklisted]
    [Description(
        "Below you find all commands within this group, to use them please do `>group command`"
        + "\nIf you still need help, you can use the help command like so `>help info command` This will send you information about the command, and how to use the command")]
    public class Information : BaseCommandModule
    {
        [Command("stats")]
        [Aliases("botinfo", "botstats")]
        [Description("This will provide you with stats about the bot")]
        public async Task Stats(CommandContext ctx)
        {
            try
            {
                var createdOn = DateTime.Parse("12/20/2017");
                var timeSince = DateTime.Now.Subtract(createdOn);

                var number = Process.GetCurrentProcess().Threads.Count;
                DiscordEmbed nice = new DiscordEmbedBuilder()
                    .WithAuthor(icon_url: "https://i.imgur.com/iESDOrc.jpg",
                        name: $"Yone | Currently playing in {ctx.Client.Guilds.Count} servers")
                    .WithThumbnailUrl($"{ctx.Client.CurrentUser.AvatarUrl}")
                    .WithTitle($"{ctx.Member.Username}: Here are the stats for {ctx.Client.CurrentApplication.Name}")
                    .WithColor(new DiscordColor(0xBA68C8))
                    .AddField("Version", $"DSharpPlus: {ctx.Client.VersionString}\n" +
                                         $"GateWay: {ctx.Client.GatewayVersion}\n" +
                                         $"{ctx.Client.CurrentApplication.Name}: {Assembly.GetEntryAssembly().GetName().Version}",
                        true)
                    .AddField("Bot Information",
                        $"Memory Usage: {getHeapSize()}Mb\n" +
                        $"Threads: {number}\n" +
                        $"Ping: {ctx.Client.Ping}ms\n" +
                        $"Uptime: {getUptime()}\n" +
                        "Creator: <@140066850556870656> | <@278409771319820299>\n" +
                        "Debuggers: <@214568791412178944> | <@238684373732294658>\n" +
                        "Server Support: [Click me](https://discord.gg/hsptVh4)\n" +
                        $"Invite {ctx.Client.CurrentApplication.Name}: [Click me](https://discordapp.com/api/oauth2/authorize?client_id=394058464080429059&permissions=2146958583&scope=bot)",
                        true)
                    .AddField("General Information", $"Servers: {ctx.Client.Guilds.Count}\n" +
                                                     $"Prefix: {ctx.Prefix} or <@{ctx.Client.CurrentUser.Id}>\n" +
                                                     $"Private Channels: {ctx.Client.PrivateChannels.Count}",
                        true)
                    .WithFooter($"Created on: {createdOn} or {timeSince.Days} days ago");

                await ctx.RespondAsync(embed: nice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("ip")]
        [Aliases("checkip", "whois", "vpn?")]
        [Description(
            "This will allow you to check an IP, meaning it will return whether or not the IP supplied is a VPN/proxy")]
        public async Task Ip(CommandContext ctx,
            [Description(
                " The string you enter will be an ipv4 or 6 address anything other than that will result in an error")]
            string ip)
        {
            try
            {
                var r = (HttpWebRequest) WebRequest.Create($"http://v2.api.iphub.info/ip/{ip}");
                r.Method = "GET";
                r.Headers["X-key"] = "MTE3OkExeUxxVE9QSjl5T2MxV0Qya1pQMEdWMmlWSVZXWTdF";

                var rs = (HttpWebResponse) r.GetResponse();
                string result = null;

                using (var sr = new StreamReader(rs.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }

                var obj = JsonConvert.DeserializeObject<InfoApi.IpApi>(result);

                var blocked = "";
                switch (obj.Block)
                {
                    case 0:
                        blocked = "\nResidential/Unclassified IP (i.e. safe IP)\n";
                        break;
                    case 1:
                        blocked = "\n Non-residential IP (hosting provider, proxy, etc.)\n";
                        break;
                    default:
                        blocked = "\nNon-residential & residential IP (warning, may flag innocent people)\n";
                        break;
                }

                var blockedColor = new int();
                switch (obj.Block)
                {
                    case 0:
                        blockedColor = 0x81C784;
                        break;
                    case 1:
                        blockedColor = 0xFFFF8D;
                        break;
                    default:
                        blockedColor = 0xFF8A80;
                        break;
                }

                var ipCheck = new DiscordEmbedBuilder()
                    .WithTitle($"{ctx.Client.CurrentApplication.Name} || Ip Checker")
                    .WithColor(new DiscordColor(blockedColor))
                    .AddField("IP", $"{obj.Ip}", true)
                    .AddField("ASN", $"{obj.Asn}", true)
                    .AddField("ISP", $"{obj.Isp}", true)
                    .AddField("Country Name", $"{obj.CountryName}", true)
                    .AddField("Type", $"{blocked}", true);

                await ctx.RespondAsync(embed: ipCheck);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("The remote server returned an error: (403) Forbidden."))
                {
                    var wrongApiKey = new DiscordEmbedBuilder()
                        .WithDescription(Formatter.BlockCode("Your api key is wrong"));
                    await ctx.RespondAsync(embed: wrongApiKey);
                }
                else if (e.Message.Contains("The remote server returned an error: (404) Not Found."))
                {
                    var couldnotFindIP = new DiscordEmbedBuilder()
                        .WithDescription(Formatter.BlockCode($"The ip: {ip} could not be found"));
                    await ctx.RespondAsync(embed: couldnotFindIP);
                }
                else
                {
                    Console.WriteLine(e);
                }

                throw;
            }
        }

        [Command("ping")]
        [Description("Check the bot's connection to the Discord Api.")]
        public async Task PingAsync(CommandContext ctx)
        {
            var pingo = new DiscordEmbedBuilder()
                .WithColor(new DiscordColor(0x4CAF50))
                .WithDescription($"`| {ctx.Client.CurrentApplication.Name}\'s ping is {ctx.Client.Ping}ms |`");
            await ctx.RespondAsync(embed: pingo);
        }

        [Command("geo")]
        [Description("want to pin point a ip? well with the combination of my IP check you can geolocate and ip.")]
        public async Task Geo(CommandContext ctx,
            [Description(
                "The string in this one will be an IP or host-name(a URL Example:`google.com` with no `https://` )")]
            [RemainingText]
            string IPorHostname)
        {
            try
            {
                var r = (HttpWebRequest) WebRequest.Create($"https://freegeoip.net/json/{IPorHostname}");
                r.Method = "GET";

                var rs = (HttpWebResponse) r.GetResponse();
                string result = null;

                using (var sr = new StreamReader(rs.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }

                var obj = JsonConvert.DeserializeObject<InfoApi.GeoApi>(result);

                var geoLocate = new DiscordEmbedBuilder()
                    .AddField("Human Location", $"Latitude: {obj.Latitude} | Longitude: {obj.Longitude}", true);
                await ctx.RespondAsync(embed: geoLocate);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("The remote server returned an error: (404) Not Found."))
                {
                    var error = new DiscordEmbedBuilder()
                        .WithTitle("Error: See below")
                        .WithColor(new DiscordColor(0xD32F2F))
                        .AddField("Message",
                            "The host-name or IP you have entered came back as an error, please make sure you have the right host-name or IP",
                            true)
                        .AddField("Information",
                            "Please do `>help info geo`\nReference: `string` = Host-name(`google.com`) or an IP address");
                    await ctx.RespondAsync(embed: error);
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }

                throw;
            }
        }


        [Command("guildInfo")]
        public async Task GuildInfomation(CommandContext c, DiscordGuild guild)
        {
            var serverInformation = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.HotPink)
                .WithDescription(Formatter.BlockCode(
                    $"{guild.Name} Owned by: {guild.Owner.Username}#{guild.Owner.Discriminator}\n" +
                    $"---------------------------------------------\n" +
                    $"• Member_Count ::                    {guild.MemberCount}\n" +
                    $"• DefaultMessage_Notifications ::    {guild.DefaultMessageNotifications}\n" +
                    $"• Guild_Owner ::                     {guild.Owner.Username}\n" +
                    $"• Afk_Channel ::                     {guild.AfkChannel}\n" +
                    $"• Afk_Timeout ::                     {guild.AfkTimeout}\n" +
                    $"• Verification_Level ::              {guild.VerificationLevel}\n" +
                    $"• Number_of_Channels ::              {guild.Channels.Count}\n" +
                    $"• Number_of_Roles ::                 {guild.Roles.Count}\n" +
                    $"• Mfa_level ::                       {guild.MfaLevel}", "asciidoc"));
            await c.RespondAsync(embed: serverInformation);
        }

        [Command("MyUserInfo")]
        public async Task UserInformation(CommandContext c)
        {
            var m = c.Member;
            var sb = new StringBuilder();
            foreach (var role in m.Roles)
                sb.Append($"{role.Name}, ");

            var getUserInformation = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Rose)
                .WithDescription(Formatter.BlockCode($"{m.Username}#{m.Discriminator} Information\n" +
                                                     $"-----------------------------------\n" +
                                                     $"• Color ::             {m.Color}\n" +
                                                     $"• Avatar_Hash ::       {m.AvatarHash}\n" +
                                                     $"• Discriminator ::     {m.Discriminator}\n" +
                                                     $"• Display_Name ::      {m.DisplayName}\n" +
                                                     $"• Is_Bot? ::           {m.IsBot}\n" +
                                                     $"• Is_Deafened? ::      {m.IsDeafened}\n" +
                                                     $"• Is_Muted? ::         {m.IsMuted}\n" +
                                                     $"• Is_Owner? ::         {m.IsOwner}\n" +
                                                     $"• Joined_At ::         {m.JoinedAt}\n" +
                                                     $"• Creation_Timestamp:: {m.CreationTimestamp}\n" +
                                                     $"• User_ID ::           {m.Id}\n" +
                                                     $"• User_Activity_Name :: {m.Presence.Activity.Name}\n" +
                                                     $"• User_Activity_Type :: {m.Presence.Activity.ActivityType}\n" +
                                                     $"• User_Activity_Stream_Url :: {m.Presence.Activity.StreamUrl}\n" +
                                                     $"• User_Status ::       {m.Presence.Status}\n" +
                                                     $"• Mfa_Enabled? ::      {m.MfaEnabled}\n" +
                                                     $"• Verified? ::         {m.Verified}\n" +
                                                     $"• Mention_String ::    {m.Mention}\n" +
                                                     $"• VoiceState ::        {m.VoiceState}\n" +
                                                     $"• Avatar_Url ::        {m.AvatarUrl}\n" +
                                                     $"• Roles? ::            {sb}", "asciidoc"));
            await c.RespondAsync(embed: getUserInformation);
        }

        [Command("getUserInfo")]
        public async Task GetUserInformation(CommandContext c,
            [Description(" m will be a member of the server you want to check there information")]
            DiscordMember m)
        {
            var sb = new StringBuilder();
            foreach (var role in m.Roles)
                sb.Append($"{role.Name}, ");

            var getUserInformation = new DiscordEmbedBuilder()
                .WithColor(DiscordColor.Rose)
                .WithDescription(Formatter.BlockCode($"{m.Username}#{m.Discriminator} Information\n" +
                                                     $"-----------------------------------\n" +
                                                     $"• Color ::             {m.Color}\n" +
                                                     $"• Avatar_Hash ::       {m.AvatarHash}\n" +
                                                     $"• Discriminator ::     {m.Discriminator}\n" +
                                                     $"• Display_Name ::      {m.DisplayName}\n" +
                                                     $"• Is_Bot? ::           {m.IsBot}\n" +
                                                     $"• Is_Deafened? ::      {m.IsDeafened}\n" +
                                                     $"• Is_Muted? ::         {m.IsMuted}\n" +
                                                     $"• Is_Owner? ::         {m.IsOwner}\n" +
                                                     $"• Joined_At ::         {m.JoinedAt}\n" +
                                                     $"• Creation_Timestamp:: {m.CreationTimestamp}\n" +
                                                     $"• User_ID ::           {m.Id}\n" +
                                                     $"• User_Activity_Name :: {m.Presence.Activity.Name}\n" +
                                                     $"• User_Activity_Type :: {m.Presence.Activity.ActivityType}\n" +
                                                     $"• User_Activity_Stream_Url :: {m.Presence.Activity.StreamUrl}\n" +
                                                     $"• User_Status ::       {m.Presence.Status}\n" +
                                                     $"• Mfa_Enabled? ::      {m.MfaEnabled}\n" +
                                                     $"• Verified? ::         {m.Verified}\n" +
                                                     $"• Mention_String ::    {m.Mention}\n" +
                                                     $"• VoiceState ::        {m.VoiceState}\n" +
                                                     $"• Avatar_Url ::        {m.AvatarUrl}\n" +
                                                     $"• Roles? ::            {sb}", "asciidoc"));
            await c.RespondAsync(embed: getUserInformation);
        }

        [Command("feedback")]
        [RequireBotPermissions(Permissions.ManageMessages)]
        [Description("`Permissions needed:` **ManageMessages**")]
        public async Task Feedback(CommandContext c)
        {
            var bot = c.Client.GetInteractivity();
            const ulong guildid = 402458071349067777;
            const ulong channelid = 405869076074725386;
            var feedback = await c.Client.GetGuildAsync(guildid);
            try
            {
                var top = await c.RespondAsync($"Welcome to {c.Client.CurrentApplication.Name}'s feedback command" +
                                               "Do you want to continue? (Y/N) **Uppercase**");
                var msg = await bot.WaitForMessageAsync(e => e.Author.Id == c.Message.Author.Id,
                    TimeSpan.FromSeconds(45));
                if (!msg.Message.Content.NonCaseSensitive("Y"))
                {
                    await c.Message.DeleteAsync();
                    await c.RespondAsync(
                        "I'm sorry if this wasn't what you were looking for. Just use this command again `>info feedback` if you want to send me something.");
                    await top.DeleteAsync("I'm cleaning after my self");
                    await msg.Message.DeleteAsync("even your message");
                    return;
                }

                await top.DeleteAsync("I'm cleaning after my self");
                await msg.Message.DeleteAsync();
                var middle = await c.RespondAsync("What would you like to do.\n" +
                                                  "`1) Summit a bug`\n" +
                                                  "`2) Send a suggestion`\n" +
                                                  "`3) Give the owner feedback`\n" +
                                                  "`4) Just want to send me something`");
                var msg2 = await bot.WaitForMessageAsync(e => e.Author.Id == c.Message.Author.Id,
                    TimeSpan.FromSeconds(45));

                if (!msg2.Message.Content.Equals("1") && !msg2.Message.Content.Equals("2") &&
                    !msg2.Message.Content.Equals("3") && !msg2.Message.Content.Equals("4"))
                {
                    await c.RespondAsync(
                        "I'm sorry if this wasn't what you were looking for. Just use this command again if you want to send me send something to my owner.");
                    await middle.DeleteAsync("I'm cleaning after my self");
                    await msg2.Message.DeleteAsync("even your message");
                }
                else if (msg2.Message.Content.Equals("1"))
                {
                    await middle.DeleteAsync("I'm cleaning after my self");
                    var subMid = await c.RespondAsync(
                        "Before you continue please make sure to use this format below, You have enough time read and create the format(Time-limit is 8 minutes)\n" +
                        "**Format:** \n" +
                        "Create a code-block or use the format.\n" +
                        "```Command: CommandNameHere\n" +
                        "Group: What group does the command belong to```");

                    var bug = await bot.WaitForMessageAsync(e => e.Author.Id == c.Message.Author.Id,
                        TimeSpan.FromMinutes(8));

                    var bugEmbed = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xFF8A80))
                        .WithAuthor(
                            $"Potential bug found by: {c.User.Username}#{c.User.Discriminator} in guild: {c.Guild.Name}")
                        .WithDescription($"{bug.Message.Content}");

                    await feedback.GetChannel(channelid).SendMessageAsync(embed: bugEmbed);
                    await subMid.DeleteAsync();
                    await msg2.Message.DeleteAsync();
                    await c.RespondAsync("I have submitted it, Thank you :heart:");
                }
                else if (msg2.Message.Content.Equals("2"))
                {
                    await middle.DeleteAsync("I'm cleaning after my self");
                    var subMid =
                        await c.RespondAsync("Please respond with what you want to suggest **(8 minute timer)**");

                    var bug = await bot.WaitForMessageAsync(e => e.Author.Id == c.Message.Author.Id,
                        TimeSpan.FromMinutes(8));

                    var bugEmbed = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xB9F6CA))
                        .WithAuthor(
                            $"A new suggestion by: {c.User.Username}#{c.User.Discriminator} in guild: {c.Guild.Name}")
                        .WithDescription($"{bug.Message.Content}");

                    await feedback.GetChannel(channelid).SendMessageAsync(embed: bugEmbed);
                    await subMid.DeleteAsync();
                    await msg2.Message.DeleteAsync();
                    await c.RespondAsync("I have sent your suggestion, Thank you :heart:");
                }
                else if (msg2.Message.Content.Equals("3"))
                {
                    await middle.DeleteAsync("I'm cleaning after my self");
                    var sub_mid =
                        await c.RespondAsync("Please respond with what you want to suggest **(8 minute timer)**");

                    var bug = await bot.WaitForMessageAsync(e => e.Author.Id == c.Message.Author.Id,
                        TimeSpan.FromMinutes(8));

                    var BugEmbed = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xA7FFEB))
                        .WithAuthor(
                            $"{c.User.Username}#{c.User.Discriminator} in guild: {c.Guild.Name} has given feedback")
                        .WithDescription($"{bug.Message.Content}");

                    await feedback.GetChannel(channelid).SendMessageAsync(embed: BugEmbed);
                    await sub_mid.DeleteAsync();
                    await msg2.Message.DeleteAsync();
                    await c.RespondAsync("Sent, Thank you :heart:");
                }
                else if (msg2.Message.Content.Equals("4"))
                {
                    await middle.DeleteAsync("I'm cleaning after my self");
                    var sub_mid =
                        await c.RespondAsync("What would you like me to send to my server? **(8 minute timer)**");

                    var bug = await bot.WaitForMessageAsync(e => e.Author.Id == c.Message.Author.Id,
                        TimeSpan.FromMinutes(8));

                    var BugEmbed = new DiscordEmbedBuilder()
                        .WithColor(new DiscordColor(0xFF80AB))
                        .WithAuthor(
                            $"{c.User.Username}#{c.User.Discriminator} in guild: {c.Guild.Name} just wanted to send something")
                        .WithDescription($"{bug.Message.Content}");

                    await feedback.GetChannel(channelid).SendMessageAsync(embed: BugEmbed);
                    await sub_mid.DeleteAsync();
                    await msg2.Message.DeleteAsync();
                    await c.RespondAsync("Sent, Thank you :heart:");
                }
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Unauthorized: 403"))
                {
                    await c.RespondAsync("I lack permission. :(");
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [Command("weather")]
        public async Task Weather(CommandContext c, string Location)
        {
            var Results =
                await
                    $"http://api.openweathermap.org/data/2.5/weather?q={Location}&APPID=471d2fb334519d7ddfbb8ceec590a3e2"
                        .GetStringAsync();
            var Data = WeatherApi.FromJson(Results);

            var Main = Data.Main;

            var Weather = new DiscordEmbedBuilder()
                .WithColor(new DiscordColor(0x1DE9B6))
                .WithAuthor($"{c.Client.CurrentApplication.Name}'s weather report for: {Data.Name} {Data.Sys.Country}")
                .AddField("Base", $"{Data.Base}")
                .AddField("Wind", $"Wind Speed: {Data.Wind.Speed}\n" +
                                  $"Wind Direction: {Data.Wind.Deg}")
                .AddField("Clouds", $"{Data.Clouds.All}")
                .AddField("Main", $"Temperature: {Main.Temp} Min: {Main.TempMin} Max: {Main.TempMax}\n" +
                                  $"Humidity: {Main.Humidity}\n" +
                                  $"Atmospheric Pressure: {Main.Pressure}")
                .AddField("Coordinates", $"{Data.Coord.Lat} {Data.Coord.Lon}");

            await c.RespondAsync(embed: Weather);
        }

        [Command("UpdateNotes")]
        public async Task UpdateNOtes(CommandContext c)
        {
            try
            {
                #region database connection

                var client = new MongoClient("mongodb://127.0.0.1:27017/");
                var database = client.GetDatabase("_Update");
                var collection = database.GetCollection<BsonDocument>($"Update:{c.User.Id}");

                var filter = Builders<BsonDocument>.Filter.Eq("_id", 0);
                var document = collection.Find(filter).First();
                var data = Testw.FromJson(document.ToJson());

                #endregion

                var notes = new DiscordEmbedBuilder()
                    .WithAuthor($"Updated by: {data.SubmittedBy}")
                    .WithDescription($"{data.UpdateText}")
                    .WithFooter($"Version: {data.UpdateVersion}")
                    .WithColor(DiscordColor.SpringGreen);
                await c.RespondAsync(embed: notes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [Command("getroles")]
        public async Task GetRolesList(CommandContext c)
        {
            try
            {
                var s = new StringBuilder();
                for (var index = 0; index < c.Guild.Roles.ToList().Count; index++)
                {
                    var r = c.Guild.Roles.ToList()[index];
                    s.AppendLine($"{r.Mention} : {r.Id}");
                }

                var RoleEmbed = new DiscordEmbedBuilder()
                    .WithDescription($"{s}");
                await c.RespondAsync(embed: RoleEmbed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string getUptime()
        {
            return (DateTime.Now - Process.GetCurrentProcess().StartTime).ToString(@"dd\.hh\:mm\:ss");
        }

        private static string getHeapSize()
        {
            return Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2).ToString();
        }
    }
}