using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Directions.Request;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Directions
{
    public class OnlyRequiredDirectionsRequestParser : GoogleRequestParserBase<DirectionsApiRequest>
    {
        public override string Parse(DirectionsApiRequest request)
        {
            return $"{FormatPropertyName(nameof(request.Origin))}={request.Origin}&" +
                   $"{FormatPropertyName(nameof(request.Destination))}={request.Destination}&" +
                   $"{FormatPropertyName(nameof(request.Key))}={request.Key}";
        }
    }
}