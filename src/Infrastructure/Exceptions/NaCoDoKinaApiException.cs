using System;

namespace NaCoDoKina.Api.Exceptions
{
    /// <inheritdoc/>
    /// <summary>
    /// Base exception type for api 
    /// </summary>
    public class NaCoDoKinaApiException : Exception
    {
        public NaCoDoKinaApiException(string userName) : base(userName)
        {
        }

        public NaCoDoKinaApiException(string userName, Exception innerException) : base(userName, innerException)
        {
        }
    }
}