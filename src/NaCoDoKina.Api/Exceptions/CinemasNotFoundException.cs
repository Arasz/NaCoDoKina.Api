using NaCoDoKina.Api.Models;

namespace NaCoDoKina.Api.Exceptions
{
    public class CinemasNotFoundException : NaCoDoKinaApiException
    {
        public CinemasNotFoundException(long movieId, SearchArea searchArea)
            : base($"Cinemas near {searchArea} playing movie {movieId} were not found")
        {
        }
    }
}