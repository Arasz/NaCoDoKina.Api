using System.Collections.Generic;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract
{
    public class GeocodingApiResponse
    {
        public List<GeocodingApiResult> Results { get; set; }

        public string Status { get; set; }
    }
}