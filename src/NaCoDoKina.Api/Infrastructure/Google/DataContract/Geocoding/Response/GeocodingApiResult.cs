using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Response
{
    [DataContract]
    public class GeocodingApiResult
    {
        [DataMember(Name = "address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

        [DataMember(Name = "formatted_address")]
        public string FormattedAddress { get; set; }

        [DataMember(Name = "geometry")]
        public Geometry Geometry { get; set; }

        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }

        [DataMember(Name = "types")]
        public List<string> Types { get; set; }
    }
}