using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;
using NaCoDoKina.Api.Mapping.Profiles;
using NaCoDoKina.Api.Models;
using System;
using Xunit;
using Location = NaCoDoKina.Api.Models.Location;
using TravelMode = NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request.TravelMode;

namespace NaCoDoKina.Api.Mappings
{
    public class LocationServiceProfileTest : ProfileBaseTest<LocationServiceProfile>
    {
        public class Map : LocationServiceProfileTest
        {
            private string ParseTuple(ValueTuple<double, double> locationTuple)
            {
                return $"{locationTuple.Item1},{locationTuple.Item2}";
            }

            [Fact]
            public void Should_return_travel_plan_for_api_request()
            {
                //Arrange
                var orign = (Lng: 0, Lat: 1);
                var destination = (Lng: 2, Lat: 3);
                var request = new DirectionsApiRequest(ParseTuple(orign), ParseTuple(destination))
                {
                    TravelMode = TravelMode.Driving,
                };

                //Act
                var result = Mapper.Map<TravelPlan>(request);

                //Assert
                result.Should().BeOfType<TravelPlan>();
                result.Origin.Latitude.Should().Be(orign.Lat);
                result.Origin.Longitude.Should().Be(orign.Lng);
                result.Destination.Latitude.Should().Be(destination.Lat);
                result.Destination.Longitude.Should().Be(destination.Lng);
                result.TravelMode.Should().HaveFlag(Models.TravelMode.Driving);
            }

            [Fact]
            public void Should_return_api_request_for_travel_plan()
            {
                //Arrange
                var orign = new Location(0, 1);
                var destination = new Location(2, 3);
                var travelPlan = new TravelPlan(orign, destination, Models.TravelMode.Bicycling);

                //Act
                var result = Mapper.Map<DirectionsApiRequest>(travelPlan);

                //Assert
                result.Should().BeOfType<DirectionsApiRequest>();
                result.Origin.Should().Be(orign.ToString());
                result.Destination.Should().Be(destination.ToString());
                result.TravelMode.Should().HaveFlag(TravelMode.Bicycling);
            }

            [Fact]
            public void Should_return_geocoding_api_request_for_address()
            {
                //Arrange
                var address = "Address";

                //Act
                var result = Mapper.Map<GeocodingApiRequest>(address);

                //Assert
                result.Address.Should().BeEquivalentTo(address);
            }
        }
    }
}