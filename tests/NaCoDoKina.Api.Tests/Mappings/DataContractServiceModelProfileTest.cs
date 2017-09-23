using FluentAssertions;
using NaCoDoKina.Api.DataContracts.Movies;
using NaCoDoKina.Api.Mapping.Profiles;
using Ploeh.AutoFixture;
using System.Linq;
using Xunit;

namespace NaCoDoKina.Api.Mappings
{
    public class DataContractServiceModelProfileTest : ProfileTestBase<DataContractServiceModelProfile>
    {
        public class LocationTest : DataContractServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_location_given_data_contract_location()
            {
                //Arrange
                var location = new Location(1, 9);

                //Act
                var result = Mapper.Map<Models.Location>(location);

                //Assert
                result.Should().BeOfType<Models.Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }

            [Fact]
            public void Should_return_data_contract_location_given_model_location()
            {
                //Arrange
                var googleLocation = new Models.Location(1, 3);

                //Act
                var result = Mapper.Map<Location>(googleLocation);

                //Assert
                result.Should().BeOfType<Location>();
                result.Longitude.Should().Be(googleLocation.Longitude);
                result.Latitude.Should().Be(googleLocation.Latitude);
            }
        }

        public class MovieDetailTest : DataContractServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_movie_details_given_data_contract_movie_details()
            {
                //Arrange
                var movieDetails = Fixture.Create<MovieDetails>();

                //Act
                var result = Mapper.Map<Models.MovieDetails>(movieDetails);

                //Assert
                result.Should().BeOfType<Models.MovieDetails>();
                result.Rating.Should().Be(movieDetails.Rating);
                result.AgeLimit.Should().Be(movieDetails.AgeLimit);
                result.Length.Should().Be(movieDetails.Length);
                result.OriginalTitle.Should().Be(movieDetails.OriginalTitle);
                result.DescriptionSites.Should().HaveSameCount(movieDetails.DescriptionSites);
                result.DescriptionSites.Should()
                    .Contain(url => movieDetails.DescriptionSites.First().Name == url.Name);
            }

            [Fact]
            public void Should_return_data_contract_movie_details_given_model_movie_details()
            {
                //Arrange
                var movieDetails = Fixture.Create<Models.MovieDetails>();

                //Act
                var result = Mapper.Map<MovieDetails>(movieDetails);

                //Assert
                result.Should().BeOfType<MovieDetails>();
                result.Rating.Should().Be(movieDetails.Rating);
                result.AgeLimit.Should().Be(movieDetails.AgeLimit);
                result.Length.Should().Be(movieDetails.Length);
                result.OriginalTitle.Should().Be(movieDetails.OriginalTitle);
                result.DescriptionSites.Should().HaveSameCount(movieDetails.DescriptionSites);
                result.DescriptionSites.Should()
                    .Contain(url => movieDetails.DescriptionSites.First().Name == url.Name);
            }
        }

        public class MovieShowtimeTest : DataContractServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_movie_showtime_given_data_contract_movie_showtime()
            {
                //Arrange
                var movieShowtime = Fixture.Create<MovieShowtime>();

                //Act
                var result = Mapper.Map<Models.MovieShowtime>(movieShowtime);

                //Assert
                result.Should().BeOfType<Models.MovieShowtime>();
                result.CinemaName.Should().Be(movieShowtime.CinemaName);
                result.Language.Should().Be(movieShowtime.Language);
                result.ShowType.Should().Be(movieShowtime.ShowType);
                result.ShowTime.Should().Be(movieShowtime.ShowTime);
            }

            [Fact]
            public void Should_return_data_contract_movie_showtime_given_model_movie_showtime()
            {
                //Arrange
                var movieShowtime = Fixture.Create<Models.MovieShowtime>();

                //Act
                var result = Mapper.Map<MovieShowtime>(movieShowtime);

                //Assert
                result.Should().BeOfType<MovieShowtime>();
                result.CinemaName.Should().Be(movieShowtime.CinemaName);
                result.Language.Should().Be(movieShowtime.Language);
                result.ShowType.Should().Be(movieShowtime.ShowType);
                result.ShowTime.Should().Be(movieShowtime.ShowTime);
            }
        }

        public class MovieTest : DataContractServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_movie_given_data_contract_movie()
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
            public void Should_return_data_contract_movie_given_model_movie()
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

        public class ServiceUrlTest : DataContractServiceModelProfileTest
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
            public void Should_return_service_url_cinema_given_model_service_url()
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

        public class CinemaTest : DataContractServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_cinema_given_data_cinema_movie()
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
                result.NetworkName.Should().Be(cinema.NetworkName);
                result.Website.Should()
                    .Match<ServiceUrl>(url => url.Name == cinema.Website.Name)
                    .And
                    .Match<ServiceUrl>(url => url.Url == cinema.Website.Url);
            }

            [Fact]
            public void Should_return_data_contract_cinema_given_model_cinema()
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
                result.Website.Should().NotBeNull();
                result.NetworkName.Should().Be(cinema.NetworkName);

                result.Website.Should()
                    .Match<ServiceUrl>(url => url.Name == cinema.Website.Name)
                    .And
                    .Match<ServiceUrl>(url => url.Url == cinema.Website.Url);
            }
        }
    }
}