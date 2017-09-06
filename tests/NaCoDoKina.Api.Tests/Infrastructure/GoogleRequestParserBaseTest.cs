using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;
using Xunit;

namespace NaCoDoKina.Api.Infrastructure
{
    public class GoogleRequestParserBaseTest
    {
        public class GoogleApiRequestFake : GoogleApiRequest
        {
            public string PropertyNameToFormat { get; set; }
        }

        public class GoogleRequestParserBaseFake : GoogleRequestParserBase<GoogleApiRequestFake>
        {
            public override string Parse(GoogleApiRequestFake request)
            {
                return FormatPropertyName(request.PropertyNameToFormat);
            }
        }

        protected IRequestParser<GoogleApiRequestFake> ParserUnderTest { get; }

        public GoogleRequestParserBaseTest()
        {
            ParserUnderTest = new GoogleRequestParserBaseFake();
        }
    }

    public class FormatPropertyName : GoogleRequestParserBaseTest
    {
        [Fact]
        public void Should_return_correctly_formatted_property()
        {
            //Arrange
            var fakeRequest = new GoogleApiRequestFake { PropertyNameToFormat = "SimpleTestProperty" };
            var expected = "simple_test_property";

            //Act
            var formatted = ParserUnderTest.Parse(fakeRequest);

            //Assert
            formatted.Should().BeEquivalentTo(expected);
        }
    }
}