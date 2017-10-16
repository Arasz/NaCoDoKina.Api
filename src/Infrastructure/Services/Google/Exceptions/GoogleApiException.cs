using Infrastructure.Exceptions;
using System;

namespace Infrastructure.Services.Google.Exceptions
{
    public class GoogleApiException : NaCoDoKinaApiException
    {
        public string Request { get; set; }

        public GoogleApiStatus Status { get; set; }

        public GoogleApiException(string request, string status, string message) : base(message)
        {
            Request = request;
            Status = Enum.Parse<GoogleApiStatus>(status.Replace("_", ""), true);
        }

        public GoogleApiException(string request, Exception exception) : base(exception.Message, exception)
        {
            Request = request;
            Status = GoogleApiStatus.Unspecifed;
        }
    }
}