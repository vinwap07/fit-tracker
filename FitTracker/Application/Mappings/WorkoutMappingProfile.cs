using AutoMapper;
using Domain.Aggregates;
using Application.DTOs.Workout;
using Domain.Entities;

namespace Application.Mappings;

public class WorkoutMappingProfile : Profile
{
    public WorkoutMappingProfile()
    {
        CreateMap<WorkoutExercise, WorkoutExerciseItemDto>()
            .ForMember(dest => dest.Weight,
                opt => opt.MapFrom(src => src.Volume.Weight.Kilograms))
            .ForMember(dest => dest.Repetitions,
                opt => opt.MapFrom(src => src.Volume.Repetitions))
            .ForMember(dest => dest.Name, opt => opt.Ignore());

        CreateMap<Workout, WorkoutDetailsDto>()
            .ForMember(dest => dest.Exercises,
                opt => opt.MapFrom(src => src.Exercises));

        CreateMap<Workout, WorkoutListItemDto>();

        CreateMap<Workout, WorkoutChartPointDto>()
            .ForMember(dest => dest.Date,
                opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)));

        CreateMap<Workout, WorkoutCaloriesChartPointDto>()
            .ForMember(dest => dest.Calories,
                opt => opt.MapFrom(src => src.TotalCalories))
            .ForMember(dest => dest.Date,
                opt => opt.MapFrom(src => DateOnly.FromDateTime(src.Date)));
    }
}