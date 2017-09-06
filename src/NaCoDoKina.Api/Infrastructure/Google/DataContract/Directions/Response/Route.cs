using NaCoDoKina.Api.Infrastructure.Google.DataContract.Geocoding;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions
{
    [DataContract]
    public class Route
    {
        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        [DataMember(Name = "legs")]
        public List<Leg> Legs { get; set; }

        [DataMember(Name = "waypoint_order")]
        public List<int> WaypointOrder { get; set; }

        [DataMember(Name = "overview_polyline")]
        public Polyline OverviewPolyline { get; set; }

        [DataMember(Name = "copyrights")]
        public string Copyrights { get; set; }

        [DataMember(Name = "warnings")]
        public List<string> Warnings { get; set; }

        [DataMember(Name = "bounds")]
        public ViewPort Bounds { get; set; }
    }
}