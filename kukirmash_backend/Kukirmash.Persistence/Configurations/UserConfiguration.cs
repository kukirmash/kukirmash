using Kukirmash.Persistence.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kukirmash.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    //---------------------------------------------------------------------------
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(user => user.Id);

        builder.Property(user => user.Login)
            .IsRequired()
            .HasMaxLength(31);

        builder.Property(user => user.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(user => user.PasswordHash).IsRequired();

        // Индексы
        builder.HasIndex(user => user.Email).IsUnique(); // Уникальный Email
        builder.HasIndex(user => user.Login).IsUnique(); // Уникальный Login
    }
    
    //---------------------------------------------------------------------------
}
