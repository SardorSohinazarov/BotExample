using Bot.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bot.Data;

public class BotDbContext : DbContext
{
    public BotDbContext(DbContextOptions<BotDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
}
