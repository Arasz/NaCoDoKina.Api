namespace Infrastructure.Services.Google.DataContract.Directions.Request
{
    /// <summary>
    /// Transportation mode used in direction calculations 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#TravelMode"/>
    public enum TravelMode
    {
        Driving,
        Walking,
        Bicycling,
        Transit
    }
}