using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;
using Xunit;

namespace NaCoDoKina.Api.Infrastructure.Google
{
    public class SimpleGeocodingRequestParserTest
    {
        protected SimpleGeocodingRequestParser ParserUnderTest { get; }

        public SimpleGeocodingRequestParserTest()
        {
            ParserUnderTest = new SimpleGeocodingRequestParser();
        }
    }

    public class Parse : SimpleGeocodingRequestParserTest
    {
        [Fact]
        public void Should_return_correct_url()
        {
            var address = "Poronińska 3, 60-472 Poznań-Jeżyce, Polska";
            var request = new GeocodingApiRequest(address);
            var expectedResult = $"address={address}";

            var result = ParserUnderTest.Parse(request);

            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}