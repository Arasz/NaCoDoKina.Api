using System.Collections.Generic;
using System.Runtime.Serialization;
using Infrastructure.Services.Google.DataContract.Common;

namespace Infrastructure.Services.Google.DataContract.Directions.Response
{
    /// <summary>
    /// Contains a single result from the specified origin and destination 
    /// </summary>
    [DataContract]
    public class Route
    {
        /// <summary>
        /// Contains a short textual description for the route, suitable for naming and
        /// disambiguating the route from alternatives.
        /// </summary>
        [DataMember(Name = "summary")]
        public string Summary { get; set; }

        /// <summary>
        /// Contains an array which contains information about a leg of the route, between two
        /// locations within the given route.
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/directions/intro#Legs"/>
        [DataMember(Name = "legs")]
        public List<Leg> Legs { get; set; }

        /// <summary>
        /// Contains an array indicating the order of any waypoints in the calculated route. 
        /// </summary>
        [DataMember(Name = "waypoint_order")]
        public List<int> WaypointOrder { get; set; }

        /// <summary>
        /// Contains a single points object that holds an encoded polyline representation of the route. 
        /// </summary>
        [DataMember(Name = "overview_polyline")]
        public Polyline OverviewPolyline { get; set; }

        /// <summary>
        /// </summary>
        [DataMember(Name = "copyrights")]
        public string Copyrights { get; set; }

        /// <summary>
        /// Contains an array of warnings to be displayed when showing these directions. 
        /// </summary>
        [DataMember(Name = "warnings")]
        public List<string> Warnings { get; set; }

        /// <summary>
        /// Contains the viewport bounding box of the OverviewPolyline 
        /// </summary>
        [DataMember(Name = "bounds")]
        public ViewPort Bounds { get; set; }
    }
}