using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response;
using NaCoDoKina.Api.Infrastructure.Google.Exceptions;
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
        protected Mock<IGoogleDirectionsService> DirectionsServiceMock { get; }

        protected Mock<IGoogleGeocodingService> GeocodingServiceMock { get; }

        protected Mock<ILogger> LoggerMock { get; }

        protected Mock<IMapper> MapperMock { get; }

        public LocationServiceTest()
        {
            LoggerMock = new Mock<ILogger>();
            MapperMock = new Mock<IMapper>();
            DirectionsServiceMock = new Mock<IGoogleDirectionsService>();
            GeocodingServiceMock = new Mock<IGoogleGeocodingService>();
            ServiceUnderTest = new LocationService(DirectionsServiceMock.Object, GeocodingServiceMock.Object, MapperMock.Object, LoggerMock.Object);
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

                MapperMock.Setup(mapper => mapper.Map<DirectionsApiRequest>(It.IsAny<TravelPlan>()))
                    .Returns(() => new DirectionsApiRequest(travelPlan.Origin.ToString(), travelPlan.Destination.ToString()));

                DirectionsServiceMock
                    .Setup(service => service.GetDirections(It.IsAny<DirectionsApiRequest>()))
                    .Returns(() => Task.FromResult(apiResponse));

                //act
                var travelTime = await ServiceUnderTest.CalculateTravelTimeAsync(travelPlan);

                //assert

                travelTime.Should().BeGreaterThan(TimeSpan.Zero);
                travelTime.Should().Be(TimeSpan.FromSeconds(duration));
            }

            [Fact]
            public async Task Should_return_min_time_and_log_when_api_returns_error()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination, MeansOfTransport.Car);

                //LoggerMock.Setup(logger => logger.LogError(It.IsAny<string>(), It.IsAny<object[]>()));

                MapperMock.Setup(mapper => mapper.Map<DirectionsApiRequest>(It.IsAny<TravelPlan>()))
                    .Returns(() => new DirectionsApiRequest(travelPlan.Origin.ToString(), travelPlan.Destination.ToString()));

                DirectionsServiceMock
                    .Setup(service => service.GetDirections(It.IsAny<DirectionsApiRequest>()))
                    .Throws(new GoogleApiException("INVALID_REQUEST", String.Empty));

                //act
                var result = await ServiceUnderTest.CalculateTravelTimeAsync(travelPlan);

                //assert
                //LoggerMock.Verify();
                result.Should().Be(TimeSpan.MinValue);
            }

            [Fact]
            public async Task Should_return_min_time_when_something_goes_wrong()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination, MeansOfTransport.Car);

                MapperMock.Setup(mapper => mapper.Map<DirectionsApiRequest>(It.IsAny<TravelPlan>()))
                    .Returns(() => new DirectionsApiRequest(travelPlan.Origin.ToString(), travelPlan.Destination.ToString()));

                DirectionsServiceMock
                    .Setup(service => service.GetDirections(It.IsAny<DirectionsApiRequest>()))
                    .Throws(new GoogleApiException(new Exception()));

                //act
                var result = await ServiceUnderTest.CalculateTravelTimeAsync(travelPlan);

                //assert
                result.Should().Be(TimeSpan.MinValue);
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