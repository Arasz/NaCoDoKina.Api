using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Data;
using NaCoDoKina.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repository
{
    public class ApplicationContextSeed
    {
        private readonly ILogger<ApplicationContextSeed> _logger;

        public ApplicationContextSeed(ILogger<ApplicationContextSeed> logger)
        {
            _logger = logger;
        }

        public async Task SeedAsync(ApplicationContext applicationContext, bool migration = true)
        {
            if (migration)
                await applicationContext.Database.MigrateAsync();

            try
            {
                await InitializeDataAsync(applicationContext);
            }
            catch (Exception e)
            {
                _logger.LogError("Error during database seed: @e", e);
            }
        }

        public async Task InitializeDataAsync(ApplicationContext applicationContext)
        {
            var cinemaNetworks = new List<CinemaNetwork>
            {
                new CinemaNetwork {Name = "A"},
                new CinemaNetwork {Name = "B"}
            };

            var cinemas = new List<Cinema>
            {
                new Cinema
                {
                    Name = "A",
                    Address = "A",
                    Location = new Location(2, 2),
                    CinemaNetwork = cinemaNetworks[0],
                },
                new Cinema
                {
                    Name = "B",
                    Address = "B",
                    Location = new Location(2, 2),
                    CinemaNetwork = cinemaNetworks[0],
                },
                new Cinema
                {
                    Name = "C",
                    Address = "C",
                    Location = new Location(2, 2),
                    CinemaNetwork = cinemaNetworks[1],
                }
            };

            cinemaNetworks[0].Cinemas = cinemas.Take(2);
            cinemaNetworks[1].Cinemas = cinemas.TakeLast(1);

            var movieShowtimes = new List<MovieShowtime>
            {
                new MovieShowtime
                {
                    DateTime = DateTime.Now.Add(TimeSpan.FromDays(3)),
                    Cinema = cinemas[0],
                },
                new MovieShowtime
                {
                    DateTime = DateTime.Now.Add(TimeSpan.FromDays(1)),
                    Cinema = cinemas[0],
                },
                new MovieShowtime
                {
                    DateTime = DateTime.Now.Add(TimeSpan.FromDays(5)),
                    Cinema = cinemas[1],
                },
                new MovieShowtime
                {
                    DateTime = DateTime.Now.Add(TimeSpan.FromHours(3)),
                    Cinema = cinemas[2],
                }
            };

            var movies = new List<Movie>
            {
                new Movie
                {
                    Title = "A",
                    Details = new MovieDetails
                    {
                        AgeLimit = "10",
                        Description = "blblabla",
                        Language = "Polish"
                    },
                    MovieShowtimes = movieShowtimes.Take(2),
                },

                new Movie
                {
                    Title = "B",
                    Details = new MovieDetails
                    {
                        AgeLimit = "12",
                        Description = "blblabla",
                        Language = "English"
                    },
                    MovieShowtimes = movieShowtimes.TakeLast(2),
                },
            };

            movieShowtimes[0].Movie = movies[0];
            movieShowtimes[1].Movie = movies[0];
            movieShowtimes[2].Movie = movies[1];
            movieShowtimes[3].Movie = movies[1];

            applicationContext.Movies.AddRange(movies);
            applicationContext.MovieShowtimes.AddRange(movieShowtimes);
            applicationContext.Cinemas.AddRange(cinemas);
            applicationContext.CinemaNetworks.AddRange(cinemaNetworks);

            await applicationContext.SaveChangesAsync();
        }
    }
}