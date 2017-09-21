﻿using System;

namespace NaCoDoKina.Api.Infrastructure.Services.Token
{
    /// <summary>
    /// Authentication token 
    /// </summary>
    public class AuthenticationToken
    {
        /// <summary>
        /// Token 
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expiration date 
        /// </summary>
        public DateTime Expires { get; set; }
    }
}