using NaCoDoKina.Api.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    /// <summary>
    /// Cinema repository 
    /// </summary>
    public interface ICinemaRepository
    {
        /// <summary>
        /// Returns all cinemas in which given movie is played 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <returns> Collection of cinemas </returns>
        Task<IEnumerable<Cinema>> GetAllCinemasForMovieAsync(long movieId);

        /// <summary>
        /// Returns all cinemas 
        /// </summary>
        /// <returns> All existing cinemas </returns>
        Task<IEnumerable<Cinema>> GetAllCinemas();

        /// <summary>
        /// Adds cinema 
        /// </summary>
        /// <param name="cinema"> New cinema </param>
        /// <returns> Added cinema </returns>
        Task<Cinema> AddCinema(Cinema cinema);

        /// <summary>
        /// Gets cinema by id 
        /// </summary>
        /// <param name="id"> Cinema name </param>
        /// <returns> Cinema </returns>
        Task<Cinema> GetCinemaAsync(long id);

        /// <summary>
        /// Gets cinema by name 
        /// </summary>
        /// <param name="name"> Cinema name </param>
        /// <returns> Cinema </returns>
        Task<Cinema> GetCinemaAsync(string name);
    }
}