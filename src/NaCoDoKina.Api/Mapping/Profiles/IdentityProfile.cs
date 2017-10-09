using AutoMapper;
using Infrastructure.Identity;
using Infrastructure.Models.Authentication;
using Infrastructure.Models.Users;
using NaCoDoKina.Api.DataContracts.Authentication;

namespace NaCoDoKina.Api.Mapping.Profiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<AuthToken, JwtToken>()
                .ReverseMap();

            CreateMap<User, RegisterUser>()
                .ReverseMap();

            CreateMap<User, Credentials>()
                .ReverseMap();

            CreateMap<User, ApplicationUser>()
                .ReverseMap();
        }
    }
}