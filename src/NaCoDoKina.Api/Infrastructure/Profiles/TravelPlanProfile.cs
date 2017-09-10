using AutoMapper;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Models;
using System.Linq;
using TravelMode = NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request.TravelMode;

namespace NaCoDoKina.Api.Infrastructure.Profiles
{
    public class TravelPlanProfile : Profile
    {
        public TravelPlanProfile()
        {
            CreateMap<Models.TravelMode, TravelMode>()
                .ReverseMap();

            CreateMap<string, Location>()
                .ConstructUsing(location =>
                {
                    var latLng = location.Split(',')
                        .Select(double.Parse)
                        .ToArray();

                    return new Location(latLng[0], latLng[1]);
                });

            CreateMap<TravelPlan, DirectionsApiRequest>()
                .ReverseMap();
        }
    }
}