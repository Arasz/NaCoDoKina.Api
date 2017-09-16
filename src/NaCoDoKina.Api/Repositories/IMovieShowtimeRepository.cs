using NaCoDoKina.Api.Entities;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public interface IMovieShowtimeRepository
    {
        /// <summary>
        /// Adds movie showtime 
        /// </summary>
        /// <param name="showtime"> Movie showtime </param>
        /// <returns> Movie showtime id </returns>
        Task<long> AddMovieShowtimeAsync(MovieShowtime showtime);

        /// <summary>
        /// Deletes all showtimes before date given in parameter 
        /// </summary>
        /// <param name="limitDateTime"> Date to which showtimes are deleted </param>
        Task DeleteAllBeforeDateAsync(DateTime limitDateTime);
    }
}