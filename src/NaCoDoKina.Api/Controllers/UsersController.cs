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
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper, ILogger<UsersController> logger)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Creates new registerUser 
        /// </summary>
        /// <param name="registerUser"> New user data </param>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUser registerUser)
        {
            using (_logger.BeginScope(nameof(CreateUser)))
            {
                _logger.LogDebug("Map to user");

                var user = _mapper.Map<User>(registerUser);

                _logger.LogDebug("User mapped {@user}", user);

                var result = await _userService.CreateUserWithPasswordAsync(user, registerUser.Password);

                _logger.LogDebug("User created with result {@result}", result);

                if (result.Succeeded)
                {
                    var userCredentials = _mapper.Map<Credentials>(user);
                    return CreatedAtAction(nameof(AuthController.GetTokenForUser), null, nameof(AuthController), userCredentials);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, result.FailReason);
            }
        }
    }
}