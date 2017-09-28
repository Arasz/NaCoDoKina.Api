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

            CreateMap<string, Entities.Cinemas.CinemaNetwork>()
                .ConstructUsing((networkName, context) => new Entities.Cinemas.CinemaNetwork
                {
                    Name = networkName
                });

            CreateMap<Entities.Cinemas.CinemaNetwork, string>()
                .ConstructUsing(network => network.Name);

            CreateMap<Models.Cinemas.Cinema, Entities.Cinemas.Cinema>()
                .ReverseMap()
                .ForMember(cinema => cinema.CinemaTravelInformation, cfg => cfg.Ignore());

            CreateMap<Models.Resources.ResourceLink, Entities.Resources.ResourceLink>()
                .ReverseMap();

            CreateMap<Models.Resources.ReviewLink, Entities.Resources.ReviewLink>()
                .ReverseMap();

            CreateMap<Models.Resources.MediaLink, Entities.Resources.MediaLink>()
                .ReverseMap();

            CreateMap<Models.Movies.Movie, Entities.Movies.Movie>()
                .ReverseMap();

            CreateMap<Models.Movies.MovieDetails, Entities.Movies.MovieDetails>()
                .ReverseMap();

            CreateMap<Models.Movies.MovieShowtime, Entities.Movies.MovieShowtime>()
                .ForMember(showtime => showtime.Cinema, cfg => cfg.ResolveUsing((showtime, movieShowtime) => new Entities.Cinemas.Cinema
                {
                    Id = showtime.CinemaId,
                }))
                .ForMember(showtime => showtime.Movie, cfg => cfg.ResolveUsing((showtime, movieShowtime) => new Entities.Movies.Movie
                {
                    Id = showtime.MovieId,
                }))
                .ReverseMap()
                .ForMember(showtime => showtime.CinemaId, cfg => cfg.MapFrom(showtime => showtime.Cinema.Id))
                .ForMember(showtime => showtime.MovieId, cfg => cfg.MapFrom(showtime => showtime.Movie.Id));
        }
    }
}