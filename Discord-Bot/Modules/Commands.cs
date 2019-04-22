using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot.Modules
{
    public class MyService
    {

    }

    public class Setup
    {
        public IServiceProvider BuildProvider() => new ServiceCollection().AddSingleton<MyService>().BuildServiceProvider();
    }

    public class Commands : ModuleBase<SocketCommandContext>
    {
        public MyService MyService { get; set; }

        public Commands(MyService myService) => MyService = myService;

        public static async Task NoCommand(SocketCommandContext context)
        {
            if (context.User.IsBot) return;
            string name = context.Guild.GetUser(context.User.Id).Nickname;
            if (name.Length == 0) name = context.User.Username;
            Console.WriteLine($"{name}@{context.Channel.Name}@{context.Guild.Name}: {context.Message}");
            if (Config.channels.user.Contains(context.Channel.Id)
             || Config.channels.admin.Contains(context.Channel.Id))
            {
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithColor(new Color(47, 191, 127));
                embed.WithTitle("I can repeat you!");
                embed.WithDescription(context.Message.ToString());
                //                await context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }

        [Command("test")]
        public async Task Test([Remainder]string text = "no parameters")
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Test result");
            embed.WithColor(new Color(47, 191, 127));
            embed.WithDescription($"This is the result of the requested test.\nIf you entered some parameters, you can have them back:\n{text}");
            embed.AddField($"Field 1", "This is the 1st field", true);
            embed.AddField($"Field 2", "This is the 2nd field", true);
            embed.AddField($"Field 3", "This is the 3rd field", true);
            embed.AddField($"Field 4", "This is the 4th field", true);
            embed.AddField($"Field 5", "This is the 5th field", true);
            embed.AddField($"Field 6", "This is the 6th field", true);
            embed.AddField($"Field 7", "This is the 7th field", true);
            embed.AddField($"Field 8", "This is the 8th field", true);
            embed.AddField($"Field 9", "This is the 9th field", true);
            embed.WithAuthor(Context.Client.CurrentUser);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("help")]
        public async Task HelpMessage()
        {
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Available Commands");
            embed.WithColor(new Color(47, 191, 127));
            embed.WithDescription("Here\nwill\nbe\nthe\ncommands");
            embed.AddField($"{Config.bot.cmdPrefix}help", "Show this help", true);
            embed.AddField($"{Config.bot.cmdPrefix}activity - restricted access", "Change the activity shown.", true);
            embed.AddField($"{Config.bot.cmdPrefix}say", "Let me repeat your message");
            embed.WithAuthor(Context.Client.CurrentUser);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("activity")]
        public async Task ChangeActivity([Remainder]string text)
        {
            if (Config.users.admin.Contains(Context.User.Id)
             && Config.channels.admin.Contains(Context.Channel.Id))
            {
                await Context.Client.SetActivityAsync(new Game(text));
                await Context.Channel.SendMessageAsync($"Changed Activity to \"{text}\"");
            }
        }

        [Command("say")]
        public async Task Say([Remainder]string text)
        {
            if (!Context.User.IsBot)
            {
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithTitle("You wanted me to say:");
                embed.WithColor(new Color(47, 191, 127));
                embed.WithDescription(text);
                embed.WithAuthor(Context.User);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
                await Context.Message.DeleteAsync();
            }
        }

        [Command("caa")]
        public async Task AddAdminChannel()
        {
            if (Config.users.admin.Contains(Context.User.Id))
            {
                if (Config.AddChannelAdmin(Context.Channel.Id))
                {
                    Config.Save();
                    await Context.Channel.SendMessageAsync("added");
                }
            }
        }
        [Command("caa")]
        public async Task AddAdminChannel([Remainder]string text)
        {
            if (Config.users.admin.Contains(Context.User.Id)
             && Config.channels.admin.Contains(Context.Channel.Id))
            {
                if (Config.AddChannelAdmin(Context.Message.MentionedChannels.First().Id))
                {
                    Config.Save();
                    await Context.Channel.SendMessageAsync("added");
                }
            }
        }

        [Command("cau")]
        public async Task AddUserChannel()
        {
            if (Config.users.user.Contains(Context.User.Id)
             || Config.users.admin.Contains(Context.User.Id))
            {
                if (Config.AddChannelUser(Context.Channel.Id))
                {
                    Config.Save();
                    await Context.Channel.SendMessageAsync("added");
                }
            }
        }
        [Command("cau")]
        public async Task AddUserChannel([Remainder]string text)
        {
            if ((Config.users.user.Contains(Context.User.Id)
              || Config.users.admin.Contains(Context.User.Id))
              && Config.channels.admin.Contains(Context.Channel.Id))
            {
                if (Config.AddChannelUser(Context.Message.MentionedChannels.First().Id))
                {
                    Config.Save();
                    await Context.Channel.SendMessageAsync("added");
                }
            }
        }

        [Command("uaa")]
        public async Task AddAdminUser([Remainder]string text)
        {
            if (Config.users.admin.Contains(Context.User.Id)
             && Config.channels.admin.Contains(Context.Channel.Id))
            {
                if (Config.AddUserAdmin(Context.Message.MentionedUsers.First().Id))
                {
                    Config.Save();
                    await Context.Channel.SendMessageAsync("added");
                }
            }
        }

        [Command("uau")]
        public async Task AddUserUser([Remainder]string text)
        {
            if (Config.users.admin.Contains(Context.User.Id))
            {
                if (Config.AddUserUser(Context.Message.MentionedUsers.First().Id))
                {
                    Config.Save();
                    await Context.Channel.SendMessageAsync("added");
                }
            }
        }
    }
}
