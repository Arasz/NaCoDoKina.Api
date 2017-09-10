using AutoMapper;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;

namespace NaCoDoKina.Api.Infrastructure.Profiles
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, Entities.Location>()
                .ReverseMap();

            CreateMap<DataContracts.Location, Entities.Location>()
                .ReverseMap();

            CreateMap<Models.Location, DataContracts.Location>()
                .ReverseMap();
        }
    }
}