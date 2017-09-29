namespace NaCoDoKina.Api.Exceptions
{
    public class MovieRatingNotFoundException : NaCoDoKinaApiException
    {
        public MovieRatingNotFoundException(long movieId, long userId)
            : base($"Rating for movie {movieId} and user {userId} was not found")
        {
        }
    }
}