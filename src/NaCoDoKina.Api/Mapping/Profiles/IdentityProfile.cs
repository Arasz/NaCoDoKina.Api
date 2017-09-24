using AutoMapper;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Services.Token;

namespace NaCoDoKina.Api.Mapping.Profiles
{
    public class IdentityProfile : Profile
    {
        public IdentityProfile()
        {
            CreateMap<AuthenticationToken, JwtToken>()
                .ReverseMap();
        }
    }
}