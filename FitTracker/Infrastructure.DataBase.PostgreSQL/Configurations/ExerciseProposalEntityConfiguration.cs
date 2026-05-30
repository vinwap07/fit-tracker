using Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ExerciseProposalEntityConfiguration: IEntityTypeConfiguration<ExerciseProposal>
{
    public void Configure(EntityTypeBuilder<ExerciseProposal> builder)
    {
        builder.HasOne<UserAccount>()
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.OwnsOne(x => x.Photo, media =>
        {
            media.Property(x => x.Bucket).HasColumnName("Bucket");
            media.Property(x => x.Key);
        });
        builder.OwnsOne(x => x.EmgVideo, media =>
        {
            media.Property(x => x.Bucket);
            media.Property(x => x.Key);
        });
        builder.OwnsOne(x => x.PerformanceVideo, media =>
        {
            media.Property(x => x.Bucket);
            media.Property(x => x.Key);
        });
    }
}