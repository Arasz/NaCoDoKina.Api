﻿using AutoMapper;
using Infrastructure.Models;
using Infrastructure.Models.Travel;
using Infrastructure.Services.Google.DataContract.Directions.Request;
using Infrastructure.Services.Google.DataContract.Geocoding.Request;
using System.Linq;
using TravelMode = Infrastructure.Services.Google.DataContract.Directions.Request.TravelMode;

namespace NaCoDoKina.Api.Mapping.Profiles
{
    public class TravelServiceProfile : Profile
    {
        public TravelServiceProfile()
        {
            CreateMap<Infrastructure.Models.Travel.TravelMode, TravelMode>()
                .ReverseMap();

            CreateMap<Location, Infrastructure.Services.Google.DataContract.Common.Location>()
                .ReverseMap();

            CreateMap<string, Location>()
                .ConstructUsing(location =>
                {
                    var lngLat = location.Split(',')
                        .Select(double.Parse)
                        .ToArray();

                    return new Location(longitude: lngLat[0], latitude: lngLat[1]);
                })
                ;

            CreateMap<string, GeocodingApiRequest>()
                .ConstructUsing(address => new GeocodingApiRequest(address));

            CreateMap<TravelPlan, DirectionsApiRequest>()
                .ReverseMap();
        }
    }
}