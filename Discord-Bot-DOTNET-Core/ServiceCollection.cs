using System;
using Discord_Bot.Data;
using Discord_Bot.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_Bot
{
    internal class ServiceCollection
    {
        public IServiceProvider Provider { get; } = new Microsoft.Extensions.DependencyInjection.ServiceCollection()
            .AddDbContext<BotDbContext>(config => { config.UseSqlite("Data Source=db.db"); })
            .BuildServiceProvider();
    }
}