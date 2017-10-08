﻿using NaCoDoKina.Api.Entities.Movies;
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
        /// <returns> True if movie was marked as deleted </returns>
        Task<bool> SoftDeleteMovieAsync(long movieId);

        /// <summary>
        /// Deletes movie 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <returns> True if movie was deleted </returns>
        Task<bool> DeleteMovieAsync(long movieId);

        /// <summary>
        /// Gets basic movie information 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Movie </returns>
        Task<Movie> GetMovieAsync(long id);

        /// <summary>
        /// Gets basic movie information by external id 
        /// </summary>
        /// <param name="id"> Id in external system </param>
        /// <returns> Movie </returns>
        Task<Movie> GetMovieByExternalIdAsync(string id);

        /// <summary>
        /// Gets movies played in cinema after date specified in parameter 
        /// </summary>
        /// <param name="cinemaId"> Cinema movieId </param>
        /// <param name="laterThan"> Earlier show time </param>
        /// <returns> Movies ids </returns>
        Task<IEnumerable<long>> GetMoviesIdsPlayedInCinemaAsync(long cinemaId, DateTime laterThan);

        /// <summary>
        /// Gets movie details 
        /// </summary>
        /// <param name="id"> Movie details movieId (equal to movie movieId) </param>
        /// <returns> Movie details </returns>
        Task<MovieDetails> GetMovieDetailsAsync(long id);

        /// <summary>
        /// Creates movies 
        /// </summary>
        /// <param name="movies"></param>
        /// <returns></returns>
        Task CreateMoviesAsync(IEnumerable<Movie> movies);

        /// <summary>
        /// Adds movie 
        /// </summary>
        /// <param name="newMovie"></param>
        /// <returns> Movie movieId </returns>
        Task<long> CreateMovieAsync(Movie newMovie);

        /// <summary>
        /// Adds movie details 
        /// </summary>
        /// <param name="movieDetails"></param>
        /// <returns> Movie details movieId (equal to movie movieId) </returns>
        Task<long> CreateMovieDetailsAsync(MovieDetails movieDetails);
    }
}