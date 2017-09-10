using System.Collections.Generic;

namespace NaCoDoKina.Api.Exceptions
{
    /// <summary>
    /// Thrown when there are no available shows for user 
    /// </summary>
    /// <inheritdoc/>
    public class MoviesNotFoundException : NaCoDoKinaApiException
    {
        public MoviesNotFoundException(IEnumerable<long> moviesIds)
            : base($"Movies with ids {string.Join(",", moviesIds)} where not found")
        {
        }
    }
}