using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using ApplicationCore.Results;
using FluentAssertions;
using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.CinemaCity.Showtimes.BuildSteps;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Context;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Requests;
using Infrastructure.DataProviders.Client;
using Infrastructure.DataProviders.Requests;
using Infrastructure.Services;
using Infrastructure.Settings.CinemaNetwork;
using Moq;
using Ploeh.AutoFixture;
using System.Threading.Tasks;
using TestsCore;
using Xunit;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.BuildSteps
{
    public class GetWebApiShowtimeBuildStepTest : UnitTestBase
    {
        private class WebClientFake : IWebClient
        {
            private readonly Result<string> _result;

            public WebClientFake(Result<string> result)
            {
                _result = result;
            }

            public async Task<Result<string>> MakeRequestAsync(IParsableRequestData requestData, params IRequestParameter[] requestParameters)
            {
                return _result;
            }
        }

        public class BuildManyAsync : GetWebApiShowtimeBuildStepTest
        {
            [Fact]
            public async Task Should_return_showtimes_from_web_api()
            {
                // Arrange
                var requestResult = Result.Success(Fixture.Create<string>());

                Mock.Provide<IWebClient>(new WebClientFake(requestResult));

                Mock.Provide(Fixture.Create<CinemaNetworksSettings>());

                var requestData = Mock.Create<GetMoviesPlayedInCinemaRequestData>();

                Mock.Provide(requestData);

                var requestResultParsed = Fixture.Build<CinemaCityResponse<GetWebApiShowtimeBuildStep.Body>>()
                    .Create();

                Mock.Mock<ISerializationService>()
                    .Setup(service => service.Deserialize<CinemaCityResponse<GetWebApiShowtimeBuildStep.Body>>(requestResult.Value))
                    .Returns(requestResultParsed);

                var movie = Fixture.Build<Movie>()
                    .Create();

                Mock.Mock<IMovieRepository>()
                    .Setup(repository => repository.GetMovieByTitleAsync(It.IsAny<string>()))
                    .ReturnsAsync(movie);

                var context = Fixture.Create<MovieShowtimesContext>();

                // Act

                var step = Mock.Create<GetWebApiShowtimeBuildStep>();
                var result = await step.BuildManyAsync(new MovieShowtime[0], context);

                // Assert
                result.IsSuccess
                    .Should()
                    .BeTrue();
            }
        }
    }
}