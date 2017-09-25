using AutoMapper;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Models;

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