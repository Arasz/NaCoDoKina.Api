using Autofac;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.Services;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.IntegrationTests.Modules;
using Ploeh.AutoFixture;
using System;
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
                .Named<IRecommendationService>(nameof(RecommendationService));

            builder.RegisterType<RecommendationRequestParser>()
                .AsImplementedInterfaces();

            IRecommendationService DecorateService(IComponentContext context, IRecommendationService service)
            {
                var logger = context.Resolve<ILogger<IRecommendationService>>();
                return new RecommendationServiceErrorHandling(service, logger);
            }

            builder
                .RegisterDecorator<IRecommendationService>(DecorateService, fromKey: nameof(RecommendationService));

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
                var response = await ServiceUnderTest.GetMovieRating(request);

                //Assert
                response.MovieId.Should().Be(request.MovieId);
                response.UserId.Should().Be(request.UserId);
                response.Rating.Should().BeGreaterThan(-1);
            }

            [Fact]
            public void Should_throw_rating_not_found_exception_when_movie_data_not_available()
            {
                //Arrange
                var request = new RecommendationApiRequest(-55, 1);

                //Act
                Func<Task> action = () => ServiceUnderTest.GetMovieRating(request);

                //Assert
                action.ShouldThrow<RatingNotFoundException>();
            }
        }

        public class SaveMovieRating : RecommendationServiceTest
        {
            [Fact]
            public void Should_save_rating_for_user_and_movie()
            {
                //Arrange
                var request = new RecommendationApiRequest(1, 1);
                var rating = new Rating(4.5);

                //Act
                Func<Task> action = () => ServiceUnderTest.SaveMovieRating(request, rating);

                //Assert
                action.ShouldNotThrow();
            }
        }
    }
}