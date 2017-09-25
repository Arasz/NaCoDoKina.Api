namespace NaCoDoKina.Api.Infrastructure.Services.Identity
{
    /// <summary>
    /// Provides currently authenticated user id 
    /// </summary>
    public interface IAuthenticatedUserId
    {
        long Id { get; }
    }
}