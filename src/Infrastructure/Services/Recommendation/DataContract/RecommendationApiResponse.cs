using System.Runtime.Serialization;

namespace Infrastructure.Services.Recommendation.DataContract
{
    /// <summary>
    /// Response from recommendation api 
    /// </summary>
    [DataContract]
    public class RecommendationApiResponse
    {
        [DataMember(Name = "movieId")]
        public long MovieId { get; set; }

        [DataMember(Name = "userId")]
        public long UserId { get; set; }

        [DataMember(Name = "rating")]
        public double Rating { get; set; }

        [DataMember(Name = "isPredicted")]
        public bool IsPredicted { get; set; }
    }
}