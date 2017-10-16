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

        private sealed class PreCreateEqualityComparer : IEqualityComparer<MovieShowtime>
        {
            public bool Equals(MovieShowtime x, MovieShowtime y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return string.Equals(x.ShowType, y.ShowType) && x.ShowTime.Equals(y.ShowTime) && Equals(x.Cinema, y.Cinema) && Equals(x.Movie, y.Movie);
            }

            public int GetHashCode(MovieShowtime obj)
            {
                unchecked
                {
                    var hashCode = (obj.ShowType != null ? obj.ShowType.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ obj.ShowTime.GetHashCode();
                    hashCode = (hashCode * 397) ^ (obj.Cinema != null ? obj.Cinema.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (obj.Movie != null ? obj.Movie.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        /// <summary>
        /// Equality comparer used to checking if MovieShowtime is not duplicated 
        /// </summary>
        public static IEqualityComparer<MovieShowtime> PreCreateComparer { get; } = new PreCreateEqualityComparer();
    }
}