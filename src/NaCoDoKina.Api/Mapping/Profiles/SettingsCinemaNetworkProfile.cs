using AutoMapper;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Infrastructure.Settings;

namespace NaCoDoKina.Api.Mapping.Profiles
{
    public class SettingsCinemaNetworkProfile : Profile
    {
        public SettingsCinemaNetworkProfile()
        {
            CreateMap<string, ResourceLink>()
                .ConstructUsing(url => new ResourceLink(url))
                .ReverseMap()
                .ConstructUsing(link => link.Url);

            CreateMap<Entities.Cinemas.CinemaNetwork, CinemaNetwork>()
                .ReverseMap();
        }
    }
}