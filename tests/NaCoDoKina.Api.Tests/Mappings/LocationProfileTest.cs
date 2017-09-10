using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using NaCoDoKina.Api.Mapping.Profiles;
using Xunit;

namespace NaCoDoKina.Api.Mappings
{
    public class LocationProfileTest : ProfileBaseTest<LocationProfile>
    {
        public class Map : LocationProfileTest
        {
            [Fact]
            public void Should_return_entity_location_given_google_location()
            {
                //Arrange
                var googleLocation = new Location(1, 9);

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
                var location = new Entities.Location(1, 9);

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
                var location = new DataContracts.Location(1, 9);

                //Act
                var result = Mapper.Map<Entities.Location>(location);

                //Assert
                result.Should().BeOfType<Entities.Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }

            [Fact]
            public void Should_return_entity_location_given_model_location()
            {
                //Arrange
                var location = new Models.Location(1, 9);

                //Act
                var result = Mapper.Map<Entities.Location>(location);

                //Assert
                result.Should().BeOfType<Entities.Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }

            [Fact]
            public void Should_return_model_location_given_data_contract_location()
            {
                //Arrange
                var location = new DataContracts.Location(1, 9);

                //Act
                var result = Mapper.Map<Models.Location>(location);

                //Assert
                result.Should().BeOfType<Models.Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }

            [Fact]
            public void Should_return_model_location_given_entity_location()
            {
                //Arrange
                var location = new Entities.Location(1, 9);

                //Act
                var result = Mapper.Map<Models.Location>(location);

                //Assert
                result.Should().BeOfType<Models.Location>();
                result.Longitude.Should().Be(location.Longitude);
                result.Latitude.Should().Be(location.Latitude);
            }

            [Fact]
            public void Should_return_data_contract_location_given_entity_location()
            {
                //Arrange
                var googleLocation = new Entities.Location(1, 3);

                //Act
                var result = Mapper.Map<DataContracts.Location>(googleLocation);

                //Assert
                result.Should().BeOfType<DataContracts.Location>();
                result.Longitude.Should().Be(googleLocation.Longitude);
                result.Latitude.Should().Be(googleLocation.Latitude);
            }

            [Fact]
            public void Should_return_data_contract_location_given_model_location()
            {
                //Arrange
                var googleLocation = new Models.Location(1, 3);

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