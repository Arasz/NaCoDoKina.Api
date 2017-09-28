using FluentAssertions;
using Moq;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Directions.Response;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Geocoding.Request;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Geocoding.Response;
using NaCoDoKina.Api.Infrastructure.Services.Google.Exceptions;
using NaCoDoKina.Api.Infrastructure.Services.Google.Services;
using NaCoDoKina.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NaCoDoKina.Api.Models.Travel;
using Xunit;
using Location = NaCoDoKina.Api.Models.Location;

namespace NaCoDoKina.Api.Services
{
    public class TravelServiceTest : ServiceTestBase<ITravelService>
    {
        protected Mock<IGoogleDirectionsService> DirectionsServiceMock { get; }

        protected Mock<IGoogleGeocodingService> GeocodingServiceMock { get; }

        public TravelServiceTest()
        {
            DirectionsServiceMock = Mock.Mock<IGoogleDirectionsService>();
            GeocodingServiceMock = Mock.Mock<IGoogleGeocodingService>();
            ServiceUnderTest = new TravelService(DirectionsServiceMock.Object, GeocodingServiceMock.Object, MapperMock.Object, LoggerMock.Object);
        }

        public class TranslateAddressToLocationAsync : TravelServiceTest
        {
            private GeocodingApiResult CreateResultWithLocation(double longitude, double latitude) => new GeocodingApiResult
            {
                Geometry = new Geometry
                {
                    Location = new Infrastructure.Services.Google.DataContract.Common.Location(longitude, latitude)
                }
            };

            [Fact]
            public async Task Should_return_correct_location_when_given_correct_address()
            {
                //arrange
                var testAddress = "Poronińska 3, 60-472 Poznań-Jeżyce, Polska";
                var expectedLocation = new Location(52.4531839, 16.882369);
                var apiResponse = new GeocodingApiResponse
                {
                    Results = new List<GeocodingApiResult>
                    {
                        CreateResultWithLocation(expectedLocation.Longitude, expectedLocation.Latitude)
                    }
                };

                MapperMock.Setup(mapper => mapper.Map<GeocodingApiRequest>(testAddress))
                    .Returns(() => new GeocodingApiRequest(testAddress));

                MapperMock.Setup(mapper => mapper.Map<Location>(It.IsAny<object>()))
                    .Returns(() => expectedLocation);

                GeocodingServiceMock
                    .Setup(service => service.GeocodeAsync(It.IsAny<GeocodingApiRequest>()))
                    .Returns(() => Task.FromResult(apiResponse));

                //act
                var location = await ServiceUnderTest.TranslateAddressToLocationAsync(testAddress);

                //assert

                location.Latitude.Should().Be(expectedLocation.Latitude);
                location.Longitude.Should().Be(expectedLocation.Longitude);
            }

            [Fact]
            public async Task Should_return_null_when_api_returns_error()
            {
                //arrange
                var testAddress = "-1-1-1";
                var expectedLocation = new Location(52.4531839, 16.882369);

                MapperMock.Setup(mapper => mapper.Map<GeocodingApiRequest>(testAddress))
                    .Returns(() => new GeocodingApiRequest(testAddress));

                MapperMock.Setup(mapper => mapper.Map<Location>(It.IsAny<object>()))
                    .Returns(() => expectedLocation);

                GeocodingServiceMock
                    .Setup(service => service.GeocodeAsync(It.IsAny<GeocodingApiRequest>()))
                    .Throws(new GoogleApiException("INVALID_REQUEST", String.Empty));

                //act
                var location = await ServiceUnderTest.TranslateAddressToLocationAsync(testAddress);

                //assert
                location.Should().BeNull();
            }

            [Fact]
            public async Task Should_return_null_when_soething_api_nonreleated_goes_wrong()
            {
                //arrange
                var testAddress = "-1-1-1";
                var expectedLocation = new Location(52.4531839, 16.882369);

                MapperMock.Setup(mapper => mapper.Map<GeocodingApiRequest>(testAddress))
                    .Returns(() => new GeocodingApiRequest(testAddress));

                MapperMock.Setup(mapper => mapper.Map<Location>(It.IsAny<object>()))
                    .Returns(() => expectedLocation);

                GeocodingServiceMock
                    .Setup(service => service.GeocodeAsync(It.IsAny<GeocodingApiRequest>()))
                    .Throws(new GoogleApiException(new Exception()));

                //act
                var location = await ServiceUnderTest.TranslateAddressToLocationAsync(testAddress);

                //assert
                location.Should().BeNull();
            }
        }

