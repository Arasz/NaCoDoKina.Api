using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using Xunit;

namespace NaCoDoKina.Api.Infrastructure.Profiles
{
    public class LocationProfileTest : ProfileBaseTest<LocationProfile>
    {
        public class Map : LocationProfileTest
        {
            [Fact]
            public void Should_return_entity_location_given_google_location()
            {
                //Arrange
                var googleLocation = new Location(9, 9);

                //Act
                var result = Mapper.Map<Entities.Location>(googleLocation);

                //Assert
                result.Should().BeOfType<Entities.Location>();
                result.Longitude.Should().Be(googleLocation.Longitude);
                result.Latitude.Should().Be(googleLocation.Latitude);
            }

            [Fact]
            public void Should_return_google_location_given_entity_location()
            {
                //Arrange
                var location = new Entities.Location(9, 9);

                //Act
                var result = Mapper.Map<Location>(location);

                //Assert
                result.Should().BeOfType<Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }

            [Fact]
            public void Should_return_entity_location_given_data_contract_location()
            {
                //Arrange
                var location = new DataContracts.Location(9, 9);

                //Act
                var result = Mapper.Map<Entities.Location>(location);

                //Assert
                result.Should().BeOfType<Entities.Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }

            [Fact]
            public void Should_return_data_contract_location_given_entity_location()
            {
                //Arrange
                var googleLocation = new Entities.Location(9, 9);

                //Act
                var result = Mapper.Map<DataContracts.Location>(googleLocation);

                //Assert
                result.Should().BeOfType<DataContracts.Location>();
                result.Longitude.Should().Be(googleLocation.Longitude);
                result.Latitude.Should().Be(googleLocation.Latitude);
            }
        }
    }
}