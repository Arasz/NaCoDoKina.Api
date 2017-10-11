using ApplicationCore.Entities;
using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Entities.Movies;
using ApplicationCore.Entities.Resources;
using AutoMapper;

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
            CreateMap<Infrastructure.Models.Location, Location>()
                 .ReverseMap();

            CreateMap<string, CinemaNetwork>()
                .ConstructUsing((networkName, context) => new CinemaNetwork
                {
                    Name = networkName
                })
                .ReverseMap()
                .ConstructUsing(network => network.Name);

            CreateMap<Infrastructure.Models.Cinemas.Cinema, Cinema>()
                .ReverseMap()
                .ForMember(cinema => cinema.CinemaTravelInformation, cfg => cfg.Ignore())
                .ForMember(cinema => cinema.NetworkName, cfg => cfg.MapFrom(cinema => cinema.CinemaNetwork.Name));

            CreateMap<Infrastructure.Models.Resources.ReviewLink, ReviewLink>()
                .ReverseMap();

            CreateMap<Infrastructure.Models.Resources.MediaLink, MediaLink>()
                .ReverseMap();

            CreateMap<Infrastructure.Models.Movies.Movie, Movie>()
                .ForMember(movie => movie.ExternalMovies, cfg => cfg.Ignore())
                .ReverseMap();

            CreateMap<Infrastructure.Models.Movies.MovieDetails, MovieDetails>()
                .ReverseMap();

            CreateMap<Infrastructure.Models.Movies.MovieShowtime, MovieShowtime>()
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