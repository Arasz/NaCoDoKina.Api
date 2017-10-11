using System;
using Infrastructure.Exceptions;

namespace Infrastructure.Services.Google.Exceptions
{
    public class GoogleApiException : NaCoDoKinaApiException
    {
        public GoogleApiStatus Status { get; set; }

        public GoogleApiException(string status, string userName) : base(userName)
        {
            Status = Enum.Parse<GoogleApiStatus>(status.Replace("_", ""), true);
        }

        public GoogleApiException(Exception exception) : base(exception.Message, exception)
        {
            Status = GoogleApiStatus.Unspecifed;
        }
    }
}