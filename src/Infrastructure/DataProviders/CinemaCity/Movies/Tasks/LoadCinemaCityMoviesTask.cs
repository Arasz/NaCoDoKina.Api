using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.DataProviders.Tasks;
using Infrastructure.Settings.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Movies.Tasks
{
    public class LoadCinemaCityMoviesTask : EntitiesBuildTask<Movie, EmptyContext>
    {
        private readonly IMovieRepository _movieRepository;

        public LoadCinemaCityMoviesTask(IMovieRepository movieRepository,
            IEntitiesBuilder<Movie, EmptyContext> entitiesBuilder,
            TasksSettings settings,
            ILogger<LoadCinemaCityMoviesTask> logger)
            : base(entitiesBuilder, settings, logger)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        }

        protected override async Task BuildEntities()
        {
            Results = (await EntitiesBuilder.BuildMany()).ToList();
            Logger.LogDebug("Built {BuiltMoviesCount} movies, first {@Movie}", Results.Count, Results.First());
        }

        protected override async Task SaveResults()
        {
            await _movieRepository.CreateMoviesAsync(Results);
        }
    }
}