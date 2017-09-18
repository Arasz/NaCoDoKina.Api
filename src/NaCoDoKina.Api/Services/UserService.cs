using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Controllers;
using NaCoDoKina.Api.Exceptions;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Email;
using NaCoDoKina.Api.Infrastructure.Services.Token;
using NaCoDoKina.Api.Models.Authentication;
using System;
using System.Threading.Tasks;
using AuthenticationToken = NaCoDoKina.Api.Models.Authentication.AuthenticationToken;

namespace NaCoDoKina.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            ITokenService tokenService,
            IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        private async Task<AuthenticationToken> CreateUserTokenAsync()
        {
            var userId = await GetCurrentUserIdAsync();
            var token = _tokenService.CreateToken(userId.ToString());
            return _mapper.Map<AuthenticationToken>(token);
        }

        public Task<long> GetCurrentUserIdAsync()
        {
            throw new System.NotImplementedException();
        }

        public async Task<AuthenticationToken> LoginAsync(UserData userData)
        {
            var result = await _signInManager.PasswordSignInAsync(userData.Email, userData.Password, false, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");

                return await CreateUserTokenAsync();
            }

            _logger.LogInformation("Invalid login attempt.");
            throw new InvalidLoginAttemptException();
        }

        public async Task RegisterAsync(UserData userData)
        {
            var user = new ApplicationUser
            {
                UserName = userData.Email,
                Email = userData.Email
            };

            var result = await _userManager.CreateAsync(user, userData.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation("User created a new account with password.");
            }

            _logger.LogInformation("User {@user} can not create account", user);
        }
    }
}