﻿using AutoMapper;
using Infrastructure.Settings;

namespace NaCoDoKina.Api.Mapping.Profiles
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