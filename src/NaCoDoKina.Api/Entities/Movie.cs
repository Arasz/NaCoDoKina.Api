namespace NaCoDoKina.Api.Entities
{
    /// <summary>
    /// Basic movie information 
    /// </summary>
    public class Movie : Entity
    {
        public string Title { get; set; }

        public MovieDetails Details { get; set; }
    }
}