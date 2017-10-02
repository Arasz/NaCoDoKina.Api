using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.DataProviders.Tasks;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies
{
    public class CinemaCityMoviesTask : TaskBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IEntitiesBuilder<Movie> _entitiesBuilder;

        public CinemaCityMoviesTask(IMovieRepository cinemaRepository, IEntitiesBuilder<Movie> entitiesBuilder, TasksSettings settings) : base(settings)
        {
            _movieRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
            _entitiesBuilder = entitiesBuilder ?? throw new ArgumentNullException(nameof(entitiesBuilder));
        }

        public override async Task Execute()
        {
            var movies = await _entitiesBuilder.BuildMany();

            await _movieRepository.CreateMoviesAsync(movies);
        }
    }
}