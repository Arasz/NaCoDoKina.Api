using System;
using NaCoDoKina.Api.Exceptions;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.Exceptions
{
    public class GoogleApiException : NaCoDoKinaApiException
    {
        public GoogleApiStatus Status { get; set; }

        public GoogleApiException(string status, string message) : base(message)
        {
            Status = Enum.Parse<GoogleApiStatus>(status.Replace("_", ""), true);
        }

        public GoogleApiException(Exception exception) : base(exception.Message, exception)
        {
            Status = GoogleApiStatus.Unspecifed;
        }
    }
}