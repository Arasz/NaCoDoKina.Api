namespace NaCoDoKina.Api.DataContracts
{
    /// <summary>
    /// Basic show infromations 
    /// </summary>
    public class Movie
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string PosterUrl { get; set; }

        public int EstimatedRating { get; set; }
    }
}