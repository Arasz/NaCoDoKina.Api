using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response;
using NaCoDoKina.Api.Infrastructure.Google.Services;
using NaCoDoKina.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.Services
{
    public class LocationServiceTest : ServiceTestBase<ILocationService>
    {
        protected Mock<IDirectionsService> DirectionsServiceMock { get; }

        protected Mock<IGeocodingService> GeocodingServiceMock { get; }

        public LocationServiceTest()
        {
            DirectionsServiceMock = new Mock<IDirectionsService>();
            GeocodingServiceMock = new Mock<IGeocodingService>();
            ServiceUnderTest = new LocationService(DirectionsServiceMock.Object, GeocodingServiceMock.Object);
        }

        public class CalculateTravelTimeAsync : LocationServiceTest
        {
            [Fact]
            public async Task Should_return_correct_time_for_car_transport()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination, MeansOfTransport.Car);
                var duration = 1000;
                var apiResponse = CreateDirectionsResponseWithDuration(duration);

                DirectionsServiceMock
                    .Setup(service => service.GetDirections(It.IsAny<DirectionsApiRequest>()))
                    .Returns(() => Task.FromResult(apiResponse));

                //act
                var travelTime = await ServiceUnderTest.CalculateTravelTimeAsync(travelPlan);

                //assert

                travelTime.Should().BeGreaterThan(TimeSpan.Zero);
                travelTime.Should().Be(TimeSpan.FromSeconds(duration));
            }
        }

        protected DirectionsApiResponse CreateDirectionsResponseWithDuration(int duration) => new DirectionsApiResponse
        {
            Routes = new List<Route>
            {
                new Route
                {
                    Legs = new List<Leg>
                    {
                        new Leg
                        {
                            Duration = new TextValue
                            {
                                Value = duration
                            }
                        }
                    }
                }
            }
        };
    }
}