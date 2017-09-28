namespace NaCoDoKina.Api.Models.Movies
{
    /// <summary>
    /// Basic movie information 
    /// </summary>
    public class Movie
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public string PosterUrl { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Title)}: {Title}";
        }
    }
}