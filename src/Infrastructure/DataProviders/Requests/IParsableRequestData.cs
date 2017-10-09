namespace Infrastructure.DataProviders.Requests
{
    /// <summary>
    /// Request data that can be parsed to web request 
    /// </summary>
    public interface IParsableRequestData
    {
        /// <summary>
        /// Parses data to web request 
        /// </summary>
        /// <param name="requestParameters"> Parameters dynamically supplied to request </param>
        /// <returns> Data parsed to request </returns>
        Request Parse(params IRequestParameter[] requestParameters);
    }
}