using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.Extended.Module;
using DSharpPlus.Interactivity;
using DSharpPlus.VoiceNext;
using DSharpPlus.VoiceNext.Codec;
using Yone.Components;
using YoneLib;
using YoneLib.Api;
using YoneSql;

namespace Yone
{
    internal static class Program
    {
        private static DiscordClient _yone;
        private static CommandsNextExtension _commands;
        private static VoiceNextExtension _voice;

        private static void Main()
        {
            FirstRun(); // Checks
        }

        private static async Task MainMenu()
        {
            
            Console.Clear();
            Console.Title = $"Yone v{Assembly.GetEntryAssembly().GetName().Version}";
            Console.ResetColor();
            Console.WriteLine("Welcome to Yone Discord Bot!");
            Console.WriteLine("----------------------------");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Author: HeraKumi / Hoshiko");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Twitter:");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("    @LoliLeague");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nPSN:");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("        LeagueOfLolis\n");
            Console.ResetColor();
            
            Console.WriteLine("---------------------------------------------------------------------------");
            Console.WriteLine("Project: Yone is licensed under the GNU Lesser General Public License v3.0");
            Console.WriteLine("Project Url: https://github.com/HeraKumi/Yone");
            Console.ResetColor();

            Console.WriteLine("---------------------------------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(
                "Permissions of this copycat license are conditioned on making available complete source code of licensed works and modifications under the same license or the GNU GPLv3. Copyright and license notices must be preserved. Contributors provide an express grant of patent rights. However, a larger work using the licensed work through interfaces provided by the licensed work may be distributed under different terms and without source code for the larger work.\n");
            Console.ResetColor();
            Console.WriteLine("---------------------------------------------------------------------------");

            Console.WriteLine($"1) Start [Yone v{Assembly.GetEntryAssembly().GetName().Version}]");
            Console.WriteLine("2) Update Default Settings");
            Console.WriteLine("3) Exit Yone");

            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.Clear();
                    MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    break;
                case "2":
                    UpdateMainMenu();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
            }
        }

        private static async Task MainAsync()
        {
            var data = new Global().DefaultDatabase();
            _yone = new DiscordClient(new DiscordConfiguration
            {
                Token = data.botToken,
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
                AutomaticGuildSync = true,
                AutoReconnect = true
            });
            _commands = _yone.UseCommandsNext(new CommandsNextConfiguration
            {
                PrefixResolver = GetPrefixPositionAsync,
                EnableMentionPrefix = true,
                EnableDms = false,
                CaseSensitive = false,
                IgnoreExtraArguments = true
            });
            _yone.UseInteractivity(new InteractivityConfiguration
            {
                PaginationBehavior = 0,
                PaginationTimeout = TimeSpan.FromMinutes(5.0),
                Timeout = TimeSpan.FromMinutes(2.0)
            });
            var msc = new VoiceNextConfiguration
            {
                VoiceApplication = VoiceApplication.Music
            };


            var dspExtended = _yone.UseDspExtended();

            _commands.SetHelpFormatter<Help.DefaultHelpFormatter>();
            _commands.RegisterCommands(Assembly.GetExecutingAssembly());

            dspExtended.RegisterAssembly(Assembly.GetExecutingAssembly());

            _yone.UseVoiceNext(msc);
            await _yone.ConnectAsync();
            await Task.Delay(-1);
        }

        private static void UpdateMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("UPDATER");
                Console.ResetColor();

                Console.WriteLine("1) Bot token\n" + "2) Discord Bot List token\n" + "3) Ip Hub api key\n" +
                                  "4) Twitch Client ID\n" + "5) Start Bot\n" + "6) Back...");

                Console.Write("Update: ");
                var numberUpdate = Console.ReadLine();
                switch (numberUpdate)
                {
                    case "1":
                        Console.Write("Please enter your updated bot token: ");
                        var token = Console.ReadLine();
                        Default.botToken(token);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Done!");
                        Thread.Sleep(500); // Half a second delay
                        Console.ResetColor();
                        Console.Clear();
                        continue;

                    case "2":
                        Console.Write("Please enter your updated DBO list token: ");
                        var DBOtoken = Console.ReadLine();
                        Default.discordBotOrg(DBOtoken);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Done!");
                        Thread.Sleep(500); // Half a second delay
                        Console.ResetColor();
                        Console.Clear();
                        continue;
                    case "3":
                        Console.Write("Please enter your updated Ip hub key: ");
                        var IpToken = Console.ReadLine();
                        Default.ipHub(IpToken);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Done!");
                        Thread.Sleep(500); // Half a second delay
                        Console.ResetColor();
                        Console.Clear();
                        continue;

                    case "4":
                        Console.Write("Please entry your updated Twitch Client ID: ");
                        var tID = Console.ReadLine();
                        Default.twitchClientId(tID);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Done!");
                        Thread.Sleep(500); // Half a second delay
                        Console.ResetColor();
                        Console.Clear();
                        continue;
                    case "5":
                        Console.Clear();
                        MainAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        break;
                    case "6":
                        Console.Clear();
                        MainMenu();
                        break;
                }
                break;
            }
        }

        private static Task<int> GetPrefixPositionAsync(DiscordMessage msg)
        {
            try
            {
                var data = new Global().GetDBRecords(msg.Channel.Guild.Id);
                return Task.FromResult(msg.GetStringPrefixLength(data.GuildPrefix));
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e);
                throw;
                Console.ResetColor();
            }
        }

        private static void FirstRun()
        {
            try
            {
                #region Windows

                var pname = Process.GetProcessesByName("mongod");
                DefaultAPI.DefaultApi data;
                if (pname.Length > 0)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("MongoDb has been detected... Continuing!");
                    Console.ResetColor();
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("MongoDB cannot be detected! Is it running?");
                    Console.ResetColor();
                    Console.WriteLine(
                        "Please make sure that you have installed MongoDb. https://www.mongodb.com/download-center?jmp=nav#community\nOnce you have installed it, Please restart the application!");
                    return;
                }

                data = new Global().DefaultDatabase();

                if (data.firstRun)
                    try
                    {
                        Console.Clear();
                        
                        Console.Write("Please insert your bot token: ");
                        var botToken = Console.ReadLine();
                        Default.botToken(botToken);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Applied bot token.");
                        Console.ResetColor();

                        Console.Write("Please insert your DiscordBotList token: ");
                        var DBOToken = Console.ReadLine();
                        Default.discordBotOrg(DBOToken);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Apllied Discord bot list token.");
                        Console.ResetColor();

                        Console.Write("Please insert your IPHub api key: ");
                        var ipKey = Console.ReadLine();
                        Default.ipHub(ipKey);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Apllied IPHub token.");
                        Console.ResetColor();

                        Default.FirstRun(false);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(
                            "Okay I have everything setup! Congratulation you can now start using this bot!");
                        Console.ResetColor();

                        MainMenu();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                else if (data.firstRun == false)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("Starting menu!....");
                    Thread.Sleep(100);
                    Console.ResetColor();
                    MainMenu();
                };

                #endregion
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Sequence contains no elements"))
                {
                    Default.CreateDatabase();
                    var filename = Assembly.GetExecutingAssembly().Location;
                    Process.Start(filename);
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}