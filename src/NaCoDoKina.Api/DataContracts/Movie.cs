namespace NaCoDoKina.Api.DataContracts
{
    /// <summary>
    /// Basic movie information 
    /// </summary>
    public class Movie
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string PosterUrl { get; set; }

        public double Rating { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Rating)}: {Rating}";
        }
    }
}