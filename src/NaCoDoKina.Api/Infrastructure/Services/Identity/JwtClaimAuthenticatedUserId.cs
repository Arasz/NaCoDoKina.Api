using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    public class JwtClaimAuthenticatedUserId : IAuthenticatedUserId
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<JwtClaimAuthenticatedUserId> _logger;
        public long Id => GetUserId();

        private long GetUserId()
        {
            using (_logger.BeginScope(nameof(GetUserId)))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var uniqueNameClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.UniqueName);

                if (uniqueNameClaim is null)
                    uniqueNameClaim = httpContext.User.FindFirst(JwtRegisteredClaimNames.Sub);

                _logger.LogDebug("Founded claim {@claim}", uniqueNameClaim);

                if (uniqueNameClaim != null && long.TryParse(uniqueNameClaim.Value, out var id))
                    return id;

                var allClaims = httpContext.User.Claims
                    .Select(claim => new { claim.Type, claim.Value });
                _logger.LogWarning("For claim {@claim} value cannot be parsed to long or claim is null, all claims {@claims} ", uniqueNameClaim, allClaims);

                return 0;
            }
        }

        public JwtClaimAuthenticatedUserId(IHttpContextAccessor httpContext, ILogger<JwtClaimAuthenticatedUserId> logger)
        {
            _httpContextAccessor = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}