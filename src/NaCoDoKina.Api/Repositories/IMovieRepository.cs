using NaCoDoKina.Api.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public interface IMovieRepository
    {
        /// <summary>
        /// Marks movie as deleted for current user 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="userId"> User id </param>
        Task<bool> DeleteMovieAsync(long movieId, long userId);

        /// <summary>
        /// Gets basic movie information 
        /// </summary>
        /// <param name="id"> Movie movieId </param>
        /// <returns> Movie details movieId </returns>
        Task<Movie> GetMovieAsync(long id);

        /// <summary>
        /// Gets movies played in cinema after date specified in parameter 
        /// </summary>
        /// <param name="cinemaId"> Cinema movieId </param>
        /// <param name="laterThan"> Earlier show time </param>
        /// <returns> Movies ids </returns>
        Task<IEnumerable<long>> GetMoviesPlayedInCinemaAsync(long cinemaId, DateTime laterThan);

        /// <summary>
        /// Gets movie details 
        /// </summary>
        /// <param name="id"> Movie details movieId (equal to movie movieId) </param>
        /// <returns> Movie details </returns>
        Task<MovieDetails> GetMovieDetailsAsync(long id);

        /// <summary>
        /// Adds movie 
        /// </summary>
        /// <param name="newMovie"></param>
        /// <returns> Movie movieId </returns>
        Task<long> AddMovieAsync(Movie newMovie);

        /// <summary>
        /// Adds movie details 
        /// </summary>
        /// <param name="movieDetails"></param>
        /// <returns> Movie details movieId (equal to movie movieId) </returns>
        Task<long> AddMovieDetailsAsync(MovieDetails movieDetails);
    }
}