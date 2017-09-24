using Microsoft.AspNetCore.Identity;
using NaCoDoKina.Api.Infrastructure.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    public class SignInManagerAdapter : ISignInManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInManagerAdapter(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public Task<SignInResult> PasswordSignInAsync(string username, string password)
        {
            return _signInManager.PasswordSignInAsync(username, password, true, false);
        }

        public async Task CreateUserClaims(ApplicationUser user)
        {
            _signInManager.Context.User = await _signInManager.ClaimsFactory.CreateAsync(user);
        }

        public Task SignInAsync(ApplicationUser user)
        {
            return _signInManager.SignInAsync(user, true);
        }

        public ClaimsPrincipal CurrentContextUser => _signInManager.Context.User;
    }
}