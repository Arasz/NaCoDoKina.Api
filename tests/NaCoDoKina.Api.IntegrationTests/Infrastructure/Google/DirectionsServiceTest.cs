using FluentAssertions;
using NaCoDoKina.Api.Infrastructure;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Google.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Google
{
    public class DirectionsServiceTest : GoogleServiceTestBase<IGoogleDirectionsService, DirectionsApiRequest>
    {
        protected override IRequestParser<DirectionsApiRequest> RequestParser
            => new OnlyRequiredDirectionsRequestParser();

        protected override IGoogleDirectionsService CreateServiceUnderTest(GoogleServiceDependencies<DirectionsApiRequest> dependencies)
            => new GoogleDirectionsService(dependencies);

        public class GetDirections : DirectionsServiceTest
        {
            [Fact]
            public async Task Should_return_not_empty_directions_for_car()
            {
                // arrange
                var destination = "52.44056,16.919235";
                var origin = "52.3846579,16.8519869";
                var request = new DirectionsApiRequest(origin, destination);

                //act
                var response = await ServiceUnderTest.GetDirections(request);

                response.Status.ShouldBeEquivalentTo("OK");
                response.ErrorMessage.Should().BeNullOrEmpty();
                response.Routes.Should().NotBeNullOrEmpty();
            }

            [Fact]
            public async Task Should_return_route_with_one_leg_and_duration()
            {
                // arrange
                var destination = "52.44056,16.919235";
                var origin = "52.3846579,16.8519869";
                var request = new DirectionsApiRequest(origin, destination);

                //act
                var response = await ServiceUnderTest.GetDirections(request);
                var leg = response.Routes
                    .Select(route => route.Legs.First())
                    .First();

                response.Status.ShouldBeEquivalentTo("OK");
                response.ErrorMessage.Should().BeNullOrEmpty();
                response.Routes.Should().NotBeNullOrEmpty();
                response.Routes
                    .SelectMany(route => route.Legs)
                    .Count().Should().Be(response.Routes.Count);
                leg.Duration.Value.Should().BeGreaterThan(0);
            }
        }
    }
}