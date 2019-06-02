using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
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

        public static Task NoCommand(SocketCommandContext context)
        {
            if (context.User.IsBot) return Task.CompletedTask;
            Console.WriteLine($"{(context.Guild.GetUser(context.User.Id).Nickname ?? context.User.Username)}@{context.Channel.Name}@{context.Guild.Name}: {context.Message}");
/*            if (Config.channels.user.Contains(context.Channel.Id)
             || Config.channels.admin.Contains(context.Channel.Id))
            {
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithColor(new Color(47, 191, 127));
                embed.WithTitle("I can repeat you!");
                embed.WithDescription(context.Message.ToString());
                //                await context.Channel.SendMessageAsync("", false, embed.Build());
            }*/
            return Task.CompletedTask;
        }

 /*       [Command("addGamingRole")]
        public async Task AddGamingRole(string role)
        {
            if (Config.users.admin.Contains(Context.User.Id)
             && Config.channels.admin.Contains(Context.Channel.Id))
            {
                if (Context.Message.MentionedRoles.Count() == 1)
                {
                    if (Config.AddGamingRole(Context.Message.MentionedRoles.First().Id))
                    {
                        Config.Save();
                        await Context.Channel.SendMessageAsync("Added");
                    }
                    else
                    {
                        await Context.Channel.SendMessageAsync("That role is already registered.");
                    }
                }
                else
                {
                    await Context.Channel.SendMessageAsync("You have to Mention exactly one Role");
                }
            }
        }*/

        [Command("test")]
        public async Task Test(string text)
        {
            await Context.Channel.SendMessageAsync(text);
        }

        [Command("help")]
        public async Task HelpMessage()
        {
            ulong guild = Context.Guild.Id;
            EmbedBuilder embed = new EmbedBuilder();
            embed.WithTitle("Available Commands");
            embed.WithColor(new Color(47, 191, 127));
            embed.WithDescription("currently no functionality guaranteed.");
            embed.AddField($"{Config.GetServerPrefix(guild)}help", "Show this help", true);
            embed.AddField($"{Config.GetServerPrefix(guild)}activity - restricted access", "Change the activity shown.", true);
            embed.AddField($"{Config.GetServerPrefix(guild)}say", "Let me repeat your message");
            embed.WithAuthor(Context.Client.CurrentUser);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }

        [Command("activity")]
        public async Task ChangeActivity(string option, [Remainder]string text = "Nothing")
        {
            ulong serverId = Context.Guild.Id;
            ulong userId = Context.User.Id;
            ulong channelId = Context.Channel.Id;
            if (Config.GetUserPermissionLevel(serverId, userId).Equals(Config.Permission.ADMIN)
             && Config.GetChannelPermissionLevel(serverId, channelId).Equals(Config.Permission.ADMIN))
            {
                ActivityType activity;
                switch (option)
                {
                    case "--listening" :
                        activity = ActivityType.Listening;
                        break;
                    case "--playing" :
                        activity = ActivityType.Playing;
                        break;
                    case "--streaming" :
                        activity = ActivityType.Streaming;
                        break;
                    case "--watching" :
                        activity = ActivityType.Watching;
                        break;
                    case "--reset" :
                        activity = ActivityType.Listening;
                        text = "/$help";
                        break;
                    default :
                        await Context.Channel.SendMessageAsync("Invalid option, use --listening, --playing, --streaming, --watching or --reset.");
                        return;
                }
                await Context.Client.SetActivityAsync(new Game(text, activity));
                await Context.Channel.SendMessageAsync($"Changed Activity to \"{text}\" of type {activity.ToString()}");
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

/*        [Command("caa")]
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
        }*/
    }
}
