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
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ILogger<IIdentityService> _logger;
        private readonly ITokenService _tokenService;
        private readonly ISignInManager _signInManager;
        private readonly IUserManager _userManager;

        public IdentityService(IUserManager userManager, ISignInManager signInManager, ITokenService tokenService, IPasswordHasher<ApplicationUser> passwordHasher, ILogger<IIdentityService> logger)
        {
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<Result<AuthenticationToken>> LoginAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password);
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

                await _signInManager.SignInAsync(user);
                _logger.LogInformation("User created a new account with password.");

                return Result.CreateSucceeded();
            }

            _logger.LogInformation("User {@user} can not create account", user);
            return Result.CreateFailed();
        }

        public bool VerifyPassword(ApplicationUser user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public Task<ApplicationUser> GetUserByNameAsync(string username)
        {
            return _userManager.GetUserByNameAsync(username);
        }

        public long GetCurrentUserId()
        {
            var currentUser = _signInManager.CurrentContextUser;
            var userIdString = _userManager.GetUserId(currentUser);
            return long.Parse(userIdString);
        }
    }
}