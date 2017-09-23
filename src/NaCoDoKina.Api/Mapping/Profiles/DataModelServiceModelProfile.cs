using AutoMapper;

namespace NaCoDoKina.Api.Mapping.Profiles
{
    /// <inheritdoc/>
    /// <summary>
    /// Profile for mapping service data model to/from persistence data model 
    /// </summary>
    public class DataModelServiceModelProfile : Profile
    {
        public DataModelServiceModelProfile()
        {
            CreateMap<Models.Location, Entities.Location>()
                 .ReverseMap();

            CreateMap<Models.Cinema, Entities.Cinema>()
                .ReverseMap();

            CreateMap<Models.ServiceUrl, Entities.ServiceUrl>()
                .ReverseMap();

            CreateMap<Models.Movie, Entities.Movie>()
                .ReverseMap();

            CreateMap<Models.MovieDetails, Entities.MovieDetails>()
                .ReverseMap();

            CreateMap<Models.MovieShowtime, Entities.MovieShowtime>()
                .ReverseMap();
        }
    }
}