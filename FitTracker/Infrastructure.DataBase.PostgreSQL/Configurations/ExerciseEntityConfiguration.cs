using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ExerciseEntityConfiguration: IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.Property(x => x.Name)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(x => x.Description)
            .IsRequired();
        builder.Property(x => x.Met)
            .IsRequired();
        builder.OwnsOne(x => x.Photo, media =>
        {
            media.Property(x => x.Bucket).HasColumnName("Bucket");
            media.Property(x => x.Key);
        });
        builder.OwnsOne(x => x.PerformanceVideo, media =>
        {
            media.Property(x => x.Bucket);
            media.Property(x => x.Key);
        });
    }
}