using AutoMapper;
using Domain.Aggregates;
using Application.DTOs.Auth;

namespace Application.Mappings;

public class UserAccountMappingProfile: Profile
{
    public UserAccountMappingProfile()
    {
        CreateMap<UserAccount, AccountInfoDto>();
    }
}