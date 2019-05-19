using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Discord_Bot
{
    class Program
    {
        DiscordSocketClient client;
        EventHandler handler;

        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            Console.WriteLine("The token:  You thought you would get my token here? Forget it!");
            Console.WriteLine("The prefix: \"" + Config.bot.cmdPrefix + "\"");

            if (Config.bot.token == null || Config.bot.token == "") return;
            this.client = new DiscordSocketClient();
            this.client.Log += Log;
            await this.client.LoginAsync(TokenType.Bot, Config.bot.token);
            await this.client.StartAsync();

            this.handler = new EventHandler();
            await this.handler.InitializeAsync(this.client);
            await this.client.SetActivityAsync(new Game(Config.bot.cmdPrefix));
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "exit")
                    break;
            }
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
