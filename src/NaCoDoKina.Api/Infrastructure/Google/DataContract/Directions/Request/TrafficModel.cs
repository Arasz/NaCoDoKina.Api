﻿namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions
{
    /// <summary>
    /// Specifies the assumptions to use when calculating time in traffic. 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#traffic-model"/>
    public enum TrafficModel
    {
        BestGuess,
        Pessimistic,
        Optimistic
    }
}