using NaCoDoKina.Api.Entities.Cinemas;
using System;

namespace NaCoDoKina.Api.Entities.Movies
{
    /// <inheritdoc/>
    /// <summary>
    /// Movie showtime 
    /// </summary>
    public class MovieShowtime : Entity
    {
        /// <summary>
        /// Movie show language and presentation type 
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Type of the show, for example 2D, 3D, etc. 
        /// </summary>
        public string ShowType { get; set; }

        /// <summary>
        /// Show date and time 
        /// </summary>
        public DateTime ShowTime { get; set; }

        /// <summary>
        /// Cinema in which a movie is played 
        /// </summary>
        public Cinema Cinema { get; set; }

        /// <summary>
        /// Played movie 
        /// </summary>
        public Movie Movie { get; set; }
    }
}