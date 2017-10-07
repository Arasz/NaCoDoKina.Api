using NaCoDoKina.Api.DataProviders.EntityBuilder;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Context
{
    /// <summary>
    /// Context for movie showtime builder 
    /// </summary>
    public class MovieShowtimesContext : IEntityBuilderContext
    {
        public MovieShowtimesContext(string cinemaId, DateTime showtimeDate)
        {
            CinemaId = cinemaId;
            ShowtimeDate = showtimeDate;
        }

        public string CinemaId { get; }

        public DateTime ShowtimeDate { get; }
    }
}