using NaCoDoKina.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Business logic contract for shows 
    /// </summary>
    public interface IMovieService
    {
        /// <summary>
        /// Get all movies available near given inside search area. 
        /// </summary>
        /// <param name="searchArea"> RegisterUser location and searched distance </param>
        /// <returns> Shows ids sorted by estimated user rating </returns>
        Task<IEnumerable<long>> GetAllMoviesAsync(SearchArea searchArea);

        /// <summary>
        /// Get movies basic information 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Basic show information </returns>
        Task<Movie> GetMovieAsync(long id);

        /// <summary>
        /// Mark movies as deleted 
        /// </summary>
        /// <param name="id"> Movie id </param>
        Task DeleteMovieAsync(long id);

        /// <summary>
        /// Get detailed information about movie 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Detailed information about show </returns>
        Task<MovieDetails> GetMovieDetailsAsync(long id);

        /// <summary>
        /// Adds new movie 
        /// </summary>
        /// <param name="newMovie"> New movie </param>
        /// <returns> Movie id </returns>
        Task<long> AddMovieAsync(Movie newMovie);

        /// <summary>
        /// Add details for movie 
        /// </summary>
        /// <param name="movieDetails"> Details </param>
        /// <returns> Movie id </returns>
        Task<long> AddMovieDetailsAsync(MovieDetails movieDetails);
    }
}