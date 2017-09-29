using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Geocoding.Response
{
    [DataContract]
    public class GeocodingApiResult
    {
        /// <summary>
        /// Collection containing the separate components applicable to this address. 
        /// </summary>
        [DataMember(Name = "address_components")]
        public List<AddressComponent> AddressComponents { get; set; }

        /// <summary>
        /// Is a string containing the human-readable address of this location. 
        /// </summary>
        [DataMember(Name = "formatted_address")]
        public string FormattedAddress { get; set; }

        /// <summary>
        /// Array denoting all the localities contained in a postal code. This is only present when
        /// the result is a postal code that contains multiple localities.
        /// </summary>
        [DataMember(Name = "postcode_localities")]
        public List<string> PostcodeLocalities { get; set; }

        [DataMember(Name = "geometry")]
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Is a unique identifier that can be used with other Google APIs. 
        /// </summary>
        [DataMember(Name = "place_id")]
        public string PlaceId { get; set; }

        /// <summary>
        /// Indicates that the geocoder did not return an exact match for the original request,
        /// though it was able to match part of the requested address.
        /// </summary>
        [DataMember(Name = "partial_match")]
        public bool PartialMatch { get; set; }

        /// <summary>
        /// Indicates the type of the returned result. This array contains a set of zero or more tags
        /// identifying the type of feature returned in the result.
        /// </summary>
        [DataMember(Name = "types")]
        public List<string> Types { get; set; }
    }
}