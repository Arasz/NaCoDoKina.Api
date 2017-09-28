using NaCoDoKina.Api.Entities.Movies;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Returns all show times for movie 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="laterThan"> Minimal show time </param>
        /// <returns> List of movie show times played in all cinemas </returns>
        Task<IEnumerable<MovieShowtime>> GetMovieShowtimesAsync(long movieId, DateTime laterThan);

        /// <summary>
        /// Returns all show times for movie in given cinema 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="cinemaId"> Cinema id </param>
        /// <param name="laterThan"> Minimal show time </param>
        /// <returns> List of movie show times played in given cinema </returns>
        Task<IEnumerable<MovieShowtime>> GetMovieShowtimesForCinemaAsync(long movieId, long cinemaId, DateTime laterThan);
    }
}