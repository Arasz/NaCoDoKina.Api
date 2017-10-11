using System;

namespace NaCoDoKina.Api.DataContracts.Authentication
{
    /// <summary>
    /// Jwt token 
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// Token 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expiration date 
        /// </summary>
        public DateTime Expires { get; set; }

        public override string ToString()
        {
            return $"{nameof(Token)}: {Token}, {nameof(Expires)}: {Expires}";
        }
    }
}