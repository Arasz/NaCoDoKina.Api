using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Context;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.Tasks;
using Infrastructure.DataProviders.Timeline;
using Infrastructure.Settings.Tasks;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Showtimes.Tasks
{
    public class LoadCinemaCityShowtimesTask : EntitiesBuildTask<MovieShowtime, MovieShowtimesContext>
    {
        private readonly ILimitedTimeline _showtimesTaskTimeline;
        private readonly IMovieShowtimeRepository _movieShowtimeRepository;
        private readonly ICinemaRepository _cinemaRepository;

        public LoadCinemaCityShowtimesTask(ICinemaRepository cinemaRepository,
            IMovieShowtimeRepository movieShowtimeRepository,
            IEntitiesBuilder<MovieShowtime, MovieShowtimesContext> entitiesBuilder,
            ILimitedTimeline showtimesTaskTimeline,
            TasksSettings settings,
            ILogger<LoadCinemaCityShowtimesTask> logger)
            : base(entitiesBuilder, settings, logger)
        {
            _showtimesTaskTimeline = showtimesTaskTimeline ?? throw new ArgumentNullException(nameof(showtimesTaskTimeline));
            _movieShowtimeRepository = movieShowtimeRepository ?? throw new ArgumentNullException(nameof(movieShowtimeRepository));
            _cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
        }

        protected override async Task BuildEntities()
        {
            var cinemas = await _cinemaRepository.GetAllCinemas();

            Logger.LogDebug("Loaded {CinemasCount} cinemas", cinemas.Count);

            Results = new List<MovieShowtime>();

            Logger.LogDebug("Showtimes timeline {@Timeline}", _showtimesTaskTimeline);

            do
            {
                foreach (var cinema in cinemas)
                {
                    var context = new MovieShowtimesContext(cinema, _showtimesTaskTimeline.Position);

                    Logger.LogDebug("Context {@Context} for cinema {@Cinema}", context, cinema);

                    var showtimes = (await EntitiesBuilder.BuildMany(parameters: context))
                        .ToArray();

                    Logger.LogDebug("Added {ShowtimesCount} new showtimes", showtimes.Count());

                    Results.AddRange(showtimes);
                }

                _showtimesTaskTimeline.Move();
                Logger.LogDebug("New showtime date {ShowtimeDate}", _showtimesTaskTimeline.Position);
            } while (_showtimesTaskTimeline.IsInRange());
        }

        protected override async Task SaveResults()
        {
            await _movieShowtimeRepository.CreateMovieShowtimesAsync(Results);
        }
    }
}