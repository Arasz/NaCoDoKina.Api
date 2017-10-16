using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Context;
using Infrastructure.DataProviders.EntityBuilder;
using Infrastructure.DataProviders.Tasks;
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
        private readonly IMovieShowtimeRepository _movieShowtimeRepository;
        private readonly ICinemaRepository _cinemaRepository;

        public LoadCinemaCityShowtimesTask(ICinemaRepository cinemaRepository,
            IMovieShowtimeRepository movieShowtimeRepository,
            IEntitiesBuilder<MovieShowtime, MovieShowtimesContext> entitiesBuilder,
            TasksSettings settings,
            ILogger<LoadCinemaCityShowtimesTask> logger)
            : base(entitiesBuilder, settings, logger)
        {
            _movieShowtimeRepository = movieShowtimeRepository ?? throw new ArgumentNullException(nameof(movieShowtimeRepository));
            _cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
        }

        protected override async Task BuildEntities()
        {
            var cinemas = await _cinemaRepository.GetAllCinemas();

            Logger.LogDebug("Loaded {CinemasCount} cinemas", cinemas.Count);

            Results = new List<MovieShowtime>();

            var dateOfNextTaskRun = Settings.ShowtimesTask.NextOccurrence();

            Logger.LogDebug("Date of the next task run {Date}", dateOfNextTaskRun);

            var showtimeDate = DateTime.Today;

            do
            {
                foreach (var cinema in cinemas)
                {
                    var context = new MovieShowtimesContext(cinema, showtimeDate);

                    Logger.LogDebug("Context {@Context} for cinema {@Cinema}", context, cinema);

                    var showtimes = (await EntitiesBuilder.BuildMany(parameters: context))
                        .ToArray();

                    Logger.LogDebug("Added {ShowtimesCount} new showtimes. First: {@FirstShowtime}", showtimes.Count(), showtimes.FirstOrDefault());

                    Results.AddRange(showtimes);
                }

                showtimeDate = showtimeDate.AddDays(1);
                Logger.LogDebug("New showtime date {ShowtimeDate}", showtimeDate);
            } while (dateOfNextTaskRun.Date != showtimeDate);
        }

        protected override async Task SaveResults()
        {
            await _movieShowtimeRepository.CreateMovieShowtimesAsync(Results);
        }
    }
}