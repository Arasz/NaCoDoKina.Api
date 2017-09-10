using NaCoDoKina.Api.Models;

namespace NaCoDoKina.Api.Exceptions
{
    public class CinemasNotFoundException : NaCoDoKinaApiException
    {
        public CinemasNotFoundException(long movieId, Location location)
            : base($"Cinemas near {location} playing movie {movieId} were not found")
        {
        }
    }
}