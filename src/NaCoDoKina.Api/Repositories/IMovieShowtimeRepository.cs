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
        Task<long> CreateMovieShowtimeAsync(MovieShowtime showtime);

        /// <summary>
        /// Adds movie showtimes in one transaction 
        /// </summary>
        /// <param name="showtimes"> Movie showtimes </param>
        /// <returns></returns>
        Task CreateMovieShowtimesAsync(IEnumerable<MovieShowtime> showtimes);

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
        Task<IEnumerable<MovieShowtime>> GetShowtimesForMovieAsync(long movieId, DateTime laterThan);

        /// <summary>
        /// Returns all movie showtimes for cinema 
        /// </summary>
        /// <param name="cinemaId"> Movies are played in this cinema </param>
        /// <param name="laterThan"> Minimal show time </param>
        /// <returns> List of movie show times played in all cinemas </returns>
        Task<IEnumerable<MovieShowtime>> GetShowtimesForCinemaAsync(long cinemaId, DateTime laterThan);

        /// <summary>
        /// Returns all show times for movie in given cinema 
        /// </summary>
        /// <param name="movieId"> Showtimes for this movie are retrieved </param>
        /// <param name="cinemaId"> Movie is played in this cinema </param>
        /// <param name="laterThan"> Minimal show time </param>
        /// <returns> List of movie show times played in given cinema </returns>
        Task<IEnumerable<MovieShowtime>> GetShowtimesForCinemaAndMovieAsync(long movieId, long cinemaId, DateTime laterThan);
    }
}