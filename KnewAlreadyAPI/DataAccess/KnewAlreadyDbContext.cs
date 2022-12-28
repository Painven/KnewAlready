using KnewAlreadyAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI.DataAccess;

public class KnewAlreadyDbContext : DbContext
{
    public DbSet<SuggestActionItem> SuggestActionItems { get; set; }
    public DbSet<User> Users { get; set; }

    public KnewAlreadyDbContext(DbContextOptions<KnewAlreadyDbContext> options) : base(options)
    {
        this.Database.EnsureCreated();

    }
}
