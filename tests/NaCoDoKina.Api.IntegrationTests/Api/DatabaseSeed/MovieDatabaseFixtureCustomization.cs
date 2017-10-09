using Ploeh.AutoFixture;
using System;
using ApplicationCore.Entities;
using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Entities.Movies;
using ApplicationCore.Entities.Resources;

namespace NaCoDoKina.Api.IntegrationTests.Api.DatabaseSeed
{
    public class MovieDatabaseFixtureCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<MediaLink>(composer =>
                composer.Without(link => link.Id));

            fixture.Customize<ReviewLink>(composer =>
                composer.Without(link => link.Id));

            fixture.Customize<Movie>(composer => composer
                .With(movie => movie.Id, 0));

            fixture.Customize<MovieDetails>(composer => composer
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
                .Without(showtime => showtime.Movie)
                .With(showtime => showtime.ShowTime, DateTime.Now.AddDays(2)));
        }
    }
}