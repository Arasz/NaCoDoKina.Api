using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Request;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding
{
    public class SimpleGeocodingRequestParser : IRequestParser<GeocodingApiRequest>
    {
        public string Parse(GeocodingApiRequest request)
        {
            return $"{request.Address}";
        }
    }
}