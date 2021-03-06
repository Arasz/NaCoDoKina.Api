using FluentAssertions;
using Infrastructure.Mappings;
using Infrastructure.Models.Travel;
using Infrastructure.Services.Google.DataContract.Directions.Request;
using Infrastructure.Services.Google.DataContract.Geocoding.Request;
using Xunit;
using Location = Infrastructure.Models.Location;
using TravelMode = Infrastructure.Services.Google.DataContract.Directions.Request.TravelMode;

namespace NaCoDoKina.Api.Mappings
{
    public class LocationServiceProfileTest : ProfileTestBase<TravelServiceProfile>
    {
        public class Map : LocationServiceProfileTest
        {
            private string ParseTuple((double Latitude, double Longitude) locationTuple)
            {
                return $"{locationTuple.Latitude},{locationTuple.Longitude}";
            }

            [Fact]
            public void Should_return_travel_plan_for_api_request()
            {
                //Arrange
                var orign = (Lat: 0, Lng: 1);
                var destination = (Lat: 2, Lng: 3);
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
                result.TravelMode.Should().HaveFlag(global::Infrastructure.Models.Travel.TravelMode.Driving);
            }

            [Fact]
            public void Should_return_api_request_for_travel_plan()
            {
                //Arrange
                var orign = new Location(1, 0);
                var destination = new Location(3, 2);
                var travelPlan = new TravelPlan(orign, destination, global::Infrastructure.Models.Travel.TravelMode.Bicycling);

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