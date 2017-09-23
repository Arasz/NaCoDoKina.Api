using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.Services;
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
            ServiceUnderTest = new RatingService(RecommendationServiceMock.Object, UserServiceMock.Object);
        }

        private Task<RecommendationApiResponse> MapRequestToResponse(RecommendationApiRequest request, double expectedRating) => Task.FromResult(new RecommendationApiResponse
        {
            MovieId = request.MovieId,
            UserId = request.UserId,
            Rating = expectedRating,
        });

        public class GetMovieRating : RatingServiceTest
        {
            [Fact]
            public async Task Should_return_rating_for_movie()
            {
                //Arrange
                var movieId = 1;
                var expectedRating = 4.5;
                var userId = 2L;

                UserServiceMock.Setup(service => service.GetCurrentUserIdAsync())
                    .Returns(() => Task.FromResult(userId));

                RecommendationServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<RecommendationApiRequest>()))
                    .Returns(new Func<RecommendationApiRequest, Task<RecommendationApiResponse>>(request => MapRequestToResponse(request, expectedRating)));

                //Act
                var result = await ServiceUnderTest.GetMovieRating(movieId);

                //Assert
                result.Should().Be(expectedRating);
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

                UserServiceMock.Setup(service => service.GetCurrentUserIdAsync())
                    .Returns(() => Task.FromResult(userId));

                RecommendationServiceMock
                    .Setup(service => service.GetMovieRating(It.IsAny<RecommendationApiRequest>()))
                    .Returns(new Func<RecommendationApiRequest, Task<RecommendationApiResponse>>(request => MapRequestToResponse(request, expectedRating)));

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