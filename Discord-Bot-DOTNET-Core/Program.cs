using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Discord_Bot
{
    class Program
    {
        public static ServiceCollection Services { get; } = new ServiceCollection();
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        private async Task StartAsync()
        {
            Console.WriteLine("The token:  You thought you would get my token here? Forget it!");
            Console.WriteLine("The prefix: \"" + Config.Bot.CmdPrefix + "\"");

            if (string.IsNullOrEmpty(Config.Bot.Token)) return;
            this._client = new DiscordSocketClient();
            this._client.Log += Log;
            await this._client.LoginAsync(TokenType.Bot, Config.Bot.Token);
            await this._client.StartAsync();

            this._handler = new CommandHandler();
            await this._handler.InitializeAsync(this._client);
            await this._client.SetActivityAsync(new Game(Config.Bot.CmdPrefix));
            while (!Console.ReadLine()?.Equals("exit") ?? false) { }
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}
