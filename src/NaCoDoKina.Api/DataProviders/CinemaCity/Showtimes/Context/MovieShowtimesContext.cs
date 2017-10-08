using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.Entities.Cinemas;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Context
{
    /// <summary>
    /// Context for movie showtime builder 
    /// </summary>
    public class MovieShowtimesContext : IEntityBuilderContext
    {
        public MovieShowtimesContext(Cinema cinema, DateTime showtimeDate)
        {
            Cinema = cinema;
            ShowtimeDate = showtimeDate;
        }

        public Cinema Cinema { get; }

        public DateTime ShowtimeDate { get; }
    }
}