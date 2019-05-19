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
    public class EventHandler
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
            this.client.GuildMemberUpdated += HandleRolesAsync;
            await this.service.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleRolesAsync(SocketGuildUser oldUser, SocketGuildUser newUser)
        {
            IEnumerable<SocketRole> gameRoles = newUser.Roles.Where(x => Config.gameRoles.many.Contains(x.Id));
            if (gameRoles.Count() != 0 && !newUser.Roles.Contains(newUser.Guild.GetRole(Config.gameRoles.one)))
            {
                await newUser.AddRoleAsync(newUser.Guild.GetRole(Config.gameRoles.one));
            }
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
    }
}