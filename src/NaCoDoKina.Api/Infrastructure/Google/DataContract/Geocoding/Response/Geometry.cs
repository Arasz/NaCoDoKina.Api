using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding.Response
{
    [DataContract]
    public class Geometry
    {
        /// <summary>
        /// Contains the geocoded latitude,longitude value. For normal address lookups, this field is
        /// typically the most important.
        /// </summary>
        [DataMember(Name = "location")]
        public Location Location { get; set; }

        /// <summary>
        /// Stores additional data about the specified location. 
        /// </summary>
        [DataMember(Name = "location_type")]
        public string LocationType { get; set; }

        /// <summary>
        /// Contains the recommended viewport for displaying the returned result. Generally the
        /// viewport is used to frame a result when displaying it to a user.
        /// </summary>
        [DataMember(Name = "viewport")]
        public ViewPort ViewPort { get; set; }

        /// <summary>
        /// Stores the bounding box which can fully contain the returned result. Note that these
        /// bounds may not match the recommended viewport.
        /// </summary>
        [DataMember(Name = "bounds")]
        public ViewPort Bounds { get; set; }
    }
}