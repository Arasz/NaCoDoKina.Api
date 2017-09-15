namespace NaCoDoKina.Api.Exceptions
{
    public class MovieNotFoundException : NaCoDoKinaApiException
    {
        public MovieNotFoundException(long movieId)
            : base($"Movie {movieId} was not found")
        {
        }
    }
}