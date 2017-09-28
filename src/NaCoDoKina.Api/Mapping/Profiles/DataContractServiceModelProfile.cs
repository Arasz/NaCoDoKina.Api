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
            CreateMap<Models.Location, DataContracts.Location>()
                .ReverseMap();

            CreateMap<Models.Cinemas.Cinema, DataContracts.Cinemas.Cinema>()
                .ReverseMap();

            CreateMap<Models.Resources.ResourceLink, DataContracts.Resources.ResourceLink>()
                .ReverseMap();

            CreateMap<Models.Resources.ReviewLink, DataContracts.Resources.ReviewLink>()
                .ReverseMap();

            CreateMap<Models.Resources.MediaLink, DataContracts.Resources.MediaLink>()
                .ReverseMap();

            CreateMap<Models.Movies.Movie, DataContracts.Movies.Movie>()
                .ReverseMap();

            CreateMap<Models.Movies.MovieDetails, DataContracts.Movies.MovieDetails>()
                .ReverseMap();

            CreateMap<Models.Movies.MovieShowtime, DataContracts.Movies.MovieShowtime>()
                .ReverseMap();

            CreateMap<Models.SearchArea, DataContracts.SearchArea>()
                .ReverseMap();
        }
    }
}