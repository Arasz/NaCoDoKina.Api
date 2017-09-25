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
    [AllowAnonymous]
    public class AuthController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, ITokenService tokenService, IMapper mapper, ILogger<AuthController> logger)
        {
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Authenticates user and returns token 
        /// </summary>
        /// <param name="credentials"> RegisterUser login data </param>
        /// <returns> Authentication token </returns>
        [HttpPost("token")]
        [ProducesResponseType(typeof(JwtToken), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTokenForUser([FromBody]Credentials credentials)
        {
            using (_logger.BeginScope(nameof(GetTokenForUser)))
            {
                _logger.LogDebug("User mapping");

                var user = _mapper.Map<User>(credentials);

                _logger.LogDebug("User mapped {@registerUser}", user);

                _logger.LogDebug("Authentication");

                var result = await _userService.AuthenticateAsync(user, credentials.Password);

                _logger.LogDebug("Authentication result {@result}", result);

                if (!result.IsSuccess)
                    return Unauthorized();

                _logger.LogDebug("Create token");

                var token = _tokenService.CreateToken(result.Value);

                _logger.LogDebug("Token created {@token}", token);

                return Ok(_mapper.Map<JwtToken>(token));
            }
        }
    }
}