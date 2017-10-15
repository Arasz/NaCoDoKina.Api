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
    public class CinemaCityShowtimesTask : TaskBase
    {
        private readonly ILogger<CinemaCityShowtimesTask> _logger;
        private readonly IMovieShowtimeRepository _movieShowtimeRepository;
        private readonly IEntitiesBuilder<MovieShowtime, MovieShowtimesContext> _entitiesBuilder;
        private readonly ICinemaRepository _cinemaRepository;

        public CinemaCityShowtimesTask(ICinemaRepository cinemaRepository, IMovieShowtimeRepository movieShowtimeRepository,
            IEntitiesBuilder<MovieShowtime, MovieShowtimesContext> entitiesBuilder, TasksSettings settings, ILogger<CinemaCityShowtimesTask> logger)
            : base(settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _movieShowtimeRepository = movieShowtimeRepository ?? throw new ArgumentNullException(nameof(movieShowtimeRepository));
            _entitiesBuilder = entitiesBuilder ?? throw new ArgumentNullException(nameof(entitiesBuilder));
            _cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
        }

        public override async Task Execute()
        {
            using (_logger.BeginScope(nameof(CinemaCityShowtimesTask)))
            {
                _logger.LogDebug("Start execution");

                var cinemas = await _cinemaRepository.GetAllCinemas();

                _logger.LogDebug("Loaded {CinemasCount} cinemas", cinemas.Count);

                var allShowtimes = new List<MovieShowtime>();

                var dateOfNextTaskRun = Settings.ShowtimesTask.NextOccurrence();

                _logger.LogDebug("Date of the next task run {Date}", dateOfNextTaskRun);

                var showtimeDate = DateTime.Today;

                do
                {
                    foreach (var cinema in cinemas)
                    {
                        var context = new MovieShowtimesContext(cinema, showtimeDate);

                        _logger.LogDebug("Context {@Context} for cinema {@Cinema}", context, cinema);

                        var showtimes = (await _entitiesBuilder
                            .BuildMany(parameters: context))
                            .ToArray();

                        _logger.LogDebug("Added {ShowtimesCount} new showtimes. First: {@FirstShowtime}", showtimes.Count(), showtimes.FirstOrDefault());

                        allShowtimes.AddRange(showtimes);
                    }

                    showtimeDate = showtimeDate.AddDays(1);
                    _logger.LogDebug("New showtime date {ShowtimeDate}", showtimeDate);
                } while (dateOfNextTaskRun.Date != showtimeDate);

                _logger.LogDebug("Create {ShowtimesCount} new showtimes", allShowtimes.Count);

                await _movieShowtimeRepository.CreateMovieShowtimesAsync(allShowtimes);

                _logger.LogDebug("Showtimes created, execution completed");
            }
        }
    }
}