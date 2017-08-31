using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract
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