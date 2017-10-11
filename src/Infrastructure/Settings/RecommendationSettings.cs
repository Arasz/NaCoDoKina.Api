namespace Infrastructure.Settings
{
    public class RecommendationSettings
    {
        public string BaseUrl { get; set; }

        public RecommendationSettings()
        {
            BaseUrl = "https://movie-recommender-service.herokuapp.com";
        }
    }
}