using System;
using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Exceptions
{
    /// <inheritdoc/>
    /// <summary>
    /// Base exception type for api 
    /// </summary>
    public class NaCoDoKinaApiException : Exception
    {
        public NaCoDoKinaApiException()
        {
        }

        public NaCoDoKinaApiException(string message) : base(message)
        {
        }

        public NaCoDoKinaApiException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NaCoDoKinaApiException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}