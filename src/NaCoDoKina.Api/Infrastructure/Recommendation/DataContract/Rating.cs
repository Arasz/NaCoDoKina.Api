using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Recommendation.DataContract
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