using Hangfire.Dashboard;

namespace HangfireHost.AuthorizationFilters
{
    public class AlwaysAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}