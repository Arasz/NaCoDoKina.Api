using NaCoDoKina.Api.DataContracts.Resources;

namespace NaCoDoKina.Api.Entities.Movies
{
    /// <inheritdoc/>
    /// <summary>
    /// Basic movie information 
    /// </summary>
    public class Movie : Entity
    {
        /// <summary>
        /// Movie name (title) 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Movie poster url 
        /// </summary>
        public MediaLink PosterUrl { get; set; }

        /// <summary>
        /// Detailed information about movie 
        /// </summary>
        public MovieDetails Details { get; set; }
    }
}