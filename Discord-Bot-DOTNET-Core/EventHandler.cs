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
            this.client.Connected += HandleConnectAsync;
            await this.service.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleConnectAsync()
        {
            IEnumerable<SocketGuild> guilds = this.client.Guilds;
            foreach (SocketGuild guild in guilds)
            {
                Config.AddServer(guild.Id);
            }
        }

        private async Task HandleRolesAsync(SocketGuildUser oldUser, SocketGuildUser newUser)
        {
            IEnumerable<SocketRole> roles = newUser.Roles;
            List<List<ulong>> dependencies = new List<List<ulong>>();
            foreach (SocketRole role in roles)
            {
                dependencies.Add(Config.getDependencies(newUser.Guild.Id, role.Id));
            }
            foreach(List<ulong> dependency in dependencies)
            {
                foreach(ulong roleId in dependency)
                {
                    if (!newUser.Roles.Contains(newUser.Guild.GetRole(roleId)))
                    {
                        await newUser.AddRoleAsync(newUser.Guild.GetRole(roleId));
                    }
                }
            }
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = s as SocketUserMessage;
            if (msg == null) return;
            SocketCommandContext context = new SocketCommandContext(this.client, msg);
            int argPos = 0;
            if (msg.HasStringPrefix(Config.GetServerPrefix(context.Guild.Id), ref argPos)
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
