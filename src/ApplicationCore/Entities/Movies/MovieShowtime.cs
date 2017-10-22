using ApplicationCore.Entities.Cinemas;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Entities.Movies
{
    /// <inheritdoc/>
    /// <summary>
    /// Movie showtime 
    /// </summary>
    public class MovieShowtime : Entity
    {
        /// <summary>
        /// Showtime id in external service 
        /// </summary>
        public string ExternalId { get; set; }

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
        /// Link to booking page 
        /// </summary>
        public string BookingLink { get; set; }

        /// <summary>
        /// Are tickets for showtime available 
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Played movie 
        /// </summary>
        public Movie Movie { get; set; }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Language)}: {Language}, {nameof(ShowType)}: {ShowType}, {nameof(ShowTime)}: {ShowTime}, {nameof(Cinema)}: {Cinema}, {nameof(Movie)}: {Movie}";
        }

        private sealed class ExternalIdEqualityComparer : IEqualityComparer<MovieShowtime>
        {
            public bool Equals(MovieShowtime x, MovieShowtime y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.ExternalId, y.ExternalId);
            }

            public int GetHashCode(MovieShowtime obj)
            {
                return (obj.ExternalId != null ? obj.ExternalId.GetHashCode() : 0);
            }
        }

        public static IEqualityComparer<MovieShowtime> ExternalIdComparer { get; } = new ExternalIdEqualityComparer();
    }
}