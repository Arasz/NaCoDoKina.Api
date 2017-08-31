using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions
{
    [DataContract]
    public class Leg
    {
        [DataMember(Name = "steps")]
        public List<Step> Steps { get; set; }

        [DataMember(Name = "start_location")]
        public Location StartLocation { get; set; }

        [DataMember(Name = "end_location")]
        public Location EndLocation { get; set; }

        [DataMember(Name = "polyline")]
        public Polyline Polyline { get; set; }

        [DataMember(Name = "start_address")]
        public string StartAddress { get; set; }

        [DataMember(Name = "end_address")]
        public string EndAddress { get; set; }

        [DataMember(Name = "duration")]
        public TextValue Duration { get; set; }

        [DataMember(Name = "distance")]
        public TextValue Distance { get; set; }

        [DataMember(Name = "duration_in_traffic")]
        public TextValue DurationInTraffic { get; set; }

        [DataMember(Name = "arrival_time")]
        public Time ArrivalTime { get; set; }

        [DataMember(Name = "departure_time ")]
        public Time DepartureTime { get; set; }
    }
}