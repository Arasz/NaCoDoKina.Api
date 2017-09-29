using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common.Request;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Geocoding.Request
{
    /// <summary>
    /// Request for google geocoding api 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/geocoding/intro"/>
    public class GeocodingApiRequest : GoogleApiRequest
    {
        /// <summary>
        /// The street address that you want to geocode, in the format used by the national postal
        /// service of the country concerned. Additional address elements such as business names and
        /// unit, suite or floor numbers should be avoided.
        /// </summary>
        /// <seealso cref="https://developers.google.com/maps/faq#geocoder_queryformat"/>
        public string Address { get; set; }

        /// <summary>
        /// A components filter with elements separated by a pipe (|). The components filter is also
        /// accepted as an optional parameter if an address is provided.
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/geocoding/intro#ComponentFiltering"/>
        public string Components { get; set; }

        public GeocodingApiRequest(string address)
        {
            Address = address;
        }

        /// <summary>
        /// The language in which to return results. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/faq#languagesupport"/>
        public string Language { get; set; }

        /// <summary>
        /// The region code, specified as a ccTLD ("top-level domain") two-character value. 
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// The bounding box of the viewport within which to bias geocode results more prominently. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/geocoding/intro#Viewports"/>
        public ViewPort Bounds { get; set; }
    }
}