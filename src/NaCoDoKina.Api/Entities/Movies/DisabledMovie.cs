namespace NaCoDoKina.Api.Entities.Movies
{
    /// <summary>
    /// Movie disabled for user 
    /// </summary>
    public class DisabledMovie
    {
        public DisabledMovie(long movieId, long userId)
        {
            MovieId = movieId;
            UserId = userId;
        }

        public DisabledMovie()
        {
        }

        /// <summary>
        /// Disabled movie id 
        /// </summary>
        public long MovieId { get; set; }

        /// <summary>
        /// User which disabled movie 
        /// </summary>
        public long UserId { get; set; }
    }
}