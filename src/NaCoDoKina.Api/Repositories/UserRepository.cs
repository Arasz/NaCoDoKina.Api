using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NaCoDoKina.Api.Infrastructure.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task UpdateUserPassword(ApplicationUser user, string oldPassword, string newPassword)
        {
            await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task IncrementAccessFailed(ApplicationUser user)
        {
            await _userManager.AccessFailedAsync(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }

        public Task<ApplicationUser> GetUserByLoginProviderAsync(string loginProvider, string key)
        {
            return _userManager.FindByLoginAsync(loginProvider, key);
        }

        public Task<ApplicationUser> GetUserByIdAsync(string userId)
        {
            return _userManager.FindByIdAsync(userId);
        }

        public Task<ApplicationUser> GetUserByNameAsync(string userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CreateUserWithPasswordAsync(ApplicationUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }
    }
}