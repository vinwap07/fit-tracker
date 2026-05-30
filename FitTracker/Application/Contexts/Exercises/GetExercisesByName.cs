using Application.Abstractions;
using Application.Abstractions.Queries;
using Application.DTOs.Exercise;
using AutoMapper;
using MediatR;

namespace Application.Exercises;

public record GetExercisesByNameQuery(string Name): IRequest<List<ExerciseListItemDto>>;

public class GetExercisesByNameQueryHandler(
    IExerciseQueries exerciseQueries,
    IFileStorageService fileStorageService,
    IMapper mapper
    ) : IRequestHandler<GetExercisesByNameQuery, List<ExerciseListItemDto>>
{
    public async Task<List<ExerciseListItemDto>> Handle(GetExercisesByNameQuery request, CancellationToken ct)
    {
        var exercises = await exerciseQueries.GetExercisesByNameAsync(request.Name, ct);
        foreach (var exercise in exercises)
        {
            exercise.PhotoUrl = await fileStorageService.GetPresignedUrlAsync(exercise.Bucket, exercise.PhotoKey);
        }
        
        return mapper.Map<List<ExerciseListItemDto>>(exercises);
    }
}