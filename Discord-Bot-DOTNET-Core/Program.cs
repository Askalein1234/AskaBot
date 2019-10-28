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
            Console.WriteLine($"The prefix: \"{Config.GetGlobalPrefix()}\"");

            if (Config.GetToken() == null || Config.GetToken() == "") return;
            this.client = new DiscordSocketClient();
            this.client.Log += Log;
            await this.client.LoginAsync(TokenType.Bot, Config.GetToken());
            await this.client.StartAsync();

            this.handler = new EventHandler();
            await this.handler.InitializeAsync(this.client);
            await this.client.SetActivityAsync(new Game($"{Config.GetGlobalPrefix()}help", ActivityType.Listening));
            bool run = true;
            while (run)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "exit":
                        Config.Save();
                        run = false;
                        break;
                    case "config reload":
                        Config.Reload();
                        break;

                }
            }
            await this.client.LogoutAsync();
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
