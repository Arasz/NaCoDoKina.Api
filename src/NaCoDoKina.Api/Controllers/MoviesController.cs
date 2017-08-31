using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataContracts;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Controllers
{
    [Route("v1/[controller]")]
    public class MoviesController : Controller
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService, ILogger<MoviesController> logger)
        {
            _logger = logger;
            _movieService = movieService ?? throw new ArgumentNullException(nameof(movieService));
        }

        /// <summary>
        /// Returns all accessible shows for current user. Shows are sorted by predicted user rating. 
        /// </summary>
        /// <returns> Shows ids sorted by estimated user rating </returns>
        [ProducesResponseType(typeof(IEnumerable<long>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAllMoviesAsync([FromBody]Location location)
        {
            if (location is null)
                return BadRequest();

            try
            {
                var shows = await _movieService.GetAllMoviesAsync(location);
                return Ok(shows);
            }
            catch (ShowsNotFoundException exception)
            {
                _logger.LogWarning("Shows not found in @location.", location);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns basic informations about show 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Basic informations about show </returns>
        [ProducesResponseType(typeof(Movie), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieAsync(long id)
        {
            try
            {
                var show = await _movieService.GetMovieAsync(id);
                return Ok(show);
            }
            catch (ShowNotFoundException exception)
            {
                _logger.LogWarning("Movie with {id} was not found", id);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Deletes (hides) show for current user 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Basic informations about show </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(long id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                return Ok();
            }
            catch (ShowNotFoundException exception)
            {
                _logger.LogWarning("Movie with {id} was not found", id);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns detailed information about show 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <returns> Detailed informations about show </returns>
        [ProducesResponseType(typeof(MovieDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetMovieDetailsAsync(long id)
        {
            try
            {
                var show = await _movieService.GetMovieDetailsAsync(id);
                return Ok(show);
            }
            catch (ShowDetailsNotFoundException exception)
            {
                _logger.LogWarning("Movie details for show with {id} were not found", id);
                return NotFound(exception.Message);
            }
        }
    }
}