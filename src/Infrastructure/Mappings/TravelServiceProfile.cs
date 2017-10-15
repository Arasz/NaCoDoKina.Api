using AutoMapper;
using Infrastructure.Models;
using Infrastructure.Models.Travel;
using Infrastructure.Services.Google.DataContract.Directions.Request;
using Infrastructure.Services.Google.DataContract.Geocoding.Request;
using System.Linq;
using TravelMode = Infrastructure.Services.Google.DataContract.Directions.Request.TravelMode;

namespace Infrastructure.Mappings
{
    public class TravelServiceProfile : Profile
    {
        public TravelServiceProfile()
        {
            CreateMap<Models.Travel.TravelMode, TravelMode>()
                .ReverseMap();

            CreateMap<Location, Services.Google.DataContract.Common.Location>()
                .ReverseMap();

            CreateMap<string, Location>()
                .ConstructUsing(location =>
                {
                    var lngLat = location.Split(',')
                        .Select(double.Parse)
                        .ToArray();

                    return new Location(latitude: lngLat[0], longitude: lngLat[1]);
                })
                ;

            CreateMap<string, GeocodingApiRequest>()
                .ConstructUsing(address => new GeocodingApiRequest(address));

            CreateMap<TravelPlan, DirectionsApiRequest>()
                .ReverseMap();
        }
    }
}