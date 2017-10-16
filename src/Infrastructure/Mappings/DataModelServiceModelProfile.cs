using ApplicationCore.Entities;
using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Entities.Movies;
using ApplicationCore.Entities.Resources;
using AutoMapper;
using System.Linq;

namespace Infrastructure.Mappings
{
    /// <inheritdoc/>
    /// <summary>
    /// Profile for mapping service data model to/from persistence data model 
    /// </summary>
    public class DataModelServiceModelProfile : Profile
    {
        public DataModelServiceModelProfile()
        {
            CreateMap<Models.Location, Location>()
                 .ReverseMap();

            CreateMap<string, CinemaNetwork>()
                .ConstructUsing((networkName, context) => new CinemaNetwork
                {
                    Name = networkName
                })
                .ReverseMap()
                .ConstructUsing(network => network.Name);

            CreateMap<Models.Cinemas.Cinema, Cinema>()
                .ReverseMap()
                .ForMember(cinema => cinema.CinemaTravelInformation, cfg => cfg.Ignore())
                .ForMember(cinema => cinema.NetworkName, cfg => cfg.MapFrom(cinema => cinema.CinemaNetwork.Name))
                .ForMember(cinema => cinema.City, cfg => cfg.ResolveUsing(cinema => cinema.Address.Split(',').Last().Trim(' ')));

            CreateMap<Models.Resources.ReviewLink, ReviewLink>()
                .ReverseMap();

            CreateMap<Models.Resources.MediaLink, MediaLink>()
                .ReverseMap();

            CreateMap<Models.Movies.Movie, Movie>()
                .ForMember(movie => movie.ExternalMovies, cfg => cfg.Ignore())
                .ReverseMap();

            CreateMap<Models.Movies.MovieDetails, MovieDetails>()
                .ReverseMap();

            CreateMap<Models.Movies.MovieShowtime, MovieShowtime>()
                .ForMember(showtime => showtime.Cinema, cfg => cfg.ResolveUsing((showtime, movieShowtime) => new Cinema
                {
                    Id = showtime.CinemaId,
                }))
                .ForMember(showtime => showtime.Movie, cfg => cfg.ResolveUsing((showtime, movieShowtime) => new Movie
                {
                    Id = showtime.MovieId,
                }))
                .ReverseMap()
                .ForMember(showtime => showtime.CinemaId, cfg => cfg.MapFrom(showtime => showtime.Cinema.Id))
                .ForMember(showtime => showtime.MovieId, cfg => cfg.MapFrom(showtime => showtime.Movie.Id));
        }
    }
}