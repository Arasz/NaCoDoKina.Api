using System;

namespace NaCoDoKina.Api.DataContracts.Movies
{
    /// <summary>
    /// Movie show times for cinema 
    /// </summary>
    public class MovieShowtime
    {
        /// <summary>
        /// Link to booking page 
        /// </summary>
        public string BookingLink { get; set; }

        /// <summary>
        /// Are tickets for showtime available 
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Movie show language and presentation type 
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// Type of the show, for example 2D, 3D, etc. 
        /// </summary>
        public string ShowType { get; set; }

        /// <summary>
        /// Cinema in which the movie is played 
        /// </summary>
        public long CinemaId { get; set; }

        /// <summary>
        /// Played movie id 
        /// </summary>
        public long MovieId { get; set; }

        /// <summary>
        /// Show times 
        /// </summary>
        public DateTime ShowTime { get; set; }

        public override string ToString()
        {
            return $"{nameof(Language)}: {Language}, {nameof(ShowType)}: {ShowType}, {nameof(CinemaId)}: {CinemaId}, {nameof(MovieId)}: {MovieId}, {nameof(ShowTime)}: {ShowTime}";
        }
    }
}