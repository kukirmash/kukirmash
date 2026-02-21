using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kukirmash.Persistence.Configurations;

public class TextMessageConfiguration : IEntityTypeConfiguration<TextMessageEntity>
{
    public void Configure(EntityTypeBuilder<TextMessageEntity> builder)
    {
        builder.ToTable("TextMessages");

        builder.HasKey(tm => tm.Id);

        builder.Property(tm => tm.Text)
            .IsRequired()
            .HasMaxLength(1023);

        builder.Property(tm => tm.CreatedDateTimeUtc)
            .IsRequired();

        builder.HasOne(tm => tm.TextChannel)
            .WithMany(tch => tch.TextMessages)
            .HasForeignKey(tm => tm.TextChannelId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tm => tm.Creator)
            .WithMany(u => u.TextMessages)
            .HasForeignKey(tm => tm.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(tm => new { tm.TextChannelId, tm.CreatedDateTimeUtc });
    }
}
