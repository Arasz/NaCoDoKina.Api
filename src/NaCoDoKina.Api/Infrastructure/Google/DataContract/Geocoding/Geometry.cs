using System.Runtime.Serialization;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding
{
    [DataContract]
    public class Geometry
    {
        [DataMember(Name = "location")]
        public Location Location { get; set; }

        [DataMember(Name = "location_type")]
        public string LocationType { get; set; }

        [DataMember(Name = "viewport")]
        public ViewPort ViewPort { get; set; }
    }
}