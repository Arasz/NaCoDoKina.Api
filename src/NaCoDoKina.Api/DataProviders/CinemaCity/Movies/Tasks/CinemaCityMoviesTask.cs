using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.DataProviders.EntityBuilder.Context;
using NaCoDoKina.Api.DataProviders.Tasks;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Settings.Tasks;
using NaCoDoKina.Api.Repositories;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies.Tasks
{
    public class CinemaCityMoviesTask : TaskBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IEntitiesBuilder<Movie, EmptyContext> _entitiesBuilder;

        public CinemaCityMoviesTask(IMovieRepository movieRepository, IEntitiesBuilder<Movie, EmptyContext> entitiesBuilder, TasksSettings settings) : base(settings)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _entitiesBuilder = entitiesBuilder ?? throw new ArgumentNullException(nameof(entitiesBuilder));
        }

        public override async Task Execute()
        {
            var movies = await _entitiesBuilder.BuildMany();

            await _movieRepository.CreateMoviesAsync(movies);
        }
    }
}