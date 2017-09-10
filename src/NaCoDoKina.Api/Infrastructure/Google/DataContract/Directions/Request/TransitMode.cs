using System;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request
{
    /// <summary>
    /// Specifies one or more preferred modes of transit. 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#traffic-model"/>
    [Flags]
    public enum TransitMode
    {
        Bus = 1,
        Subway = 2,
        Train = 4,
        Tram = 8,
        Rail = 16
    }
}