using System.Runtime.Serialization;

namespace Infrastructure.Services.Recommendation.DataContract
{
    /// <summary>
    /// Movie rating 
    /// </summary>
    [DataContract]
    public class Rating
    {
        [DataMember(Name = "rating")]
        public double MovieRating { get; }

        public Rating(double movieRating)
        {
            MovieRating = movieRating;
        }
    }
}