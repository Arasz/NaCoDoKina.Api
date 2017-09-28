using FluentAssertions;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Mapping.Profiles;
using Ploeh.AutoFixture;
using System.Linq;
using Xunit;
using Cinema = NaCoDoKina.Api.Entities.Cinemas.Cinema;
using Location = NaCoDoKina.Api.Models.Location;
using Movie = NaCoDoKina.Api.Entities.Movies.Movie;
using MovieDetails = NaCoDoKina.Api.Entities.Movies.MovieDetails;
using MovieShowtime = NaCoDoKina.Api.Entities.Movies.MovieShowtime;

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
                var result = Mapper.Map<Models.Movies.MovieDetails>(movieDetails);

                //Assert
                result.Should().BeOfType<Models.Movies.MovieDetails>();
                result.Rating.Should().Be(0);
                result.AgeLimit.Should().Be(movieDetails.AgeLimit);
                result.Length.Should().Be(movieDetails.Length);
                result.OriginalTitle.Should().Be(movieDetails.OriginalTitle);
                result.MediaResources.Should().HaveSameCount(movieDetails.MediaResources);
                result.MovieReviews.Should().HaveSameCount(movieDetails.MovieReviews);
                result.MovieReviews.Should()
                    .Contain(url => movieDetails.MovieReviews.First().Name == url.Name);
            }

            [Fact]
            public void Should_return_data_model_movie_details_given_model_movie_details()
            {
                //Arrange
                var movieDetails = Fixture.Create<Models.Movies.MovieDetails>();

                //Act
                var result = Mapper.Map<MovieDetails>(movieDetails);

                //Assert
                result.Should().BeOfType<MovieDetails>();
                result.AgeLimit.Should().Be(movieDetails.AgeLimit);
                result.Length.Should().Be(movieDetails.Length);
                result.OriginalTitle.Should().Be(movieDetails.OriginalTitle);
                result.MediaResources.Should().HaveSameCount(movieDetails.MediaResources);
                result.MovieReviews.Should().HaveSameCount(movieDetails.MovieReviews);
                result.MovieReviews.Should()
                    .Contain(url => movieDetails.MovieReviews.First().Name == url.Name);
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
                var result = Mapper.Map<Models.Movies.MovieShowtime>(movieShowtime);

                //Assert
                result.Should().BeOfType<Models.Movies.MovieShowtime>();

                result.Language.Should().Be(movieShowtime.Language);
                result.ShowType.Should().Be(movieShowtime.ShowType);
            }

            [Fact]
            public void Should_return_data_model_movie_showtime_given_model_movie_showtime()
            {
                //Arrange
                var movieShowtime = Fixture.Create<Models.Movies.MovieShowtime>();

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
                var result = Mapper.Map<Models.Movies.Movie>(movie);

                //Assert
                result.Should().BeOfType<Models.Movies.Movie>();
                result.Title.Should().Be(movie.Title);
                result.Id.Should().Be(movie.Id);
                result.PosterUrl.Should().Be(movie.PosterUrl);
            }

            [Fact]
            public void Should_return_data_model_movie_given_model_movie()
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
                result.Logo.Url.Should().Be(reviewLink.Logo.Url);
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
                result.Url.Should().Be(reviewLink.Url);
                result.Logo.Url.Should().Be(reviewLink.Logo.Url);
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
                result.MediaType.Should().HaveFlag(Mapper.Map<MediaType>(mediaLink.MediaType));
            }
        }

        public class ResourceLinkTest : DataModelServiceModelProfileTest
        {
            [Fact]
            public void Should_return_model_resource_link_given_data_resource_link()
            {
                //Arrange
                var resourceLink = Fixture.Create<ResourceLink>();

                //Act
                var result = Mapper.Map<Models.Resources.ResourceLink>(resourceLink);

                //Assert
                result.Should().BeOfType<Models.Resources.ResourceLink>();
                result.Url.Should().Be(resourceLink.Url);
            }

            [Fact]
            public void Should_return_data_model_resource_link_given_model_resource_link()
            {
                //Arrange
                var resourceLink = Fixture.Create<Models.Resources.ResourceLink>();

                //Act
                var result = Mapper.Map<ResourceLink>(resourceLink);

                //Assert
                result.Should().BeOfType<ResourceLink>();
                result.Url.Should().Be(resourceLink.Url);
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
                var result = Mapper.Map<Models.Cinemas.Cinema>(cinema);

                //Assert
                result.Should().BeOfType<Models.Cinemas.Cinema>();
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
                var cinema = Fixture.Create<Models.Cinemas.Cinema>();

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