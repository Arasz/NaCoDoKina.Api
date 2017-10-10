using FluentAssertions;
using Infrastructure.Services.Google.DataContract.Geocoding;
using Infrastructure.Services.Google.DataContract.Geocoding.Request;
using Xunit;

namespace NaCoDoKina.Api.Infrastructure.Google
{
    public class OnlyRequiredGeocodingRequestParserTest
    {
        protected OnlyRequiredGeocodingRequestParser ParserUnderTest { get; }

        public OnlyRequiredGeocodingRequestParserTest()
        {
            ParserUnderTest = new OnlyRequiredGeocodingRequestParser();
        }

        public class Parse : OnlyRequiredGeocodingRequestParserTest
        {
            [Fact]
            public void Should_return_correct_url()
            {
                var address = "Poronińska 3, 60-472 Poznań-Jeżyce, Polska";
                var key = "key";
                var request = new GeocodingApiRequest(address)
                {
                    Key = key,
                };
                var expectedResult = $"address={address}&key={key}";

                var result = ParserUnderTest.Parse(request);

                result.Should().BeEquivalentTo(expectedResult);
            }
        }
    }
}