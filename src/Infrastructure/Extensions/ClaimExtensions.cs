using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Infrastructure.Extensions
{
    public static class ClaimExtensions
    {
        public static void LogDebugInfo(this Claim claim, ILogger logger, string message)
        {
            logger.LogDebug(message + " {Issuer}, {Type}, {Value}, {ValueType}, {OriginalIssuer}, {SubjectName}, {SubjectIsAuthenticated}",
                claim.Issuer, claim.Type, claim.Value, claim.ValueType, claim.OriginalIssuer, claim.Subject?.Name, claim.Subject?.IsAuthenticated);
        }
    }
}