using System;

namespace NaCoDoKina.Api.Infrastructure.Google.Exceptions
{
    public class GoogleApiException : Exception
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