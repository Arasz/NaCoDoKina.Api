using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response
{
    [DataContract]
    public class DirectionsApiResponse
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "geocoded_waypoints ")]
        public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }

        [DataMember(Name = "routes ")]
        public List<Route> Routes { get; set; }

        [DataMember(Name = "available_travel_modes ")]
        public List<string> AvailableTravelModes { get; set; }
    }
}