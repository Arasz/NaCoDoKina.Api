namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Request
{
    /// <summary>
    /// Transportation mode used in direction calculations 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#TravelModes"/>
    public enum TravelModes
    {
        Driving,
        Walking,
        Bicycling,
        Transit
    }
}