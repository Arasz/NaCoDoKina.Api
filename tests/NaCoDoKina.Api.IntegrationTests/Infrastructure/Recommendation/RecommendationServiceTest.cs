using Autofac;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Configuration;
using NaCoDoKina.Api.Infrastructure.Recommendation;
using NaCoDoKina.Api.Infrastructure.Recommendation.DataContract;
using NaCoDoKina.Api.Infrastructure.Recommendation.Exceptions;
using NaCoDoKina.Api.Infrastructure.Recommendation.Services;
using NaCoDoKina.Api.IntegrationTests.Modules;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Recommendation
{
    public class RecommendationServiceTest
    {
        private IContainer _container;
        private IRecommendationService ServiceUnderTest { get; }

        public RecommendationServiceTest()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterAssemblyModules<BasicServiceDependenciesModule>(typeof(RecommendationServiceTest).Assembly);

            builder
                .RegisterType<RecommendationApiConfiguration>()
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
                var request = new RecommendationApiRequest(1, 1);

                //Act
                var response = await ServiceUnderTest.GetMovieRating(request);

                //Assert
                response.MovieId.Should().Be(request.MovieId);
                response.UserId.Should().Be(request.UserId);
                response.Rating.Should().BeGreaterThan(-1);
            }

            [Fact]
            public void Should_throw_exception_with_404_code_when_no_data_available()
            {
                //Arrange
                var request = new RecommendationApiRequest(100000000, 1);

                //Act
                Func<Task> action = () => ServiceUnderTest.GetMovieRating(request);

                //Assert
                action.ShouldThrow<RecommendationApiException>()
                    .Which.StatusCode.Should().HaveFlag(HttpStatusCode.NotFound);
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

            [Fact]
            public void Should_throw_exception_with_404_code_when_no_data_available()
            {
                //Arrange
                var request = new RecommendationApiRequest(100000000, 1);
                var rating = new Rating(4.5);

                //Act
                Func<Task> action = () => ServiceUnderTest.SaveMovieRating(request, rating);

                //Assert
                action.ShouldThrow<RecommendationApiException>()
                    .Which.StatusCode.Should().HaveFlag(HttpStatusCode.NotFound);
            }
        }
    }
}