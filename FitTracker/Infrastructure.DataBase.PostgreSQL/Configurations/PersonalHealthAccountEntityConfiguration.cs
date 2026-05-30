using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PersonalHealthAccountEntityConfiguration: IEntityTypeConfiguration<PersonalHealthAccount>
{
    public void Configure(EntityTypeBuilder<PersonalHealthAccount> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne<UserAccount>()
            .WithOne()
            .HasForeignKey<PersonalHealthAccount>(x => x.Id);

        builder.OwnsOne(x => x.Profile, profile =>
        {
            profile.Property(p => p.DateOfBirth).HasColumnName("date_of_birth");
            profile.Property(p => p.Sex).HasColumnName("sex");
            profile.OwnsOne(p => p.Weight, weight =>
            {
                weight.Property(w => w.Kilograms).HasColumnName("weight");
            });
            profile.OwnsOne(x => x.Height, height =>
            {
                height.Property(h => h.Centimeters).HasColumnName("height");
            });
        });
    }
}