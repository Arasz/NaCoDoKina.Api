namespace DataProviders
{
    /// <summary>
    /// Parses service response 
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public interface IResponseParser<out TResponse>
    {
        /// <summary>
        /// Parses response content to TResponse data type 
        /// </summary>
        /// <param name="responseContent"> Content of http response </param>
        /// <returns> Response data type </returns>
        TResponse Parse(string responseContent);
    }
}