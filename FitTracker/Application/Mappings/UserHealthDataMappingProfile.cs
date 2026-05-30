using AutoMapper;
using Domain.Aggregates;
using Application.DTOs.UserHealth;

namespace Application.Mappings;

public class UserHealthDataMappingProfile : Profile
{
    public UserHealthDataMappingProfile()
    {
        CreateMap<PersonalHealthAccount, PersonalHealthAccountDto>()
            .ForMember(dest => dest.Weight, 
                opt => opt.MapFrom(scr => scr.Profile.Weight.Kilograms))
            .ForMember(dest => dest.Height,
                opt => opt.MapFrom(scr => scr.Profile.Height.Centimeters))
            .ForMember(dest => dest.DateOfBirth,
            opt => opt.MapFrom(scr => scr.Profile.DateOfBirth))
            .ForMember(dest => dest.Sex,
                opt => opt.MapFrom(scr => scr.Profile.Sex));
    }
}