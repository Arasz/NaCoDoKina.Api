using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request;
using Xunit;

namespace NaCoDoKina.Api.Infrastructure.Google
{
    public class OnlyRequiredDirectionsRequestParserTest
    {
        protected OnlyRequiredDirectionsRequestParser ParserUnderTest { get; }

        public OnlyRequiredDirectionsRequestParserTest()
        {
            ParserUnderTest = new OnlyRequiredDirectionsRequestParser();
        }

        public class Parse : OnlyRequiredDirectionsRequestParserTest
        {
            [Fact]
            public void Should_return_correct_url_when_given_minmal_location()
            {
                var destination = "52.44056,16.919235";
                var origin = "52.3846579,16.8519869";
                var key = "key";
                var request = new DirectionsApiRequest(origin, destination)
                {
                    Key = key,
                };
                var expectedResult = $"origin={origin}&destination={destination}&key={key}";

                var result = ParserUnderTest.Parse(request);

                result.Should().BeEquivalentTo(expectedResult);
            }
        }
    }
}