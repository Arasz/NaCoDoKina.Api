using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding
{
    public class OnlyRequiredGeocodingRequestParser : GoogleRequestParserBase<GeocodingApiRequest>
    {
        public override string Parse(GeocodingApiRequest request)
        {
            return $"{FormatPropertyName(nameof(request.Address))}={request.Address}&" +
                   $"{FormatPropertyName(nameof(request.Key))}={request.Key}";
        }
    }
}