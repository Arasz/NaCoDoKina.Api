using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.Services;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.Results;
using Ploeh.AutoFixture;
using System;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class RatingServiceTest : ServiceTestBase<IRatingService>
    {
        protected Mock<IRecommendationService> RecommendationServiceMock { get; }

        protected Mock<IUserService> UserServiceMock { get; }

        public RatingServiceTest()
        {
            UserServiceMock = Mock.Mock<IUserService>();
            RecommendationServiceMock = Mock.Mock<IRecommendationService>();

            Mock.Provide(new RatingSettings { UnratedMovieRating = 6 });
            ServiceUnderTest = Mock.Create<RatingService>();
        }

        private Task<Result<RecommendationApiResponse>> MapRequestToResponse(RecommendationApiRequest request, double expectedRating) => Task.FromResult(Result.Success(new RecommendationApiResponse
        {
            MovieId = request.MovieId,
            UserId = request.UserId,
            Rating = expectedRating,
        }));

        public class GetMovieRating : RatingServiceTest
        {
            [Fact]
            public async Task Should_return_movie_rating_when_rating_exist_for_current_user()
            {
                //Arrange
                var movieId = Fixture.Create<long>();
                var expectedRating = Fixture.Create<double>();
                var userId = Fixture.Create<long>();

                var response = Fixture.Build<RecommendationApiResponse>()
                    .With(apiResponse => apiResponse.Rating, expectedRating)
                    .Create();

                UserServiceMock.Setup(service => service.GetCurrentUserId())
                    .Returns(() => userId);

                RecommendationServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<RecommendationApiRequest>()))
                    .ReturnsAsync(Result.Success(response));

                //Act
                var result = await ServiceUnderTest.GetMovieRating(movieId);

                //Assert
                result.Should().Be(expectedRating);
            }

            [Fact]
            public async Task Should_return_default_rating_when_could_not_get_rating_for_movie()
            {
                //Arrange
                var movieId = Fixture.Create<long>();
                var expectedRating = Fixture.Create<double>();
                var userId = Fixture.Create<long>();

                UserServiceMock.Setup(service => service.GetCurrentUserId())
                    .Returns(() => userId);

                var settings = Mock.Create<RatingSettings>();

                RecommendationServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<RecommendationApiRequest>()))
                    .ReturnsAsync(Result.Failure<RecommendationApiResponse>());

                //Act
                var result = await ServiceUnderTest.GetMovieRating(movieId);

                //Assert
                result.Should().Be(settings.UnratedMovieRating);
            }
        }

        public class SetMovieRating : RatingServiceTest
        {
            [Fact]
            public async Task Should_set_rating_for_movie()
            {
                //Arrange
                var movieId = 1;
                var expectedRating = 4.5;
                var userId = 2L;

                UserServiceMock.Setup(service => service.GetCurrentUserId())
                    .Returns(() => userId);

                RecommendationServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<RecommendationApiRequest>()))
                    .Returns(new Func<RecommendationApiRequest, Task<Result<RecommendationApiResponse>>>(request => MapRequestToResponse(request, expectedRating)));

                //Act
                Func<Task> action = () => ServiceUnderTest.SetMovieRating(movieId, expectedRating);
                var rating = await ServiceUnderTest.GetMovieRating(movieId);

                //Assert
                action.ShouldNotThrow();
                rating.Should().Be(expectedRating);
            }
        }
    }
}