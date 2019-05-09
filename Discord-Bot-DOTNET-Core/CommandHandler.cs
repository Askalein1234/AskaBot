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
        private DiscordSocketClient Client { get; set; }
        private CommandService Service { get; set; }

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            this.Client = client;
            this.Service = new CommandService();
            this.Client.MessageReceived += this.HandleCommandAsync;
            await this.Service.AddModulesAsync(Assembly.GetEntryAssembly(), Program.Services.Provider);
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;
            
            var context = new SocketCommandContext(this.Client, msg);
            var argPos = 0;
            
            if (msg.HasStringPrefix(Config.Bot.CmdPrefix,  ref argPos) && !context.IsPrivate)
            {
                var result = await this.Service.ExecuteAsync(context, argPos, Program.Services.Provider);
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
