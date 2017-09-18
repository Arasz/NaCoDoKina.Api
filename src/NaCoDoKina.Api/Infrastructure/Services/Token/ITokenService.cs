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
        /// <param name="userInformation"> User information from a token is generated </param>
        /// <returns> Authentication token </returns>
        AuthenticationToken CreateToken(UserInformation userInformation);
    }
}