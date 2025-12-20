using System.Reflection;
using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;

namespace Kukirmash.Persistence;

public class KukirmashDbContext : DbContext
{
    public KukirmashDbContext(DbContextOptions<KukirmashDbContext> options) : base(options)
    {

    }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<ServerEntity> Servers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // находит UserConfiguration и ServerConfiguration в текущей сборке и применяет их.
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
