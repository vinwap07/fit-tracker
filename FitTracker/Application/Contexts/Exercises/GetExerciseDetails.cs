using Application.Abstractions;
using Application.Abstractions.Queries;
using Application.DTOs.Exercise;
using Application.Exceptions;
using AutoMapper;
using MediatR;

namespace Application.Exercises;

public record GetExerciseDetailsQuery(Guid ExerciseId) : IRequest<ExerciseDetailDto>;

public class GetExerciseDetailsQueryHandler(
    IExerciseQueries exerciseQueries,
    IFileStorageService fileStorageService,
    IMapper mapper
) : IRequestHandler<GetExerciseDetailsQuery, ExerciseDetailDto>
{
    public async Task<ExerciseDetailDto> Handle(GetExerciseDetailsQuery request, CancellationToken ct)
    {
        var exercise =  await exerciseQueries.GetByIdAsync(request.ExerciseId, ct)
                        ?? throw new NotFoundException("exercise", request.ExerciseId);
        exercise.PhotoUrl = await fileStorageService.GetPresignedUrlAsync(exercise.Bucket, exercise.PhotoKey);
        exercise.PerformanceVideoUrl = await fileStorageService.GetPresignedUrlAsync(exercise.Bucket, exercise.PerformanceVideoKey);
        return mapper.Map<ExerciseDetailDto>(exercise);
    }
}