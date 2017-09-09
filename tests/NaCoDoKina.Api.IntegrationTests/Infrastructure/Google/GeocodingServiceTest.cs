using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;
using NaCoDoKina.Api.Infrastructure.Google.Services;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Google
{
    public class GeocodingServiceTest : GoogleServiceTestBase<IGeocodingService, GeocodingApiRequest>
    {
        protected override IRequestParser<GeocodingApiRequest> RequestParser
            => new OnlyRequiredGeocodingRequestParser();

        protected override IGeocodingService CreateServiceUnderTest(GoogleServiceDependencies<GeocodingApiRequest> dependencies)
            => new GeocodingService(dependencies);

        public class GeocodeAsync : GeocodingServiceTest
        {
            [Fact]
            public async Task Should_return_geolocation_for_given_address()
            {
                //arrange
                var testAddress = "Poronińska 3, 60-472 Poznań-Jeżyce, Polska";
                var testGeolocation = new Location { Lat = 52.4531839, Lng = 16.882369 };
                var apiRequest = new GeocodingApiRequest(testAddress);

                //act
                var response = await ServiceUnderTest.GeocodeAsync(apiRequest);

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
                var apiRequest = new GeocodingApiRequest(testAddress);

                //act
                var response = await ServiceUnderTest.GeocodeAsync(apiRequest);

                //assert
                response.Status.Should().Be("INVALID_REQUEST");
            }

            [Fact]
            public async Task Should_return_zero_results_when_address_is_incorrect()
            {
                //arrange
                var testAddress = "dkfkfdfddfd";
                var apiRequest = new GeocodingApiRequest(testAddress);

                //act
                var response = await ServiceUnderTest.GeocodeAsync(apiRequest);

                //assert
                response.Status.Should().Be("ZERO_RESULTS");
            }
        }
    }
}