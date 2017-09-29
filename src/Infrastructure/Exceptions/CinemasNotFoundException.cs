using NaCoDoKina.Api.Models;

namespace NaCoDoKina.Api.Exceptions
{
    public class CinemasNotFoundException : NaCoDoKinaApiException
    {
        public CinemasNotFoundException(long movieId, SearchArea searchArea)
            : base($"There is no cinema in {searchArea} playing movie {movieId}")
        {
        }

        public CinemasNotFoundException(SearchArea searchArea)
            : base($"There is no cinema in {searchArea}")
        {
        }
    }
}