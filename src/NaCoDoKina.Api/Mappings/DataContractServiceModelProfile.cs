using AutoMapper;
using Infrastructure.Models;
using Infrastructure.Models.Cinemas;
using Infrastructure.Models.Movies;
using Infrastructure.Models.Resources;

namespace NaCoDoKina.Api.Mappings
{
    /// <inheritdoc/>
    /// <summary>
    /// Profile for mapping api data model (contract) to/from service data model 
    /// </summary>
    public class DataContractServiceModelProfile : Profile
    {
        public DataContractServiceModelProfile()
        {
            CreateMap<Location, DataContracts.Location>()
                .ReverseMap();

            CreateMap<Cinema, DataContracts.Cinemas.Cinema>()
                .ReverseMap();

            CreateMap<ReviewLink, DataContracts.Resources.ReviewLink>()
                .ReverseMap();

            CreateMap<MediaLink, DataContracts.Resources.MediaLink>()
                .ReverseMap();

            CreateMap<Movie, DataContracts.Movies.Movie>()
                .ReverseMap();

            CreateMap<MovieDetails, DataContracts.Movies.MovieDetails>()
                .ReverseMap();

            CreateMap<MovieShowtime, DataContracts.Movies.MovieShowtime>()
                .ReverseMap();

            CreateMap<SearchArea, DataContracts.SearchArea>()
                .ReverseMap();
        }
    }
}