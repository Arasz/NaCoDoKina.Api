using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Response;
using NaCoDoKina.Api.Infrastructure.Google.Exceptions;
using NaCoDoKina.Api.Infrastructure.Google.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using NaCoDoKina.Api.Infrastructure;
using Xunit;

namespace NaCoDoKina.Api.IntegrationTests.Infrastructure.Google
{
    public class GeocodingServiceTest : GoogleServiceTestBase<IGoogleGeocodingService, GeocodingApiRequest>
    {
        protected override IRequestParser<GeocodingApiRequest> RequestParser
            => new OnlyRequiredGeocodingRequestParser();

        protected override IGoogleGeocodingService CreateServiceUnderTest(GoogleServiceDependencies<GeocodingApiRequest> dependencies)
            => new GoogleGeocodingService(dependencies);

        public class GeocodeAsync : GeocodingServiceTest
        {
            [Fact]
            public async Task Should_return_geolocation_for_given_address()
            {
                //arrange
                var testAddress = "Poronińska 3, 60-472 Poznań-Jeżyce, Polska";
                var testGeolocation = new Location { Longitude = 16.882369, Latitude = 52.4531839 };
                var apiRequest = new GeocodingApiRequest(testAddress);

                //act
                var response = await ServiceUnderTest.GeocodeAsync(apiRequest);

                //assert
                response.Status.Should().Be("OK");
                response.Results.Should().NotBeEmpty();
                var geolocation = response.Results.First().Geometry.Location;

                geolocation.Latitude.Should().Be(testGeolocation.Latitude);
                geolocation.Longitude.Should().Be(testGeolocation.Longitude);
            }

            [Fact]
            public void Should_return_invalid_request_when_address_not_given()
            {
                //arrange
                var testAddress = "";
                var apiRequest = new GeocodingApiRequest(testAddress);

                //act
                Func<Task<GeocodingApiResponse>> action = async () => await ServiceUnderTest.GeocodeAsync(apiRequest);

                //assert
                action.ShouldThrow<GoogleApiException>()
                    .Which.Status.Should().HaveFlag(GoogleApiStatus.InvalidRequest);
            }

            [Fact]
            public void Should_return_zero_results_when_address_is_incorrect()
            {
                //arrange
                var testAddress = "dkfkfdfddfd";
                var apiRequest = new GeocodingApiRequest(testAddress);

                //act
                Func<Task<GeocodingApiResponse>> action = async () => await ServiceUnderTest.GeocodeAsync(apiRequest);

                //assert
                action.ShouldThrow<GoogleApiException>()
                    .Which.Status.Should().HaveFlag(GoogleApiStatus.ZeroResults);
            }
        }
    }
}