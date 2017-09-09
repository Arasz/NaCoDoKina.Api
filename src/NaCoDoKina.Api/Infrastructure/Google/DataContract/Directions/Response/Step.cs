using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response
{
    /// <summary>
    /// Defines a single step of the calculated directions. A step is the most atomic unit of a
    /// direction's route, containing a single step describing a specific, single instruction on the
    /// journey. E.g. "Turn left at W. 4th St."
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#Routes"/>
    /// <remarks> Step class doesn't have TransitDetails field </remarks>
    [DataContract]
    public class Step
    {
        /// <summary>
        /// Contains the location of the starting point of this step 
        /// </summary>
        [DataMember(Name = "start_location")]
        public Location StartLocation { get; set; }

        /// <summary>
        /// Contains the location of the end point of this step 
        /// </summary>
        [DataMember(Name = "end_location")]
        public Location EndLocation { get; set; }

        /// <summary>
        /// Contains a single points object that holds an encoded polyline representation of the step. 
        /// </summary>
        /// <see cref="https://developers.google.com/maps/documentation/utilities/polylinealgorithm"/>
        [DataMember(Name = "polyline")]
        public Polyline Polyline { get; set; }

        /// <summary>
        /// Contains formatted instructions for this step, presented as an HTML text string. 
        /// </summary>
        [DataMember(Name = "html_instructions")]
        public string HtmlInstructions { get; set; }

        /// <summary>
        /// Contains the typical time required to perform the step, until the next step. 
        /// </summary>
        [DataMember(Name = "duration")]
        public TextValue Duration { get; set; }

        /// <summary>
        /// contains detailed directions for walking or driving steps in transit directions. Substeps
        /// are only available when TravelMode is set to "transit".
        /// </summary>
        [DataMember(Name = "steps")]
        public Step[] Steps { get; set; }

        /// <summary>
        /// Contains the distance covered by this step until the next step 
        /// </summary>
        [DataMember(Name = "distance")]
        public TextValue Distance { get; set; }
    }
}