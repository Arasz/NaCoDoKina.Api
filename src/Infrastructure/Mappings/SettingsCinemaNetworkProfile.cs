using AutoMapper;
using Infrastructure.Settings;
using Infrastructure.Settings.CinemaNetwork;

namespace Infrastructure.Mappings
{
    public class SettingsCinemaNetworkProfile : Profile
    {
        public SettingsCinemaNetworkProfile()
        {
            CreateMap<ApplicationCore.Entities.Cinemas.CinemaNetwork, CinemaNetwork>()
                .ForMember(network => network.Url, cfg => cfg.MapFrom(network => network.CinemaNetworkUrl))
                .ReverseMap()
                .ForMember(network => network.CinemaNetworkUrl, cfg => cfg.MapFrom(network => network.Url));
        }
    }
}