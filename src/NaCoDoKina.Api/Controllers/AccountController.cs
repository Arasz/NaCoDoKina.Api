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
    [Authorize]
    [Route("v1/[controller]")]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public AccountController(IIdentityService identityService, IMapper mapper, ILogger<AccountController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        /// <summary>
        /// Login user and return authentication token 
        /// </summary>
        /// <param name="user"> User login data </param>
        /// <returns> Authentication token </returns>
        [HttpPost("token")]
        [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _identityService.LoginAsync(user.Email, user.Password);

            if (!result.Succeeded)
                return Unauthorized();

            var token = result.Data;
            return Ok(_mapper.Map<JwtToken>(token));
        }

        /// <summary>
        /// Register new user 
        /// </summary>
        /// <param name="registerUser"> Register user data </param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUser registerUser)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new ApplicationUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email
            };

            var result = await _identityService.CreateUserAsync(user, registerUser.Password);

            return result.Succeeded ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}