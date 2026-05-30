using Application.DTOs.UserHealth;
using AutoMapper;
using Domain.Aggregates;

namespace Application.Mappings;

public class WeightHistoryMappingProfile : Profile
{
    public WeightHistoryMappingProfile()
    {
        CreateMap<WeightHistoryPoint, WeightHistoryListItemDto>()
            .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight.Kilograms));
    }
}
