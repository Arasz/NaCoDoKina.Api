using Autofac;
using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.Services;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.IntegrationTests.Modules;
using Ploeh.AutoFixture;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Recommendation
{
    public class RecommendationServiceTest
    {
        private Fixture Fixture { get; }

        private IContainer _container;
        private IRecommendationService ServiceUnderTest { get; }

        public RecommendationServiceTest()
        {
            Fixture = new Fixture();

            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyModules<BasicServiceDependenciesModule>(typeof(RecommendationServiceTest).Assembly);

            builder
                .RegisterType<RecommendationSettings>()
                .AsSelf();

            builder
                .RegisterType<RecommendationService>()
                .Named<IRecommendationService>(nameof(RecommendationService))
                .AsImplementedInterfaces();

            builder.RegisterType<RecommendationRequestParser>()
                .AsImplementedInterfaces();

            _container = builder.Build();

            ServiceUnderTest = _container.Resolve<IRecommendationService>();
        }

        public class GetMovieRating : RecommendationServiceTest
        {
            [Fact]
            public async Task Should_return_rating_for_user_and_movie()
            {
                //Arrange
                var userId = 1;
                var movieId = 1;
                var request = new RecommendationApiRequest(userId, movieId);

                //Act
                var result = await ServiceUnderTest.GetMovieRating(request);
                var response = result.Value;

                //Assert
                result.IsSuccess.Should().BeTrue();
                response.MovieId.Should().Be(request.MovieId);
                response.UserId.Should().Be(request.UserId);
                response.Rating.Should().BeGreaterThan(-1);
            }

            [Fact]
            public async Task Should_return_failure_result_when_user_do_not_exist()
            {
                //Arrange
                var request = new RecommendationApiRequest(-9999999999, 1);

                //Act
                var result = await ServiceUnderTest.GetMovieRating(request);
                //Assert
                result.IsSuccess.Should().BeFalse();
            }

            [Fact]
            public async Task Should_return_failure_result_when_movie_do_not_exist()
            {
                //Arrange
                var request = new RecommendationApiRequest(1, -55);

                //Act
                var result = await ServiceUnderTest.GetMovieRating(request);
                //Assert
                result.IsSuccess.Should().BeFalse();
            }
        }

        public class SaveMovieRating : RecommendationServiceTest
        {
            [Fact]
            public async Task Should_save_rating_for_user_and_movie()
            {
                //Arrange
                var request = new RecommendationApiRequest(1, 1);
                var rating = new Rating(4.5);

                //Act
                var result = await ServiceUnderTest.SaveMovieRating(request, rating);

                //Assert
                result.IsSuccess.Should().BeTrue();
            }
        }
    }
}