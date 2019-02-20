using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Discord_Bot.Modules;
using Discord;

namespace Discord_Bot
{
    public class CommandHandler
    {
        DiscordSocketClient client;
        CommandService service;
        IServiceProvider services;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            this.client = client;
            this.service = new CommandService();
            this.services = new Setup().BuildProvider();
            this.client.MessageReceived += HandleCommandAsync;
            await this.service.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = s as SocketUserMessage;
            if (msg == null) return;
            SocketCommandContext context = new SocketCommandContext(this.client, msg);
            int argPos = 0;
            if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
             && !context.IsPrivate)
            {
                IResult result = await this.service.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
            else
            {
                await Commands.NoCommand(context);
            }
        }
        public async Task NoCommand(SocketCommandContext context, string text)
        {
            if (context.User.IsBot) return;
            Console.WriteLine(text);
            if (Config.channels.user.Contains(context.Channel.Id)
             || Config.channels.admin.Contains(context.Channel.Id))
            {
                EmbedBuilder embed = new EmbedBuilder();
                embed.WithColor(new Color(47, 191, 127));
                embed.WithTitle("I can repeat you!");
                embed.WithDescription(text);
//                await context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
    }
}
