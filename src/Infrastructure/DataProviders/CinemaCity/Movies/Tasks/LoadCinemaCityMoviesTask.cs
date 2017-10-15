using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.DataProviders.Tasks;
using Infrastructure.Settings.Tasks;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Movies.Tasks
{
    public class LoadCinemaCityMoviesTask : TaskBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IEntitiesBuilder<Movie, EmptyContext> _entitiesBuilder;

        public LoadCinemaCityMoviesTask(IMovieRepository movieRepository, IEntitiesBuilder<Movie, EmptyContext> entitiesBuilder, TasksSettings settings) : base(settings)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
            _entitiesBuilder = entitiesBuilder ?? throw new ArgumentNullException(nameof(entitiesBuilder));
        }

        public override async Task Execute()
        {
            var movies = await _entitiesBuilder.BuildMany();

            foreach (var movie in movies)
            {
                var title = movie.Title;
                var existingMovie = await _movieRepository.GetMovieByTitleAsync(title);
                if (existingMovie is null)
                    await _movieRepository.CreateMovieAsync(movie);
            }
        }
    }
}