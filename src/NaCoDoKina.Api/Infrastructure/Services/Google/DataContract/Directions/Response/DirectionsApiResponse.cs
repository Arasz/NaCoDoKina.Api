using System.Collections.Generic;
using System.Runtime.Serialization;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common.Response;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Directions.Response
{
    [DataContract]
    public class DirectionsApiResponse : GoogleApiResponse
    {
        /// <summary>
        /// Contains an array with details about the geocoding of origin, destination and waypoints. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#GeocodedWaypoints"/>
        [DataMember(Name = "geocoded_waypoints")]
        public List<GeocodedWaypoint> GeocodedWaypoints { get; set; }

        /// <summary>
        /// Contains an array of routes from the origin to the destination. 
        /// </summary>
        [DataMember(Name = "routes")]
        public List<Route> Routes { get; set; }

        /// <summary>
        /// Contains an array of available travel modes. This field is returned when a request
        /// specifies a travel mode and gets no results.
        /// </summary>
        [DataMember(Name = "available_travel_modes")]
        public List<string> AvailableTravelModes { get; set; }
    }
}