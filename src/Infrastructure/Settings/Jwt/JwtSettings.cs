namespace Infrastructure.Settings.Jwt
{
    /// <summary>
    /// JSON Web Tokens settings 
    /// </summary>
    /// <see cref="https://jwt.io/"/>
    public class JwtSettings
    {
        /// <summary>
        /// Token issuer 
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Token security key used for hashing 
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Time after token expires, in minutes 
        /// </summary>
        public double ExpiryMinutes { get; set; }
    }
}