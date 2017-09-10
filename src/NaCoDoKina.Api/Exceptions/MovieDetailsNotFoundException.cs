namespace NaCoDoKina.Api.Exceptions
{
    public class MovieDetailsNotFoundException : NaCoDoKinaApiException
    {
        public MovieDetailsNotFoundException(long movieId)
            : base($"Details for movie {movieId} were not found ")
        {
        }
    }
}