using NaCoDoKina.Api.Models;
using System.Collections.Generic;
using System.Linq;

namespace NaCoDoKina.Api.Exceptions
{
    /// <summary>
    /// Thrown when there are no available shows for user 
    /// </summary>
    /// <inheritdoc/>
    public class MoviesNotFoundException : NaCoDoKinaApiException
    {
        public MoviesNotFoundException(IEnumerable<Cinema> cinemas, SearchArea searchArea)
            : base($"There is no movie played in cinemas " +
                   $"{string.Join(", ", cinemas.Select(cinema => cinema.Name))}" +
                   $" within search area {searchArea}")
        {
        }

        public MoviesNotFoundException(IEnumerable<long> moviesIds)
            : base($"Movies with ids {string.Join(",", moviesIds)} where not found")
        {
        }
    }
}