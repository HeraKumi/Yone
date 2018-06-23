using System.Collections.Generic;
using System.Linq;
using System.Text;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.CommandsNext.Entities;
using DSharpPlus.Entities;

namespace Yone.Components
{
    public abstract class Help
    {
        public class DefaultHelpFormatter : BaseHelpFormatter
        {
            public DefaultHelpFormatter(CommandsNextExtension cnext)
                : base(cnext)
            {
                EmbedBuilder = new DiscordEmbedBuilder()
                    .WithTitle($"Help - {cnext.Client.CurrentApplication.Name}'s Menu! | https://discord.gg/kgbVzb5")
                    .WithColor(0xFF80AB);
            }

            private DiscordEmbedBuilder EmbedBuilder { get; }
            private Command Command { get; set; }

            public override BaseHelpFormatter WithCommand(Command command)
            {
                Command = command;

                EmbedBuilder.WithDescription(
                    $"{Formatter.InlineCode(command.Name)}: {command.Description ?? "No description provided."}");

                if (command is CommandGroup cgroup && cgroup.IsExecutableWithoutSubcommands)
                    EmbedBuilder.WithDescription(
                        $"{EmbedBuilder.Description}\n\nThis group can be executed as a standalone command.");

                if (command.Aliases?.Any() == true)
                    EmbedBuilder.AddField("Aliases", string.Join(", ", command.Aliases.Select(Formatter.InlineCode)),
                        false);

                if (command.Overloads?.Any() != true) return this;
                var sb = new StringBuilder();

                foreach (var ovl in command.Overloads.OrderByDescending(x => x.Priority))
                {
                    sb.Append('`').Append(command.QualifiedName);

                    foreach (var arg in ovl.Arguments)
                        sb.Append(arg.IsOptional || arg.IsCatchAll ? " [" : " <").Append(arg.Name)
                            .Append(arg.IsCatchAll ? "..." : "")
                            .Append(arg.IsOptional || arg.IsCatchAll ? ']' : '>');

                    sb.Append("`\n");

                    foreach (var arg in ovl.Arguments)
                        sb.Append('`').Append(arg.Name).Append(" (")
                            .Append(CommandsNext.GetUserFriendlyTypeName(arg.Type)).Append(")`: ")
                            .Append(arg.Description ?? "No description provided.").Append('\n');

                    sb.Append('\n');
                }

                EmbedBuilder.AddField("Arguments", sb.ToString().Trim(), false);
                return this;
            }

            public override BaseHelpFormatter WithSubcommands(IEnumerable<Command> subcommands)
            {
                EmbedBuilder.AddField(Command != null ? "Commands" : "Groups",
                    string.Join(" | ", subcommands.Select(x => Formatter.InlineCode($"{x.Name}"))), false);
                return this;
            }

            public override CommandHelpMessage Build()
            {
                var desc =
                    $"**Hello, Thank you for using my help command**, below you will find all of the groups within **{CommandsNext.Client.CurrentApplication.Name}**, these groups hold within them the commands" +
                    "\nTo view them use `>help group`(**Remember using \"group\" will NOT work you have to use an actual group(listed below)**) This will then send you another embed message. Please then read that embed message.\n" +
                    "below you will find more information about how to use commands and stuff..\n" +
                    "`------------------------------------------`\n" +
                    "```asciidoc\n" +
                    ".1) To use a command you must use this format >group command the group will be one of the groups listed below & for the command you will find them inside the group you want, to do so please use >help group to list all commands within that group\n" +
                    ".2) Example command usage: >info stats\n" +
                    ".3) Example help usage: >help info\n" +
                    ".4) Example help command usage: >help info ip\n```" +
                    "```asciidoc\n" +
                    "== Here is more help ==\n" +
                    "•Usage:: <group> <command>\n" +
                    ".Example 1: info stats (Issues a command)\n" +
                    ".Example 2: info ip 127.0.0.1 (Issues a command that require additional arguments to execute)\n\n" +
                    "•help:: <command> search for help on a command\n" +
                    ".Example 1: help info (Shows available commands/Description of the group)\n" +
                    ".Example 2: help info ip (Shows infornmation how the command you want help will)\n\n" +
                    "•where:: <group> is one of below\n" +
                    "•where:: <command> is one of the commands found in one of the groups```\n" +
                    $"\"Where do I get the commands?\" well you have to do `help group`";

                if (Command == null)
                    EmbedBuilder.WithDescription($"{desc}");
                EmbedBuilder.WithFooter("If you still need help please join my server: https://discord.gg/kgbVzb5");
                return new CommandHelpMessage(embed: EmbedBuilder.Build());
            }
        }
    }
}