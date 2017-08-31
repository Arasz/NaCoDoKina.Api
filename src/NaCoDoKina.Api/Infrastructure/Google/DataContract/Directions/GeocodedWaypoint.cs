using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions
{
    [DataContract]
    public class GeocodedWaypoint
    {
        [DataMember(Name = "geocoder_status")]
        public string GeocoderStatus { get; set; }

        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }

        [DataMember(Name = "types")]
        public List<string> Types { get; set; }
    }
}