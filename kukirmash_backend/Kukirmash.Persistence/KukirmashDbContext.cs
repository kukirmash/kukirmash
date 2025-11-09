using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;

namespace Kukirmash.Persistence;

public class KukirmashDbContext : DbContext
{
    public KukirmashDbContext(DbContextOptions<KukirmashDbContext> options) : base(options)
    {

    }

    public DbSet<UserEntity> Users { get; set; }
}
