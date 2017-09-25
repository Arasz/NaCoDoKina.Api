using NaCoDoKina.Api.Entities;
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
                .Without(details => details.MovieId));
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