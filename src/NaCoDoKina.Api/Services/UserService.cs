using NaCoDoKina.Api.Infrastructure.Services.Identity;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class UserService : IUserService
    {
        private readonly IIdentityService _identityService;

        public UserService(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Task<long> GetCurrentUserIdAsync() => Task.FromResult(_identityService.GetCurrentUserId());
    }
}