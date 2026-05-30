using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class WeightHistoryEntityConfiguration: IEntityTypeConfiguration<WeightHistoryPoint>
{
    public void Configure(EntityTypeBuilder<WeightHistoryPoint> builder)
    {
        builder.Property(x => x.Date)
            .HasDefaultValue(DateOnly.FromDateTime(DateTime.Now));
        builder.Property(x => x.UserId)
            .IsRequired();
        builder.OwnsOne(p => p.Weight, weight =>
        {
            weight.Property(w => w.Kilograms).HasColumnName("weight");
        });
    }
}