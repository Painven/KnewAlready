using KnewAlreadyAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace KnewAlreadyAPI.DataAccess;

public class KnewAlreadyDbContext : DbContext
{
    public DbSet<SuggestActionItem> SuggestActionItems { get; set; }
    public DbSet<User> Users { get; set; }

    protected KnewAlreadyDbContext(DbContextOptions<KnewAlreadyDbContext> options)
    {
    }
}
