using ApplicationCore.Entities.Cinemas;
using System;

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
        /// Hash of movie and cinema id with show time. Used for optimization 
        /// </summary>
        public int HashId { get; set; }

        /// <summary>
        /// Played movie 
        /// </summary>
        public Movie Movie { get; set; }

        /// <summary>
        /// Calculates hash id for this showtime. 
        /// </summary>
        public void CalculateHashId()
        {
            if (Cinema is null)
                throw new ArgumentNullException(nameof(Cinema));
            if (Movie is null)
                throw new ArgumentNullException(nameof(Movie));

            unchecked
            {
                var hashCode = GetHashCode();
                hashCode = (hashCode * 397) ^ ShowTime.GetHashCode();
                hashCode = (hashCode * 397) ^ Cinema.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ Movie.Id.GetHashCode();
                HashId = hashCode;
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(Language)}: {Language}, {nameof(ShowType)}: {ShowType}, {nameof(ShowTime)}: {ShowTime}, {nameof(Cinema)}: {Cinema}, {nameof(Movie)}: {Movie}";
        }
    }
}