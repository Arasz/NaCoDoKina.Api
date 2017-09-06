using System;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request
{
    /// <summary>
    /// Request to google directions api. 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#traffic-model"/>
    public class DirectionsApiRequest : GoogleApiRequest
    {
        /// <summary>
        /// The address, textual latitude/longitude value, or place ID from which you wish to
        /// calculate directions.
        /// </summary>
        public string Origin { get; set; }

        /// <summary>
        /// The address, textual latitude/longitude value, or place ID to which you wish to calculate directions.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Transportation mode used in direction calculations 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#TravelModes"/>
        public TravelModes TravelMode { get; set; }

        /// <summary>
        /// Specifies an array of waypoints. Waypoints alter a route by routing it through the
        /// specified location(s).
        /// </summary>
        public GeocodedWaypoint[] Waypoints { get; set; }

        /// <summary>
        /// If set to true, specifies that the Directions service may provide more than one route
        /// alternative in the response.
        /// </summary>
        public bool Alternatives { get; set; }

        /// <summary>
        /// Indicates that the calculated route(s) should avoid the indicated features. 
        /// </summary>
        public AvoidOptions AvoidOptions { get; set; }

        /// <summary>
        /// The language in which to return results. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/faq#languagesupport"/>
        public string Language { get; set; }

        /// <summary>
        /// Specifies the unit system to use when displaying results. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#UnitSystems"/>
        public UnitSystem UnitSystem { get; set; }

        /// <summary>
        /// Specifies the region code, specified as a ccTLD ("top-level domain") two-character value. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#RegionBiasing"/>
        public string Region { get; set; }

        /// <summary>
        /// Specifies the desired time of arrival for transit directions, in seconds since midnight,
        /// January 1, 1970 UTC. You can specify either DepartureTime or ArrivalTime, but not both.
        /// </summary>
        /// <remarks> Note that ArrivalTime must be specified as an integer. </remarks>
        public DateTime ArrivalTime { get; set; }

        /// <summary>
        /// Specifies the desired time of arrival for transit directions, in seconds since midnight,
        /// January 1, 1970 UTC. You can specify either DepartureTime or ArrivalTime, but not both.
        /// </summary>
        public DateTime DepartureTime { get; set; }

        /// <summary>
        /// Specifies the assumptions to use when calculating time in traffic. 
        /// </summary>
        public TrafficModel TrafficModel { get; set; }

        /// <summary>
        /// Specifies one or more preferred modes of transit. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#traffic-model"/>
        public TransitMode TransitMode { get; set; }

        /// <summary>
        /// Specifies preferences for transit routes. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#traffic-model"/>

        public TransitRoutingPreference TransitRoutingPreference { get; set; }

        public DirectionsApiRequest(string origin, string destination, string key) : base(key)
        {
            Origin = origin;
            Destination = destination;
        }
    }
}