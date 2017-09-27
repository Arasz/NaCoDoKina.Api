using AutoMapper;
using NaCoDoKina.Api.Entities;

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
            CreateMap<Models.Location, Location>()
                 .ReverseMap();

            CreateMap<string, CinemaNetwork>()
                .ConstructUsing((networkName, context) => new CinemaNetwork { Name = networkName });

            CreateMap<CinemaNetwork, string>()
                .ConstructUsing(network => network.Name);

            CreateMap<Models.Cinema, Cinema>()
                .ReverseMap()
                .ForMember(cinema => cinema.CinemaTravelInformation, cfg => cfg.Ignore());

            CreateMap<Models.ServiceUrl, ServiceUrl>()
                .ReverseMap();

            CreateMap<Models.Movie, Movie>()
                .ReverseMap();

            CreateMap<Models.MovieDetails, MovieDetails>()
                .ReverseMap();

            CreateMap<Models.MovieShowtime, MovieShowtime>()
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