﻿using System;

namespace NaCoDoKina.Api.Models.Authentication
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