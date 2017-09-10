namespace NaCoDoKina.Api.Configuration
{
    public class RecommendationApiConfiguration
    {
        public string BaseUrl { get; set; }

        public RecommendationApiConfiguration()
        {
            BaseUrl = "http://localhost:9000/";
        }
    }
}