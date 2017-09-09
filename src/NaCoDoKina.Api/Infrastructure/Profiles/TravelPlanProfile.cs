using AutoMapper;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Models;

namespace NaCoDoKina.Api.Infrastructure.Profiles
{
    public class TravelPlanProfile : Profile
    {
        public TravelPlanProfile()
        {
            CreateMap<TravelPlan, DirectionsApiRequest>();
        }
    }
}