using AutoMapper;
using Domain.Aggregates;
using Application.DTOs.ExerciseProposal;
using Domain.ValueObjects;

namespace Application.Mappings;

public class ExerciseProposalMappingProfile : Profile
{
    public ExerciseProposalMappingProfile()
    {
        CreateMap<ExerciseProposal, ExerciseProposalListItemDto>();
        CreateMap<ExerciseProposal, ExerciseProposalDetails>();
        CreateMap<ExerciseProposalDetails, ExerciseProposalDetailsDto>();
    }
}