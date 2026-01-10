using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kukirmash.Persistence.Configurations;

public class ServerConfiguration : IEntityTypeConfiguration<ServerEntity>
{
    public void Configure(EntityTypeBuilder<ServerEntity> builder)
    {
        builder.HasKey(server => server.Id);

        builder.Property(server => server.Name)
            .IsRequired()
            .HasMaxLength(31);

        builder.Property(server => server.Description)
            .HasMaxLength(255);

        builder.Property(server => server.IconPath);

        builder.Property(server => server.IsPrivate)
            .IsRequired();

        builder.HasOne(server => server.Creator)
            .WithMany(user => user.CreatedServers)
            .HasForeignKey(server => server.CreatorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(server => server.Users)
            .WithMany(user => user.Servers)
            .UsingEntity(j => j.ToTable("ServerUser"));
    }
}
