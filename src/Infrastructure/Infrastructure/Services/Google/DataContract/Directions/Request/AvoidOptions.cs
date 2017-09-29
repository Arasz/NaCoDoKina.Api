using System;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Directions.Request
{
    /// <summary>
    /// Indicates that the calculated route(s) should avoid the indicated features. 
    /// </summary>
    [Flags]
    public enum AvoidOptions
    {
        /// <summary>
        /// indicates that the calculated route should avoid toll roads/bridges 
        /// </summary>
        Tolls = 1,

        /// <summary>
        /// indicates that the calculated route should avoid highways. 
        /// </summary>
        Highways = 2,

        /// <summary>
        /// indicates that the calculated route should avoid ferries. 
        /// </summary>
        Ferries = 4,

        /// <summary>
        /// indicates that the calculated route should avoid indoor steps for walking and transit
        /// directions. Only requests that include an API key or a Google Maps APIs Premium Plan
        /// client ID will receive indoor steps by default.
        /// </summary>
        Indoor = 8
    }
}