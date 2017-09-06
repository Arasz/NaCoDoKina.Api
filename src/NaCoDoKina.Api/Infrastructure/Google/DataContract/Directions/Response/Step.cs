using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response
{
    [DataContract]
    public class Step
    {
        [DataMember(Name = "travel_mode")]
        public string TravelMode { get; set; }

        [DataMember(Name = "start_location")]
        public Location StartLocation { get; set; }

        [DataMember(Name = "end_location")]
        public Location EndLocation { get; set; }

        [DataMember(Name = "polyline")]
        public Polyline Polyline { get; set; }

        [DataMember(Name = "html_instructions")]
        public string HtmlInstructions { get; set; }

        [DataMember(Name = "duration")]
        public TextValue Duration { get; set; }

        [DataMember(Name = "distance")]
        public TextValue Distance { get; set; }
    }
}