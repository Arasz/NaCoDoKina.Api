using System;
using System.Threading.Tasks;
using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.DataProviders.Tasks;
using Infrastructure.Settings.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Movies.Tasks
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