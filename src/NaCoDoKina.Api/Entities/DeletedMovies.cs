namespace NaCoDoKina.Api.Entities
{
    /// <summary>
    /// Marks movie as deleted for user 
    /// </summary>
    public class DeletedMovies
    {
        public DeletedMovies(long movieId, long userId)
        {
            MovieId = movieId;
            UserId = userId;
        }

        public DeletedMovies()
        {
        }

        public long MovieId { get; set; }

        public long UserId { get; set; }
    }
}