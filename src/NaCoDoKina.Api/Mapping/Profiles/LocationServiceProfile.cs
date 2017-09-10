using AutoMapper;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;
using NaCoDoKina.Api.Models;
using System.Linq;
using TravelMode = NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request.TravelMode;

namespace NaCoDoKina.Api.Mapping.Profiles
{
    public class LocationServiceProfile : Profile
    {
        public LocationServiceProfile()
        {
            CreateMap<Models.TravelMode, TravelMode>()
                .ReverseMap();

            CreateMap<string, Location>()
                .ConstructUsing(location =>
                {
                    var lngLat = location.Split(',')
                        .Select(double.Parse)
                        .ToArray();

                    return new Location(longitude: lngLat[0], latitude: lngLat[1]);
                });

            CreateMap<string, GeocodingApiRequest>()
                .ConstructUsing(address => new GeocodingApiRequest(address));

            CreateMap<TravelPlan, DirectionsApiRequest>()
                .ReverseMap();
        }
    }
}