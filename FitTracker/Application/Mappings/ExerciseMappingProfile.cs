using AutoMapper;
using Domain.Aggregates;
using Application.DTOs.Exercise;

namespace Application.Mappings;

public class ExerciseMappingProfile : Profile
{
    public ExerciseMappingProfile()
    {
        CreateMap<Exercise, ExerciseListItem>();
        CreateMap<ExerciseListItem, ExerciseListItemDto>();
        CreateMap<Exercise, ExerciseDetail>();
        CreateMap<ExerciseDetail, ExerciseDetailDto>();
    }
}