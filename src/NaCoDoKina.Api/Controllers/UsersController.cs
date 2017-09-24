using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Identity;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Controllers
{
    [Route("v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService identityService, IMapper mapper, ILogger<UsersController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        /// <summary>
        /// Authenticates user and returns token 
        /// </summary>
        /// <param name="creditentials"> User login data </param>
        /// <returns> Authentication token </returns>
        [HttpPost("token")]
        [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenForUser([FromBody]Creditentials creditentials)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _identityService.LoginAsync(creditentials.Email, creditentials.Password);

            if (!result.Succeeded)
                return Unauthorized();

            var token = result.Data;
            return Ok(_mapper.Map<JwtToken>(token));
        }

        /// <summary>
        /// Creates new user 
        /// </summary>
        /// <param name="createUser"> New user data </param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUser createUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser
            {
                UserName = createUser.Email,
                Email = createUser.Email
            };

            var result = await _identityService.CreateUserAsync(user, createUser.Password);

            return result.Succeeded ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}