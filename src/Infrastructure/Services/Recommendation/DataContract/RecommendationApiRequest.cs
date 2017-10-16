namespace Infrastructure.Services.Recommendation.DataContract
{
    /// <summary>
    /// Request for movie rating 
    /// </summary>
    public class RecommendationApiRequest
    {
        public long UserId { get; }

        public long MovieId { get; }

        public RecommendationApiRequest(long userId, long movieId)
        {
            UserId = userId;
            MovieId = movieId;
        }
    }
}