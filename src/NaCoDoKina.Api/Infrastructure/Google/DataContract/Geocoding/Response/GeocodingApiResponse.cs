using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Response;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Response
{
    [DataContract]
    public class GeocodingApiResponse : GoogleApiResponse
    {
        [DataMember(Name = "results")]
        public List<GeocodingApiResult> Results { get; set; }
    }
}