using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using NaCoDoKina.Api.Infrastructure;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding;
using NaCoDoKina.Api.Infrastructure.Google.Services;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Google
{
    public class GeocodingServiceTest
    {
        protected IGeocodingService ServiceUnderTest { get; }

        public GeocodingServiceTest()
        {
            //MOVE THIS API KEY TO SAFE STORAGE
            ServiceUnderTest = new GeocodingService(new HttpClient(), new NullLogger<BaseHttpApiClient>(), "AIzaSyB0k9n49t5OXZ9XUfh8n9zUfhmdQ-_Tt5M");
        }
    }

    public class GeocodeAsync : GeocodingServiceTest
    {
        [Fact]
        public async Task Should_return_geolocation_for_given_address()
        {
            //arrange
            var testAddress = "Poronińska 3, 60-472 Poznań-Jeżyce, Polska";
            var testGeolocation = new Location { Lat = 52.4531839, Lng = 16.882369 };

            //act
            var response = await ServiceUnderTest.GeocodeAsync(testAddress);

            //assert
            response.Status.Should().Be("OK");
            response.Results.Should().NotBeEmpty();
            var geolocation = response.Results.First().Geometry.Location;

            geolocation.Lat.Should().Be(testGeolocation.Lat);
            geolocation.Lng.Should().Be(testGeolocation.Lng);
        }

        [Fact]
        public async Task Should_return_invalid_request_when_address_not_given()
        {
            //arrange
            var testAddress = "";

            //act
            var response = await ServiceUnderTest.GeocodeAsync(testAddress);

            //assert
            response.Status.Should().Be("INVALID_REQUEST");
        }

        [Fact]
        public async Task Should_return_zero_results_when_address_is_incorrect()
        {
            //arrange
            var testAddress = "dkfkfdfddfd";

            //act
            var response = await ServiceUnderTest.GeocodeAsync(testAddress);

            //assert
            response.Status.Should().Be("ZERO_RESULTS");
        }
    }
}