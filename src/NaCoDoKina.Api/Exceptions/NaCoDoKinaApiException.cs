using System;

namespace NaCoDoKina.Api.Exceptions
{
    /// <inheritdoc/>
    /// <summary>
    /// Base exception type for api 
    /// </summary>
    public class NaCoDoKinaApiException : Exception
    {
        public NaCoDoKinaApiException(string message) : base(message)
        {
        }

        public NaCoDoKinaApiException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}