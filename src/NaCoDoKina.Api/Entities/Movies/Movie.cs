using NaCoDoKina.Api.Entities.Resources;
using System.Collections.Generic;

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
        /// External ids from cinemas 
        /// </summary>
        public List<MovieCinemaId> MovieCinemaIds { get; set; }

        /// <summary>
        /// Movie poster url 
        /// </summary>
        public MediaLink PosterUrl { get; set; }

        /// <summary>
        /// Detailed information about movie 
        /// </summary>
        public MovieDetails Details { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Title)}: {Title}";
        }
    }
}