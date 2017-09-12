namespace NaCoDoKina.Api.Entities
{
    public class Movie : Entity
    {
        public string Title { get; set; }

        public MovieDetails Details { get; set; }
    }
}