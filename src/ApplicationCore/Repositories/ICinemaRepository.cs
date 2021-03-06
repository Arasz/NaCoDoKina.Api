﻿using ApplicationCore.Entities.Cinemas;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Repositories
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
        Task<ICollection<Cinema>> GetAllCinemasForMovieAsync(long movieId);

        /// <summary>
        /// Returns all cinemas 
        /// </summary>
        /// <returns> All existing cinemas </returns>
        Task<ICollection<Cinema>> GetAllCinemas();

        /// <summary>
        /// Creates cinema 
        /// </summary>
        /// <param name="cinema"> New cinema </param>
        /// <returns> Added cinema </returns>
        Task<Cinema> CreateCinemaAsync(Cinema cinema);

        /// <summary>
        /// Gets cinema by id 
        /// </summary>
        /// <param name="id"> Cinema name </param>
        /// <returns> Cinema </returns>
        Task<Cinema> GetCinemaByIdAsync(long id);

        /// <summary>
        /// Gets cinema by name 
        /// </summary>
        /// <param name="name"> Cinema name </param>
        /// <returns> Cinema </returns>
        Task<Cinema> GetCinemaByNameAsync(string name);

        /// <summary>
        /// Creates many cinemas at once 
        /// </summary>
        /// <param name="cinemas"> Cinemas to create </param>
        /// <returns></returns>
        Task CreateCinemasAsync(IEnumerable<Cinema> cinemas);

        /// <summary>
        /// Returns all cinemas from city 
        /// </summary>
        /// <param name="city"> City in which cinemas are located </param>
        /// <returns> Cinemas in city </returns>
        Task<ICollection<Cinema>> GetCinemasByCityAsync(string city);
    }
}