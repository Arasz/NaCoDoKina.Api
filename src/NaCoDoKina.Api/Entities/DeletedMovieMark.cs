namespace NaCoDoKina.Api.Entities
{
    /// <summary>
    /// Marks movie as deleted for user 
    /// </summary>
    public class DeletedMovieMark
    {
        public DeletedMovieMark(long movieId, long userId)
        {
            MovieId = movieId;
            UserId = userId;
        }

        public DeletedMovieMark()
        {
        }

        public long MovieId { get; set; }

        public long UserId { get; set; }
    }
}