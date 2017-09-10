namespace NaCoDoKina.Api.Exceptions
{
    public class ShowNotFoundException : NaCoDoKinaApiException
    {
        public ShowNotFoundException(long movieId)
            : base($"Movie {movieId} was not found")
        {
        }
    }
}