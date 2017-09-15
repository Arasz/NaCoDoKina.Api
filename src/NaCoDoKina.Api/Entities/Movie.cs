namespace NaCoDoKina.Api.Entities
{
    /// <inheritdoc/>
    /// <summary>
    /// Basic movie information 
    /// </summary>
    public class Movie : Entity
    {
        public string Name { get; set; }

        public string PosterUrl { get; set; }

        public MovieDetails Details { get; set; }
    }
}