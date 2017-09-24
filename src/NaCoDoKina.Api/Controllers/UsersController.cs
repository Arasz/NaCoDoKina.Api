using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataContracts.Authentication;
using NaCoDoKina.Api.Models;
using NaCoDoKina.Api.Services;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Controllers
{
    [Route("v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, ITokenService tokenService, IMapper mapper, ILogger<UsersController> logger)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Authenticates registerUser and returns token 
        /// </summary>
        /// <param name="credentials"> RegisterUser login data </param>
        /// <returns> Authentication token </returns>
        [HttpPost("token")]
        [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetTokenForUser([FromBody]Credentials credentials)
        {
            using (_logger.BeginScope(nameof(GetTokenForUser)))
            {
                _logger.LogDebug("Model validation, state {@modelState}", ModelState);

                if (!ModelState.IsValid)
                    return BadRequest();

                _logger.LogDebug("RegisterUser mapping");

                var user = _mapper.Map<User>(credentials);

                _logger.LogDebug("RegisterUser mapped {@registerUser}", user);

                _logger.LogDebug("Authentication");

                var result = await _userService.Authenticate(user, credentials.Password);

                _logger.LogDebug("Authentication result {@result}", result);

                if (!result.Succeeded)
                    return Unauthorized();

                _logger.LogDebug("Create token");

                var token = _tokenService.CreateToken(user);

                _logger.LogDebug("Token created {@token}", token);

                return Ok(_mapper.Map<JwtToken>(token));
            }
        }

        /// <summary>
        /// Creates new registerUser 
        /// </summary>
        /// <param name="registerUser"> New user data </param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUser registerUser)
        {
            using (_logger.BeginScope(nameof(CreateUser)))
            {
                _logger.LogDebug("Model validation, state {@modelState}", ModelState);

                if (!ModelState.IsValid)
                    return BadRequest();

                _logger.LogDebug("Map to user");

                var user = _mapper.Map<User>(registerUser);

                _logger.LogDebug("User mapped {@user}", user);

                var result = await _userService.CreateUserWithPasswordAsync(user, registerUser.Password);

                _logger.LogDebug("User created with result {@result}", result);

                return result.Succeeded ? Ok() : StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}