using System.Collections.Generic;
using System.Runtime.Serialization;
using Infrastructure.Services.Google.DataContract.Common;

namespace Infrastructure.Services.Google.DataContract.Directions.Response
{
    /// <summary>
    /// Specifies a single leg of the journey from the origin to the destination in the calculated
    /// route. For routes that contain no waypoints, the route will consist of a single "leg," but
    /// for routes that define one or more waypoints, the route will consist of one or more legs,
    /// corresponding to the specific legs of the journey.
    /// </summary>
    [DataContract]
    public class Leg
    {
        /// <summary>
        /// Contains an array of steps denoting information about each separate step of the leg of
        /// the journey.
        /// </summary>
        [DataMember(Name = "steps")]
        public List<Step> Steps { get; set; }

        /// <summary>
        /// Contains the latitude/longitude coordinates of the origin of this leg. 
        /// </summary>
        [DataMember(Name = "start_location")]
        public Location StartLocation { get; set; }

        /// <summary>
        /// Contains the latitude/longitude coordinates of the given destination of this leg. 
        /// </summary>
        [DataMember(Name = "end_location")]
        public Location EndLocation { get; set; }

        /// <summary>
        /// Contains the human-readable address (typically a street address) resulting from reverse
        /// geocoding the StartLocation of this leg.
        /// </summary>
        [DataMember(Name = "start_address")]
        public string StartAddress { get; set; }

        /// <summary>
        /// Contains the human-readable address (typically a street address) from reverse geocoding
        /// the EndLocation of this leg.
        /// </summary>
        [DataMember(Name = "end_address")]
        public string EndAddress { get; set; }

        /// <summary>
        /// Indicates the total duration of this leg 
        /// </summary>
        [DataMember(Name = "duration")]
        public TextValue Duration { get; set; }

        /// <summary>
        /// Indicates the total distance covered by this leg 
        /// </summary>
        [DataMember(Name = "distance")]
        public TextValue Distance { get; set; }

        /// <summary>
        /// Indicates the total duration of this leg. This value is an estimate of the time in
        /// traffic based on current and historical traffic conditions.
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#Routes"/>
        [DataMember(Name = "duration_in_traffic")]
        public TextValue DurationInTraffic { get; set; }

        /// <summary>
        /// Contains the estimated time of arrival for this leg. This property is only returned for
        /// transit directions
        /// </summary>
        [DataMember(Name = "arrival_time")]
        public Time ArrivalTime { get; set; }

        /// <summary>
        /// Contains the estimated time of departure for this leg. Is available only for transit directions. 
        /// </summary>
        [DataMember(Name = "departure_time ")]
        public Time DepartureTime { get; set; }
    }
}