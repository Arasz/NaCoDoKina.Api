using FluentAssertions;
using Infrastructure.Models.Travel;
using Infrastructure.Services.Google.DataContract.Directions.Response;
using Infrastructure.Services.Travel;
using IntegrationTestsCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Location = Infrastructure.Models.Location;

namespace NaCoDoKina.Api.IntegrationTests.Services
{
    public class TravelServiceTest : HttpTestBase<Startup>
    {
        public TravelServiceTest()
        {
            ServiceUnderTest = Services.GetService<ITravelService>();
        }

        public ITravelService ServiceUnderTest { get; set; }

        public class TranslateAddressToTravelAsync : TravelServiceTest
        {
            [Fact]
            public async Task Should_return_correct_location_when_given_correct_address()
            {
                //arrange
                var testAddress = "Poronińska 3, 60-472 Poznań-Jeżyce, Polska";
                var expectedLocation = new Location(52.4531839, 16.882369);

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
                var testAddress = "";

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

                //act
                var travelInformation = await ServiceUnderTest.GetInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.TravelPlan.Should().Be(travelPlan);
                travelInformation.Duration.Should().BeGreaterThan(TimeSpan.Zero);
                travelInformation.Distance.Should().BeGreaterThan(0);
            }

            [Fact]
            public async Task Should_return_longest_route_time_when_returns_multiple_routes()
            {
                //arrange
                var destination = new Location(52.44056, 16.919235);
                var origin = new Location(52.3846579, 16.8519869);
                var travelPlan = new TravelPlan(origin, destination);

                //act
                var travelInformation = await ServiceUnderTest.GetInformationForTravelAsync(travelPlan);

                //assert
                travelInformation.TravelPlan.Should().Be(travelPlan);
                travelInformation.Duration.Should().BeGreaterThan(TimeSpan.Zero);
                travelInformation.Distance.Should().BeGreaterThan(0);
            }

            [Fact]
            public async Task Should_return_estimated_route_when_api_returns_errors()
            {
                //arrange
                var destination = new Location(-99999, -99999);
                var origin = new Location(-99999, -99999);
                var travelPlan = new TravelPlan(origin, destination);

                //act
                var travelInformation = await ServiceUnderTest.GetInformationForTravelAsync(travelPlan);

                //assert
                var estimator = Services.GetService<ITravelInformationEstimator>();
                travelInformation
                    .Distance.Should().Be(estimator.Estimate(travelPlan).Distance);
            }
        }

        protected Route CreateRouteWithOneLeg(int duration) => new Route
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
        };
    }
}