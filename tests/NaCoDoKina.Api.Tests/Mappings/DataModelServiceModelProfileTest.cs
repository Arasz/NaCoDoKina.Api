using FluentAssertions;
using NaCoDoKina.Api.Mapping.Profiles;
using Xunit;

namespace NaCoDoKina.Api.Mappings
{
    public class DataModelServiceModelProfileTest : ProfileTestBase<DataModelServiceModelProfile>
    {
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
    }
}