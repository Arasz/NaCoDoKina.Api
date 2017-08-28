using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Controllers
{
    [Route("v1/[controller]")]
    public class ShowsController : Controller
    {
        /// <summary>
        /// Returns all accessible shows 
        /// </summary>
        /// <returns> List of shows ids </returns>
        [ProducesResponseType(typeof(IEnumerable<long>), StatusCodes.Status202Accepted)]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            throw new NotImplementedException();
        }


    }
}