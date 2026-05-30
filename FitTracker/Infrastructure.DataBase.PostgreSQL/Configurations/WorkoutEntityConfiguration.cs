using Domain.Aggregates;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class WorkoutEntityConfiguration: IEntityTypeConfiguration<Workout>
{
    public void Configure(EntityTypeBuilder<Workout> builder)
    {
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.TotalCalories).IsRequired();
        builder.OwnsMany(x => x.Exercises, exercise =>
            {
                exercise.ToTable("workout_exercises");
        
                exercise.WithOwner().HasForeignKey("workout_id");
        
                exercise.Property(x => x.TotalCalories);

                exercise.OwnsOne(x => x.Volume, volume =>
                {
                    volume.Property(x => x.Repetitions).HasColumnName("repetitions");
            
                    volume.OwnsOne(p => p.Weight, weight =>
                    {
                        weight.Property(w => w.Kilograms).HasColumnName("weight_kg");
                    });
                });
            })
            .UsePropertyAccessMode(PropertyAccessMode.Field); 
    }
}