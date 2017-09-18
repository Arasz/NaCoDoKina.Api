namespace NaCoDoKina.Api.Infrastructure.Services.Token
{
    /// <summary>
    /// Logic for creation of authentication token for user 
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Creates authentication token for user 
        /// </summary>
        /// <param name="userId"> User id </param>
        /// <returns> Authentication token </returns>
        AuthenticationToken CreateToken(string userId);
    }
}