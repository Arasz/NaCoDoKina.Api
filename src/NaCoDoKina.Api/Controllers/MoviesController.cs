﻿using AutoMapper;
using Infrastructure.Exceptions;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataContracts;
using NaCoDoKina.Api.DataContracts.Cinemas;
using NaCoDoKina.Api.DataContracts.Movies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Controllers
{
    [Route("v1/[controller]")]
    //[Authorize]
    public class MoviesController : Controller
    {
        private readonly IMovieShowtimeService _movieShowtimeService;
        private readonly IRatingService _ratingService;
        private readonly IMapper _mapper;
        private readonly ICinemaService _cinemaService;
        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService, ICinemaService cinemaService,
            IRatingService ratingService, ILogger<MoviesController> logger, IMapper mapper,
            IMovieShowtimeService movieShowtimeService)
        {
            _movieShowtimeService = movieShowtimeService ?? throw new ArgumentNullException(nameof(movieShowtimeService));
            _ratingService = ratingService ?? throw new ArgumentNullException(nameof(ratingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _cinemaService = cinemaService ?? throw new ArgumentNullException(nameof(cinemaService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        /// <summary>
        /// Returns all accessible shows for current api user sorted by rating. When user is
        /// anonymous movies are not sorted
        /// </summary>
        /// <returns> Shows ids sorted by estimated user rating </returns>
        [ProducesResponseType(typeof(IEnumerable<long>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAllMoviesAsync([FromQuery]SearchArea searchArea)
        {
            if (searchArea is null || searchArea.Center is null)
                return BadRequest();

            try
            {
                var modelSearchArea = _mapper.Map<Infrastructure.Models.SearchArea>(searchArea);
                var movies = await _movieService.GetAllMoviesAsync(modelSearchArea);
                return Ok(movies);
            }
            catch (MoviesNotFoundException exception)
            {
                _logger.LogWarning("Shows not found in {@searchArea}", searchArea);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns basic information about show 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Basic information about show </returns>
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieAsync(long id)
        {
            try
            {
                var movie = await _movieService.GetMovieAsync(id);
                return Ok(_mapper.Map<Movie>(movie));
            }
            catch (MovieNotFoundException exception)
            {
                _logger.LogWarning("Movie with {id} was not found", id);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Deletes (hides) show for current user 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Basic information about show </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMovieAsync(long id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                return Ok();
            }
            catch (MovieNotFoundException exception)
            {
                _logger.LogWarning("Movie with {id} was not found", id);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns list of cinemas inside search area playing movie with given id 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <param name="searchArea"> User search area </param>
        /// <returns> List of cinemas </returns>
        [ProducesResponseType(typeof(IEnumerable<Cinema>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}/cinemas")]
        public async Task<IActionResult> GetCinemasPlayingMovieInSearchArea(long id, [FromQuery]SearchArea searchArea)
        {
            if (searchArea is null)
                return BadRequest();

            try
            {
                var modelSearchArea = _mapper.Map<Infrastructure.Models.SearchArea>(searchArea);

                var nearestCinemas = await _cinemaService.GetCinemasPlayingMovieInSearchArea(id, modelSearchArea);

                var mappedCinemas = nearestCinemas
                    .Select(_mapper.Map<Cinema>)
                    .ToArray();

                return Ok(mappedCinemas);
            }
            catch (CinemasNotFoundException exception)
            {
                _logger.LogWarning("Cinemas playing movie with id {id} were not " +
                                   "found near {@searchArea} in given proximity", id, searchArea);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns movie showtimes for given cinema. 
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="cinemaId"> Cinema id. </param>
        /// <param name="laterThan">
        /// Minimal movie show time. When not provided current time is used
        /// </param>
        /// <returns> Movie showtimes </returns>
        [ProducesResponseType(typeof(IEnumerable<MovieShowtime>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{movieId}/cinemas/{cinemaId}/showtimes")]
        public async Task<IActionResult> GetMovieShowtimesAsync(long movieId, long cinemaId, [FromQuery]DateTime? laterThan = null)
        {
            try
            {
                var minimalShowTime = laterThan ?? DateTime.Now;

                var showtimes = await _movieShowtimeService.GetMovieShowtimesForCinemaAsync(movieId, cinemaId, minimalShowTime);

                var mappedShowtimes = showtimes
                    .Map<Infrastructure.Models.Movies.MovieShowtime, MovieShowtime>(_mapper)
                    .ToArray();

                return Ok(mappedShowtimes);
            }
            catch (MovieShowtimeNotFoundException exception)
            {
                _logger.LogWarning("Showtime for movie {movieId} was not found", movieId);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns detailed information about show 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Detailed information about show </returns>
        [ProducesResponseType(typeof(MovieDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetMovieDetailsAsync(long id)
        {
            try
            {
                var movieDetails = await _movieService.GetMovieDetailsAsync(id);
                return Ok(_mapper.Map<MovieDetails>(movieDetails));
            }
            catch (MovieDetailsNotFoundException exception)
            {
                _logger.LogWarning("Movie details for show with {id} were not found", id);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Sets user rating for movie 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <param name="rating"> User rating </param>
        /// <returns> Set rating </returns>
        [ProducesResponseType(typeof(double), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{id}/rating")]
        [Authorize]
        public async Task<IActionResult> SetRatingForMovie(long id, [FromBody]double rating)
        {
            var result = await _ratingService.SetMovieRating(id, rating);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetMovieDetailsAsync), id, rating);

            return NotFound(result.FailureReason);
        }
    }
}