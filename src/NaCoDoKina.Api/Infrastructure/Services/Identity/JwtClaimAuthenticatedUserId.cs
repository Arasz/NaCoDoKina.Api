using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    public class JwtClaimAuthenticatedUserId : IAuthenticatedUserId
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<JwtClaimAuthenticatedUserId> _logger;
        public long Id => GetUserId();

        private readonly HashSet<string> _claimsWithId = new HashSet<string>
        {
            JwtRegisteredClaimNames.UniqueName,
            ClaimTypes.NameIdentifier,
            JwtRegisteredClaimNames.Sub
        };

        private long GetUserId()
        {
            using (_logger.BeginScope(nameof(GetUserId)))
            {
                var httpContext = _httpContextAccessor.HttpContext;
                var idClaim = httpContext.User
                    .FindFirst(claim => _claimsWithId.Contains(claim.Type));

                _logger.LogDebug("Founded claim {@claim}", idClaim);

                if (idClaim != null && long.TryParse(idClaim.Value, out var id))
                    return id;

                var allClaims = httpContext.User.Claims
                    .Select(claim => new { claim.Type, claim.Value });
                _logger.LogWarning("For claim {@claim} value cannot be parsed to long or claim is null, all claims {@claims} ", idClaim, allClaims);

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