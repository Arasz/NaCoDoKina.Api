using System.Collections.Generic;
using System.Runtime.Serialization;
using Infrastructure.Services.Google.DataContract.Common.Response;

namespace Infrastructure.Services.Google.DataContract.Geocoding.Response
{
    [DataContract]
    public class GeocodingApiResponse : GoogleApiResponse
    {
        [DataMember(Name = "results")]
        public List<GeocodingApiResult> Results { get; set; }
    }
}