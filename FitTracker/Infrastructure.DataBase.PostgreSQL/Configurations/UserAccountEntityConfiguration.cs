using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class UserAccountEntityConfiguration: IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.Property(x => x.Email)
            .HasMaxLength(254)
            .IsRequired();
        builder.Property(x => x.Login)
            .HasMaxLength(30)
            .IsRequired();
        builder.Property<string>("_passwordHash")
            .HasColumnName("password_hash");
        builder.Property(x => x.Role)
            .HasDefaultValue("user");
        // TODO: переделать взятие названия ролей из конфигурации
    }
}