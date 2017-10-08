using NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Context;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.DataProviders.Tasks;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Settings.Tasks;
using NaCoDoKina.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Tasks
{
    public class CinemaCityShowtimesTask : TaskBase
    {
        private readonly IMovieShowtimeRepository _movieShowtimeRepository;
        private readonly IEntitiesBuilder<MovieShowtime, MovieShowtimesContext> _entitiesBuilder;
        private readonly ICinemaRepository _cinemaRepository;

        public CinemaCityShowtimesTask(ICinemaRepository cinemaRepository, IMovieShowtimeRepository movieShowtimeRepository, IEntitiesBuilder<MovieShowtime, MovieShowtimesContext> entitiesBuilder, TasksSettings settings)
            : base(settings)
        {
            _movieShowtimeRepository = movieShowtimeRepository ?? throw new ArgumentNullException(nameof(movieShowtimeRepository));
            _entitiesBuilder = entitiesBuilder ?? throw new ArgumentNullException(nameof(entitiesBuilder));
            _cinemaRepository = cinemaRepository ?? throw new ArgumentNullException(nameof(cinemaRepository));
        }

        public override async Task Execute()
        {
            var cinemas = await _cinemaRepository.GetAllCinemas();

            var allShowtimes = new List<MovieShowtime>();

            var dateOfNextTaskRun = Settings.ShowtimesTask.NextOccurrence();

            var showtimeDate = DateTime.Today;

            do
            {
                foreach (var cinema in cinemas)
                {
                    var context = new MovieShowtimesContext(cinema, showtimeDate);
                    var showtimes = await _entitiesBuilder
                        .BuildMany(parameters: context);
                    allShowtimes.AddRange(showtimes);
                }
                showtimeDate = showtimeDate.AddDays(1);
            } while (dateOfNextTaskRun.Date != showtimeDate);

            await _movieShowtimeRepository.CreateMovieShowtimesAsync(allShowtimes);
        }
    }
}