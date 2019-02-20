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
            Console.WriteLine(context.Message);
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
