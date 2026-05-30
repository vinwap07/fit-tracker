using Application.Abstractions;
using Application.Abstractions.Queries;
using Application.DTOs.Exercise;
using AutoMapper;
using MediatR;

namespace Application.Exercises;

public record GetExercisesQuery : IRequest<List<ExerciseListItemDto>>;

public class GetExercisesQueryHandler(
    IExerciseQueries exerciseQueries,
    IFileStorageService fileStorageService,
    IMapper mapper
) : IRequestHandler<GetExercisesQuery, List<ExerciseListItemDto>>
{
    public async Task<List<ExerciseListItemDto>> Handle(GetExercisesQuery request, CancellationToken ct)
    {
        var exercises = await exerciseQueries.GetExercisesAsync(ct);
        foreach (var exercise in exercises)
        {
            exercise.PhotoUrl = await fileStorageService.GetPresignedUrlAsync(exercise.Bucket, exercise.PhotoKey);
        }
        
        return mapper.Map<List<ExerciseListItemDto>>(exercises);
    }
}