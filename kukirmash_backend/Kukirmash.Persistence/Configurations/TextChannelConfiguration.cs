using Kukirmash.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kukirmash.Persistence.Configurations;

public class TextChannelConfiguration : IEntityTypeConfiguration<TextChannelEntity>
{
    public void Configure(EntityTypeBuilder<TextChannelEntity> builder)
    {
        builder.ToTable("TextChannels");

        builder.HasKey(tch => tch.Id);

        builder.Property(tch => tch.Name)
            .IsRequired()
            .HasMaxLength(31);

        builder.HasOne(tch => tch.Server)
            .WithMany(server => server.TextChannels)
            .HasForeignKey(tch => tch.ServerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
