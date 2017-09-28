﻿using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Entities.Movies;
using Ploeh.AutoFixture;

namespace NaCoDoKina.Api.IntegrationTests.Api.DatabaseSeed
{
    public class MovieDatabaseFixtureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<Movie>(composer => composer
                .Without(movie => movie.Id));

            fixture.Customize<MovieDetails>(composer => composer
                .Without(details => details.Id)
                .Without(details => details.Id));
            fixture.Customize<Cinema>(composer => composer
                .Without(cinema => cinema.Id));

            fixture.Customize<CinemaNetwork>(composer => composer
                .Without(cinemaNetwork => cinemaNetwork.Id));

            fixture.Customize<Location>(composer => composer
                .With(location => location.Longitude, 52.44056)
                .With(location => location.Latitude, 16.919235));

            fixture.Customize<MovieShowtime>(composer => composer
                .Without(showtime => showtime.Id)
                .Without(showtime => showtime.Cinema)
                .Without(showtime => showtime.Movie));
        }
    }
}