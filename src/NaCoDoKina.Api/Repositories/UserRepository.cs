using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<IUserRepository> _logger;

        public UserRepository(UserManager<ApplicationUser> userManager, ILogger<IUserRepository> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> UpdateUserPassword(ApplicationUser user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            _logger.LogDebug("{methodName} with result {@result}", nameof(UpdateUserPassword), result);
            return result.Succeeded;
        }

        public async Task<bool> IncrementAccessFailed(ApplicationUser user)
        {
            var result = await _userManager.AccessFailedAsync(user);
            _logger.LogDebug("{methodName} with result {@result}", nameof(IncrementAccessFailed), result);
            return result.Succeeded;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public Task<ApplicationUser> GetUserByIdAsync(long userId)
        {
            return _userManager.FindByIdAsync(userId.ToString());
        }

        public Task<ApplicationUser> GetUserByNameAsync(string userName)
        {
            return _userManager.Users
                .SingleOrDefaultAsync(user => user.UserName == userName);
        }

        public Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return _userManager.Users
                .SingleOrDefaultAsync(user => user.Email == email);
        }

        public async Task<bool> CreateUserWithPasswordAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }
    }
}