using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Geocoding.Request;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Geocoding
{
    /// <summary>
    /// Parses only required request parameters 
    /// </summary>
    public class OnlyRequiredGeocodingRequestParser : GoogleRequestParserBase<GeocodingApiRequest>
    {
        public override string Parse(GeocodingApiRequest request)
        {
            return $"{FormatPropertyName(nameof(request.Address))}={request.Address}&" +
                   $"{FormatPropertyName(nameof(request.Key))}={request.Key}";
        }
    }
}