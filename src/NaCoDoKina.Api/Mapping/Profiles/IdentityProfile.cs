using AutoMapper;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Models.Authentication;
using NaCoDoKina.Api.Models.Users;

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