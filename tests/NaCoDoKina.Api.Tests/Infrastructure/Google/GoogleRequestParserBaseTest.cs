using System.Collections.Generic;
using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Services;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common.Request;
using Xunit;

namespace NaCoDoKina.Api.Infrastructure.Google
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
        public static IEnumerable<object[]> TestData => new List<object[]>
        {
            new object[] {"SimpleTestProperty", "simple_test_property"},
            new object[] {"Simple", "simple"},
            new object[] {"PrOpErTy", "pr_op_er_ty"},
            new object[] {"PROpErTy", "p_r_op_er_ty"}
        };

        [Theory, MemberData(nameof(TestData))]
        public void Should_return_correctly_formatted_property(string propertyName, string expectedPropertyName)
        {
            //Arrange
            var fakeRequest = new GoogleApiRequestFake { PropertyNameToFormat = propertyName };
            var expected = expectedPropertyName;

            //Act
            var formatted = ParserUnderTest.Parse(fakeRequest);

            //Assert
            formatted.Should().BeEquivalentTo(expected);
        }
    }
}