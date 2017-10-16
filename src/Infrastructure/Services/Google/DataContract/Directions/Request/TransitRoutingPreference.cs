namespace Infrastructure.Services.Google.DataContract.Directions.Request
{
    /// <summary>
    /// Specifies preferences for transit routes. 
    /// </summary>
    /// <see cref="https://developers.google.com/maps/documentation/directions/intro#traffic-model"/>
    public enum TransitRoutingPreference
    {
        LessWalking,
        FewerTransfers
    }
}