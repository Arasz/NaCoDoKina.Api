namespace NaCoDoKina.Api.Exceptions
{
    public class RatingNotFoundException : NaCoDoKinaApiException
    {
        public RatingNotFoundException(long movieId, long userId)
            : base($"Rating for movie {movieId} and user {userId} not found")
        {
        }
    }
}