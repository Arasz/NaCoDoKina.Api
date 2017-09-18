using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Identity;
using NaCoDoKina.Api.Infrastructure.Services.Token;
using NaCoDoKina.Api.Results;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly ILogger<IIdentityService> _logger;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService, ILogger<IIdentityService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Result<AuthenticationToken>> LoginAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");

                var userId = GetCurrentUserId();

                var userInformation = new UserInformation
                {
                    Email = userName,
                    Id = userId.ToString(),
                };
                var token = _tokenService.CreateToken(userInformation);

                return Result<AuthenticationToken>.CreateSucceeded(token);
            }

            _logger.LogInformation("Invalid login attempt.");
            return Result<AuthenticationToken>.CreateFailed("Invalid login attempt.");
        }

        public async Task<Result> CreateUserAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation("User created a new account with password.");

                return Result.CreateSucceeded();
            }

            _logger.LogInformation("User {@user} can not create account", user);
            return Result.CreateFailed();
        }

        public bool VerifyPassword(ApplicationUser user, string password)
        {
            var result = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public Task<ApplicationUser> GetUserByNameAsync(string username) => GetUserByNameAsync(username);

        public long GetCurrentUserId()
        {
            var currentUser = _signInManager.Context.User;
            var userIdString = _userManager.GetUserId(currentUser);
            return long.Parse(userIdString);
        }
    }
}