        public class CalculateInformationForTravelAsync : TravelServiceTest
        {
            [Fact]
            public async Task Should_return_correct_travel_information()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination);
                var duration = 1000;
                var distance = 1000;
                var apiResponse = new DirectionsApiResponse
                {
                    Routes = new List<Route> { CreateRouteWithOneLeg(duration, distance) }
                };

                MapperMock.Setup(mapper => mapper.Map<DirectionsApiRequest>(It.IsAny<TravelPlan>()))
                    .Returns(() => new DirectionsApiRequest(travelPlan.Origin.ToString(), travelPlan.Destination.ToString()));

                DirectionsServiceMock
                    .Setup(service => service.GetDirections(It.IsAny<DirectionsApiRequest>()))
                    .Returns(() => Task.FromResult(apiResponse));

                //act
                var travelInformation = await ServiceUnderTest.CalculateInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.TravelPlan.Should().Be(travelPlan);
                travelInformation.Duration.Should().Be(TimeSpan.FromSeconds(duration));
                travelInformation.Distance.Should().Be(distance);
            }

            [Fact]
            public async Task Should_return_longest_route_travel_information_when_returns_multiple_routes()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination);
                var maxDuration = 1000;
                var distance = 1000f;
                var apiResponse = new DirectionsApiResponse
                {
                    Routes = new List<Route>
                    {
                        CreateRouteWithOneLeg(maxDuration/10, distance/2),
                        CreateRouteWithOneLeg(maxDuration/2, distance/2),
                        CreateRouteWithOneLeg(maxDuration, distance/2),
                        CreateRouteWithOneLeg(maxDuration, distance)
                    }
                };

                MapperMock.Setup(mapper => mapper.Map<DirectionsApiRequest>(It.IsAny<TravelPlan>()))
                    .Returns(() => new DirectionsApiRequest(travelPlan.Origin.ToString(), travelPlan.Destination.ToString()));

                DirectionsServiceMock
                    .Setup(service => service.GetDirections(It.IsAny<DirectionsApiRequest>()))
                    .Returns(() => Task.FromResult(apiResponse));

                //act
                var travelInformation = await ServiceUnderTest.CalculateInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.TravelPlan.Should().Be(travelPlan);
                travelInformation.Distance.Should().Be(distance);
                travelInformation.Duration.Should().Be(TimeSpan.FromSeconds(maxDuration));
            }

            [Fact]
            public async Task Should_return_min_time_and_log_when_api_returns_error()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination);

                //LoggerMock.Setup(logger => logger.LogError(It.IsAny<string>(), It.IsAny<object[]>()));

                MapperMock.Setup(mapper => mapper.Map<DirectionsApiRequest>(It.IsAny<TravelPlan>()))
                    .Returns(() => new DirectionsApiRequest(travelPlan.Origin.ToString(), travelPlan.Destination.ToString()));

                DirectionsServiceMock
                    .Setup(service => service.GetDirections(It.IsAny<DirectionsApiRequest>()))
                    .Throws(new GoogleApiException("INVALID_REQUEST", String.Empty));

                //act
                var travelInformation = await ServiceUnderTest.CalculateInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.Should().BeNull();
            }

            [Fact]
            public async Task Should_return_min_time_when_something_goes_wrong()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination);

                MapperMock.Setup(mapper => mapper.Map<DirectionsApiRequest>(It.IsAny<TravelPlan>()))
                    .Returns(() => new DirectionsApiRequest(travelPlan.Origin.ToString(), travelPlan.Destination.ToString()));

                DirectionsServiceMock
                    .Setup(service => service.GetDirections(It.IsAny<DirectionsApiRequest>()))
                    .Throws(new GoogleApiException(new Exception()));

                //act
                var travelInformation = await ServiceUnderTest.CalculateInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.Should().BeNull();
            }
        }

        protected Route CreateRouteWithOneLeg(int duration, double distance = 500) => new Route
        {
            Legs = new List<Leg>
            {
                new Leg
                {
                    Duration = new TextValue
                    {
                        Value = duration
                    },
                    Distance = new TextValue
                    {
                        Value = distance
                    }
                }
            }
        };
    }
}