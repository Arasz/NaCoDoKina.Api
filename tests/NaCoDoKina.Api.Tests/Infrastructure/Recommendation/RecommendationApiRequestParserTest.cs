using FluentAssertions;
using NaCoDoKina.Api.Infrastructure.Services;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation;
using NaCoDoKina.Api.Infrastructure.Services.Recommendation.DataContract;
using Xunit;

namespace NaCoDoKina.Api.Infrastructure.Recommendation
{
    public class RecommendationApiRequestParserTest
    {
        private IRequestParser<RecommendationApiRequest> RequestParser { get; }

        public RecommendationApiRequestParserTest()
        {
            RequestParser = new RecommendationRequestParser();
        }

        public class Parse : RecommendationApiRequestParserTest
        {
            [Fact]
            public void Should_parse_request_to_correct_url()
            {
                //Arrange
                var movieId = 55;
                var userId = 69;
                var expectedUrl = $"/v1/ratings/movies/{movieId}/users/{userId}";
                var request = new RecommendationApiRequest(userId, movieId);

                //Act
                var url = RequestParser.Parse(request);

                //Assert
                url.Should().Be(expectedUrl);
            }
        }
    }
}