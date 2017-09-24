using AutoMapper;

namespace NaCoDoKina.Api.Mapping.Profiles
{
    /// <inheritdoc/>
    /// <summary>
    /// Profile for mapping api data model (contract) to/from service data model 
    /// </summary>
    public class DataContractServiceModelProfile : Profile
    {
        public DataContractServiceModelProfile()
        {
            CreateMap<Models.Location, DataContracts.Movies.Location>()
                .ReverseMap();

            CreateMap<Models.Cinema, DataContracts.Movies.Cinema>()
                .ReverseMap();

            CreateMap<Models.ServiceUrl, DataContracts.Movies.ServiceUrl>()
                .ReverseMap();

            CreateMap<Models.Movie, DataContracts.Movies.Movie>()
                .ReverseMap();

            CreateMap<Models.MovieDetails, DataContracts.Movies.MovieDetails>()
                .ReverseMap();

            CreateMap<Models.MovieShowtime, DataContracts.Movies.MovieShowtime>()
                .ReverseMap();

            CreateMap<Models.SearchArea, DataContracts.Movies.SearchArea>()
                .ReverseMap();
        }
    }
}