using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Infrastructure.Services.Google.DataContract.Directions.Response
{
    /// <summary>
    /// Details about the geocoding of every waypoint, as well as origin and destination 
    /// </summary>
    [DataContract]
    public class GeocodedWaypoint
    {
        /// <summary>
        /// Indicates the status code resulting from the geocoding operation. 
        /// </summary>
        [DataMember(Name = "geocoder_status")]
        public string GeocoderStatus { get; set; }

        /// <summary>
        /// Indicates that the geocoder did not return an exact match for the original request,
        /// though it was able to match part of the requested address.
        /// </summary>
        [DataMember(Name = "partial_match")]
        public bool PartialMatch { get; set; }

        /// <summary>
        /// Is a unique identifier that can be used with other Google APIs. 
        /// </summary>
        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }

        /// <summary>
        /// Indicates the address type of the geocoding result used for calculating directions. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#TransitDetails"/>
        [DataMember(Name = "types")]
        public List<string> Types { get; set; }
    }
}