using FluentAssertions;
using NaCoDoKina.Api.DataContracts;
using NaCoDoKina.Api.DataContracts.Cinemas;
using NaCoDoKina.Api.DataContracts.Movies;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Mapping.Profiles;
using Ploeh.AutoFixture;
using System.Linq;
using Xunit;
using MediaLink = NaCoDoKina.Api.Entities.Resources.MediaLink;

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
                var result = Mapper.Map<Models.Movies.MovieDetails>(movieDetails);

                //Assert
                result.Should().BeOfType<Models.Movies.MovieDetails>();
                result.Rating.Should().Be(movieDetails.Rating);
                result.AgeLimit.Should().Be(movieDetails.AgeLimit);
                result.Length.Should().Be(movieDetails.Length);
                result.OriginalTitle.Should().Be(movieDetails.OriginalTitle);
                result.MediaResources.Should().HaveSameCount(movieDetails.MediaResources);
                result.MovieReviews.Should().HaveSameCount(movieDetails.MovieReviews);
                result.MovieReviews.Should()
                    .Contain(url => movieDetails.MovieReviews.First().Name == url.Name);
            }

            [Fact]
            public void Should_return_data_contract_movie_details_given_model_movie_details()
            {
                //Arrange
                var movieDetails = Fixture.Create<Models.Movies.MovieDetails>();

                //Act
                var result = Mapper.Map<MovieDetails>(movieDetails);

                //Assert
                result.Should().BeOfType<MovieDetails>();
                result.Rating.Should().Be(movieDetails.Rating);
                result.AgeLimit.Should().Be(movieDetails.AgeLimit);
                result.Length.Should().Be(movieDetails.Length);
                result.OriginalTitle.Should().Be(movieDetails.OriginalTitle);
                result.MediaResources.Should().HaveSameCount(movieDetails.MediaResources);
                result.MovieReviews.Should().HaveSameCount(movieDetails.MovieReviews);
                result.MovieReviews.Should()
                    .Contain(url => movieDetails.MovieReviews.First().Name == url.Name);
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
                var result = Mapper.Map<Models.Movies.MovieShowtime>(movieShowtime);

                //Assert
                result.Should().BeOfType<Models.Movies.MovieShowtime>();

                result.Language.Should().Be(movieShowtime.Language);
                result.ShowType.Should().Be(movieShowtime.ShowType);
                result.ShowTime.Should().Be(movieShowtime.ShowTime);
            }

            [Fact]
            public void Should_return_data_contract_movie_showtime_given_model_movie_showtime()
            {
                //Arrange
                var movieShowtime = Fixture.Create<Models.Movies.MovieShowtime>();

                //Act
                var result = Mapper.Map<MovieShowtime>(movieShowtime);

                //Assert
                result.Should().BeOfType<MovieShowtime>();

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
                var result = Mapper.Map<Models.Movies.Movie>(movie);

                //Assert
                result.Should().BeOfType<Models.Movies.Movie>();
                result.Title.Should().Be(movie.Title);
                result.Id.Should().Be(movie.Id);
                result.PosterUrl.Should().Be(movie.PosterUrl);
            }

            [Fact]
            public void Should_return_data_contract_movie_given_model_movie()
            {
                //Arrange
                var movie = Fixture.Create<Models.Movies.Movie>();

                //Act
                var result = Mapper.Map<Movie>(movie);

                //Assert
                result.Should().BeOfType<Movie>();
                result.Title.Should().Be(movie.Title);
                result.Id.Should().Be(movie.Id);
                result.PosterUrl.Should().Be(movie.PosterUrl);
            }
        }

        public class ReviewLinkTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_review_link_given_data_review_link()
            {
                //Arrange
                var reviewLink = Fixture.Create<ReviewLink>();

                //Act
                var result = Mapper.Map<Models.Resources.ReviewLink>(reviewLink);

                //Assert
                result.Should().BeOfType<Models.Resources.ReviewLink>();
                result.Url.Should().Be(reviewLink.Url);
                result.LogoUrl.Should().Be(reviewLink.LogoUrl);
                result.Rating.Should().Be(reviewLink.Rating);
            }

            [Fact]
            public void Should_return_data_model_review_link_given_model_review_link()
            {
                //Arrange
                var reviewLink = Fixture.Create<Models.Resources.ReviewLink>();

                //Act
                var result = Mapper.Map<ReviewLink>(reviewLink);

                //Assert
                result.Should().BeOfType<ReviewLink>();
                result.Url.Should().Be(reviewLink.Url);
                result.LogoUrl.Should().Be(reviewLink.LogoUrl);
                result.Rating.Should().Be(reviewLink.Rating);
            }
        }

        public class MediaLinkTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_media_link_given_data_media_link()
            {
                //Arrange
                var mediaLink = Fixture.Create<MediaLink>();

                //Act
                var result = Mapper.Map<Models.Resources.MediaLink>(mediaLink);

                //Assert
                result.Should().BeOfType<Models.Resources.MediaLink>();
                result.Url.Should().Be(mediaLink.Url);
                result.MediaType.Should()
                    .HaveFlag(Mapper.Map<Models.Resources.MediaType>(mediaLink.MediaType));
            }

            [Fact]
            public void Should_return_data_model_resource_link_given_model_resource_link()
            {
                //Arrange
                var mediaLink = Fixture.Create<Models.Resources.MediaLink>();

                //Act
                var result = Mapper.Map<MediaLink>(mediaLink);

                //Assert
                result.Should().BeOfType<MediaLink>();
                result.Url.Should().Be(mediaLink.Url);
                result.MediaType.Should()
                    .HaveFlag(Mapper.Map<MediaType>(mediaLink.MediaType));
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
                var result = Mapper.Map<Models.Cinemas.Cinema>(cinema);

                //Assert
                result.Should().BeOfType<Models.Cinemas.Cinema>();
                result.Name.Should().Be(cinema.Name);
                result.Address.Should().Be(cinema.Address);
                result.Id.Should().Be(cinema.Id);
                result.CinemaUrl.Should()
                    .NotBeNullOrEmpty()
                    .And
                    .Be(cinema.CinemaUrl);
                result.NetworkName.Should().Be(cinema.NetworkName);
            }

            [Fact]
            public void Should_return_data_contract_cinema_given_model_cinema()
            {
                //Arrange
                var cinema = Fixture.Create<Models.Cinemas.Cinema>();

                //Act
                var result = Mapper.Map<Cinema>(cinema);

                //Assert
                result.Should().BeOfType<Cinema>();
                result.Name.Should().Be(cinema.Name);
                result.Address.Should().Be(cinema.Address);
                result.Id.Should().Be(cinema.Id);
                result.CinemaUrl.Should()
                    .NotBeNullOrEmpty()
                    .And
                    .Be(cinema.CinemaUrl);
                result.NetworkName.Should().Be(cinema.NetworkName);
            }
        }
    }
}