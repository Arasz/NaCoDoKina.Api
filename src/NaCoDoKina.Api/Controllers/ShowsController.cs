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
    public class ShowsController : Controller
    {
        private readonly ILogger<ShowsController> _logger;
        private readonly IShowService _showService;

        public ShowsController(IShowService showService, ILogger<ShowsController> logger)
        {
            _logger = logger;
            _showService = showService ?? throw new ArgumentNullException(nameof(showService));
        }

        /// <summary>
        /// Returns all accessible shows for current user. Shows are sorted by predicted user rating. 
        /// </summary>
        /// <returns> Shows ids sorted by estimated user rating </returns>
        [ProducesResponseType(typeof(IEnumerable<long>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAllShowsAsync([FromBody]Location location)
        {
            if (location is null)
                return BadRequest();

            try
            {
                var shows = await _showService.GetAllShowsAsync(location);
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
        /// <param name="id"> Show id </param>
        /// <returns> Basic informations about show </returns>
        [ProducesResponseType(typeof(Show), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowAsync(long id)
        {
            try
            {
                var show = await _showService.GetShowAsync(id);
                return Ok(show);
            }
            catch (ShowNotFoundException exception)
            {
                _logger.LogWarning("Show with {id} was not found", id);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Deletes (hides) show for current user 
        /// </summary>
        /// <param name="id"> Show id </param>
        /// <returns> Basic informations about show </returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShowAsync(long id)
        {
            try
            {
                await _showService.DeleteShowAsync(id);
                return Ok();
            }
            catch (ShowNotFoundException exception)
            {
                _logger.LogWarning("Show with {id} was not found", id);
                return NotFound(exception.Message);
            }
        }

        /// <summary>
        /// Returns detailed information about show 
        /// </summary>
        /// <param name="id"> Show id </param>
        /// <returns> Detailed informations about show </returns>
        [ProducesResponseType(typeof(ShowDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetShowDetailsAsync(long id)
        {
            try
            {
                var show = await _showService.GetShowDetailsAsync(id);
                return Ok(show);
            }
            catch (ShowDetailsNotFoundException exception)
            {
                _logger.LogWarning("Show details for show with {id} were not found", id);
                return NotFound(exception.Message);
            }
        }
    }
}