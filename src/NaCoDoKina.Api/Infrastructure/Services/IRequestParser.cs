namespace NaCoDoKina.Api.Infrastructure.Services
{
    /// <summary>
    /// Request parsing strategy 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IRequestParser<in TRequest>
    {
        /// <summary>
        /// Parses request to url 
        /// </summary>
        /// <param name="request"> Request </param>
        /// <returns> Url </returns>
        string Parse(TRequest request);
    }
}