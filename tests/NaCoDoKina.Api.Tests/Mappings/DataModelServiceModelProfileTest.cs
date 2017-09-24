using FluentAssertions;
using NaCoDoKina.Api.Mapping.Profiles;
using NaCoDoKina.Api.Models;
using Ploeh.AutoFixture;
using System.Linq;
using Xunit;
using Cinema = NaCoDoKina.Api.Entities.Cinema;
using Movie = NaCoDoKina.Api.Entities.Movie;
using MovieDetails = NaCoDoKina.Api.Entities.MovieDetails;
using MovieShowtime = NaCoDoKina.Api.Entities.MovieShowtime;
using ServiceUrl = NaCoDoKina.Api.Entities.ServiceUrl;

namespace NaCoDoKina.Api.Mappings
{
    public class DataModelServiceModelProfileTest : ProfileTestBase<DataModelServiceModelProfile>
    {
        public class LocationTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_entity_location_given_model_location()
            {
                //Arrange
                var location = Fixture.Create<Location>();

                //Act
                var result = Mapper.Map<Entities.Location>(location);

                //Assert
                result.Should().BeOfType<Entities.Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }

            [Fact]
            public void Should_return_model_location_given_entity_location()
            {
                //Arrange
                var location = new Entities.Location(1, 9);

                //Act
                var result = Mapper.Map<Location>(location);

                //Assert
                result.Should().BeOfType<Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }
        }

        public class MovieDetailTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_movie_details_given_data_model_movie_details()
            {
                //Arrange
                var movieDetails = Fixture.Create<MovieDetails>();

                //Act
                var result = Mapper.Map<Models.MovieDetails>(movieDetails);

                //Assert
                result.Should().BeOfType<Models.MovieDetails>();
                result.Rating.Should().Be(0);
                result.AgeLimit.Should().Be(movieDetails.AgeLimit);
                result.Length.Should().Be(movieDetails.Length);
                result.OriginalTitle.Should().Be(movieDetails.OriginalTitle);
                result.DescriptionSites.Should().HaveSameCount(movieDetails.DescriptionSites);
                result.DescriptionSites.Should()
                    .Contain(url => movieDetails.DescriptionSites.First().Name == url.Name);
            }

            [Fact]
            public void Should_return_data_model_movie_details_given_model_movie_details()
            {
                //Arrange
                var movieDetails = Fixture.Create<Models.MovieDetails>();

                //Act
                var result = Mapper.Map<MovieDetails>(movieDetails);

                //Assert
                result.Should().BeOfType<MovieDetails>();
                result.AgeLimit.Should().Be(movieDetails.AgeLimit);
                result.Length.Should().Be(movieDetails.Length);
                result.OriginalTitle.Should().Be(movieDetails.OriginalTitle);
                result.DescriptionSites.Should().HaveSameCount(movieDetails.DescriptionSites);
                result.DescriptionSites.Should()
                    .Contain(url => movieDetails.DescriptionSites.First().Name == url.Name);
            }
        }

        public class MovieShowtimeTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_movie_showtime_given_data_model_movie_showtime()
            {
                //Arrange
                var movieShowtime = Fixture.Create<MovieShowtime>();

                //Act
                var result = Mapper.Map<Models.MovieShowtime>(movieShowtime);

                //Assert
                result.Should().BeOfType<Models.MovieShowtime>();
                result.CinemaName.Should().Be(movieShowtime.Cinema.Name);
                result.Language.Should().Be(movieShowtime.Language);
                result.ShowType.Should().Be(movieShowtime.ShowType);
            }

            [Fact]
            public void Should_return_data_model_movie_showtime_given_model_movie_showtime()
            {
                //Arrange
                var movieShowtime = Fixture.Create<Models.MovieShowtime>();

                //Act
                var result = Mapper.Map<MovieShowtime>(movieShowtime);

                //Assert
                result.Should().BeOfType<MovieShowtime>();
                //result.CinemaName.Should().Be(movieShowtime.CinemaName);
                result.Language.Should().Be(movieShowtime.Language);
                result.ShowType.Should().Be(movieShowtime.ShowType);
                //result.ShowTimes.Should().Contain(movieShowtime.ShowTimes);
            }
        }

        public class MovieTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_movie_given_data_model_movie()
            {
                //Arrange
                var movie = Fixture.Create<Movie>();

                //Act
                var result = Mapper.Map<Models.Movie>(movie);

                //Assert
                result.Should().BeOfType<Models.Movie>();
                result.Name.Should().Be(movie.Name);
                result.Id.Should().Be(movie.Id);
                result.PosterUrl.Should().Be(movie.PosterUrl);
            }

            [Fact]
            public void Should_return_data_model_movie_given_model_movie()
            {
                //Arrange
                var movie = Fixture.Create<Models.Movie>();

                //Act
                var result = Mapper.Map<Movie>(movie);

                //Assert
                result.Should().BeOfType<Movie>();
                result.Name.Should().Be(movie.Name);
                result.Id.Should().Be(movie.Id);
                result.PosterUrl.Should().Be(movie.PosterUrl);
            }
        }

        public class ServiceUrlTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_service_url_given_data_service_url()
            {
                //Arrange
                var serviceUrl = Fixture.Create<ServiceUrl>();

                //Act
                var result = Mapper.Map<Models.ServiceUrl>(serviceUrl);

                //Assert
                result.Should().BeOfType<Models.ServiceUrl>();
                result.Name.Should().Be(serviceUrl.Name);
                result.Url.Should().Be(serviceUrl.Url);
            }

            [Fact]
            public void Should_return_data_model_service_url_given_model_service_url()
            {
                //Arrange
                var serviceUrl = Fixture.Create<Models.ServiceUrl>();

                //Act
                var result = Mapper.Map<ServiceUrl>(serviceUrl);

                //Assert
                result.Should().BeOfType<ServiceUrl>();
                result.Name.Should().Be(serviceUrl.Name);
                result.Url.Should().Be(serviceUrl.Url);
            }
        }

        public class CinemaTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_cinema_given_data_model_cinema()
            {
                //Arrange
                var cinema = Fixture.Create<Cinema>();

                //Act
                var result = Mapper.Map<Models.Cinema>(cinema);

                //Assert
                result.Should().BeOfType<Models.Cinema>();
                result.Name.Should().Be(cinema.Name);
                result.Address.Should().Be(cinema.Address);
                result.Id.Should().Be(cinema.Id);
                result.Website.Should().NotBeNull();
                //result.NetworkName.Should().Be(cinema.NetworkName);
                //result.Website.Should()
                //    .Match<ServiceUrl>(url => url.Name == cinema.Website.Name)
                //    .And
                //    .Match<ServiceUrl>(url => url.Website == cinema.Website.Website);
            }

            [Fact]
            public void Should_return_data_model_cinema_given_model_cinema()
            {
                //Arrange
                var cinema = Fixture.Create<Models.Cinema>();

                //Act
                var result = Mapper.Map<Cinema>(cinema);

                //Assert
                result.Should().BeOfType<Cinema>();
                result.Name.Should().Be(cinema.Name);
                result.Address.Should().Be(cinema.Address);
                result.Id.Should().Be(cinema.Id);
            }
        }
    }
}