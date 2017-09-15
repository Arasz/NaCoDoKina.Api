namespace NaCoDoKina.Api.Configuration
{
    public class RecommendationApiConfiguration
    {
        public string BaseUrl { get; set; }

        public RecommendationApiConfiguration()
        {
            BaseUrl = "https://movie-recommender-service.herokuapp.com";
        }
    }
}