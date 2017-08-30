using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NaCoDoKina.Api.DataContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Controllers
{
    [Route("v1/[controller]")]
    public class ShowsController : Controller
    {
        /// <summary>
        /// Returns all accessible shows for current user. Shows are sorted by predicted user rating. 
        /// </summary>
        /// <returns> Sorted list of shows ids </returns>
        [ProducesResponseType(typeof(IEnumerable<long>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromBody]Location location)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns basic informations about show 
        /// </summary>
        /// <param name="id"> Show id </param>
        /// <returns> Basic informations about show </returns>
        [ProducesResponseType(typeof(Show), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetShowAsync(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes (hides) show for current user 
        /// </summary>
        /// <param name="id"> Show id </param>
        /// <returns> Basic informations about show </returns>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShowAsync(long id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns detailed information about show 
        /// </summary>
        /// <param name="id"> Show id </param>
        /// <returns> Detailed informations about show </returns>
        [ProducesResponseType(typeof(ShowDetails), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetShowDetailsAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}