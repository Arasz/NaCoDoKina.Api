using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Models.Cinemas;

namespace Infrastructure.Services
{
    /// <summary>
    /// Business logic for cinemas 
    /// </summary>
    public interface ICinemaService
    {
        /// <summary>
        /// Returns list of nearest cinemas which plays movie with given id 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="searchArea"> RegisterUser searchArea </param>
        /// <returns> List of cinemas in user searchArea that play given movie </returns>
        Task<ICollection<Cinema>> GetCinemasPlayingMovieInSearchArea(long movieId, SearchArea searchArea);

        /// <summary>
        /// Returns list of cinemas in search area 
        /// </summary>
        /// <param name="searchArea"> User search area </param>
        /// <returns> List of nearest cinemas </returns>
        Task<ICollection<Cinema>> GetCinemasInSearchAreaAsync(SearchArea searchArea);

        /// <summary>
        /// Adds new cinema 
        /// </summary>
        /// <param name="cinema"></param>
        /// <returns></returns>
        Task<Cinema> AddCinemaAsync(Cinema cinema);

        /// <summary>
        /// Returns cinema with given id 
        /// </summary>
        /// <param name="id"> Cinema id </param>
        /// <returns> Cinema </returns>
        Task<Cinema> GetCinemaAsync(long id);

        /// <summary>
        /// Returns cinema with given name 
        /// </summary>
        /// <param name="name"> Cinema name </param>
        /// <returns> Cinema </returns>
        Task<Cinema> GetCinemaAsync(string name);
    }
}