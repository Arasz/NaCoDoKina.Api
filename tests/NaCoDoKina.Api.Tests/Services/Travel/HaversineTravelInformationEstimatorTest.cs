using FluentAssertions;
using Infrastructure.Models;
using Infrastructure.Models.Travel;
using Infrastructure.Services.Travel;
using System;
using TestsCore;
using Xunit;

namespace NaCoDoKina.Api.Services.Travel
{
    public class HaversineTravelInformationEstimatorTest : UnitTestBase
    {
        [Fact]
        public void Should_estimate_correct_travel_distance_and_duration()
        {
            // Arrange
            var origin = new Location
            {
                Longitude = -73.9864,
                Latitude = 40.7486
            };
            var destination = new Location
            {
                Longitude = 16.882369,
                Latitude = 52.4531839
            };
            var travelPlan = new TravelPlan(origin, destination);
            var expectedDistance = 6593959;
            var expectedDuration = TimeSpan.FromSeconds(Math.Round((expectedDistance / 50e3) * 60 * 60));
            // Act

            var estimator = new HaversineTravelInformationEstimator();
            var travelInfo = estimator.Estimate(travelPlan);

            // Assert
            travelInfo.Distance
                .Should()
                .Be(expectedDistance);
            travelInfo.Duration
                .Should()
                .Be(expectedDuration);
        }
    }
}