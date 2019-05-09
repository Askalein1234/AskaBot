using Microsoft.EntityFrameworkCore;

namespace Discord_Bot.Data
{
    public class BotDbContext: DbContext
    {
        public BotDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}