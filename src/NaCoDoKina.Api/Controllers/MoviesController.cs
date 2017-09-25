using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataContracts.Movies;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Controllers
{
    [Route("v1/[controller]")]
    [Authorize]
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
        /// Returns all accessible shows for current user. Shows are sorted by predicted user rating. 
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
                var modelSearchArea = _mapper.Map<Models.SearchArea>(searchArea);
                var movies = await _movieService.GetAllMoviesAsync(modelSearchArea);
                return Ok(movies);
            }
            catch (MoviesNotFoundException exception)
            {
                _logger.LogWarning("Shows not found in @searchArea.", searchArea);
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
            catch (MovieNotFoundException exception)
            {
                _logger.LogWarning("Movie with {id} was not found", id);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns list of nearest cinemas which plays movie with given id 
        /// </summary>
        /// <param name="id"> Movie id </param>
        /// <param name="searchArea"> RegisterUser searchArea </param>
        /// <returns> Detailed informations about show </returns>
        [ProducesResponseType(typeof(IEnumerable<Cinema>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}/cinemas")]
        public async Task<IActionResult> GetNearestCinemasForMovie(long id, SearchArea searchArea)
        {
            if (searchArea is null)
                return BadRequest();

            try
            {
                var modelSearchArea = _mapper.Map<Models.SearchArea>(searchArea);
                var nearestCinemas = await _cinemaService.GetNearestCinemasForMovieAsync(id, modelSearchArea);
                return Ok(nearestCinemas.Select(_mapper.Map<Location>));
            }
            catch (CinemasNotFoundException exception)
            {
                _logger.LogWarning("Cinemas playing movie with id {id} were not " +
                                   "found near {@searchArea} in given proximity", id, searchArea);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns movie showtimes for given cinema. If no cinema is given, all showtimes for movie
        /// are returned.
        /// </summary>
        /// <param name="movieId"> Movie id </param>
        /// <param name="cinemaId"> Cinema id. Optional. </param>
        /// <param name="laterThan">
        /// Minimal movie show time. When not provided current time is used
        /// </param>
        /// <returns> Movie showtimes </returns>
        [ProducesResponseType(typeof(IEnumerable<MovieShowtime>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{movieId}/cinemas/{cinemaId?}")]
        public async Task<IActionResult> GetMovieShowtimesAsync(long movieId, long? cinemaId = null, [FromQuery]DateTime? laterThan = null)
        {
            try
            {
                var minimalShowTime = laterThan ?? DateTime.Now;

                if (cinemaId is null)
                {
                    var showtimes = await _movieShowtimeService.GetMovieShowtimesAsync(movieId, minimalShowTime);

                    return Ok(showtimes.Map<Models.MovieShowtime, MovieShowtime>(_mapper));
                }
                else
                {
                    var showtimes = await _movieShowtimeService.GetMovieShowtimesForCinemaAsync(movieId, cinemaId.Value, minimalShowTime);

                    return Ok(showtimes.Map<Models.MovieShowtime, MovieShowtime>(_mapper));
                }
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
        /// <returns> Detailed informations about show </returns>
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

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("{id}/rating")]
        public async Task<IActionResult> SetRatingForMovie(long id, [FromBody]double rating)
        {
            try
            {
                await _ratingService.SetMovieRating(id, rating);
                return CreatedAtAction(nameof(GetMovieDetailsAsync), id, rating);
            }
            catch (MovieNotFoundException movieNotFoundException)
            {
                _logger.LogWarning("Movie with id {id} not found by rating service", id);
                return NotFound(movieNotFoundException.Message);
            }
        }
    }
}