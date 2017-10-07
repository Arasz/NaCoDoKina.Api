using ApplicationCore.Results;

namespace NaCoDoKina.Api.DataProviders.Requests
{
    /// <summary>
    /// Request parameter 
    /// </summary>
    public interface IRequestParameter
    {
        /// <summary>
        /// Substitutes all occurrences of template in pattern for value 
        /// </summary>
        /// <param name="pattern"> String with templates </param>
        /// <returns> Pattern with substituted templates </returns>
        Result<string> SubstituteTemplate(string pattern);
    }
}