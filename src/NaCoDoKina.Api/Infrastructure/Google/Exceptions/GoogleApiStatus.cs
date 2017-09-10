namespace NaCoDoKina.Api.Infrastructure.Google.Exceptions
{
    public enum GoogleApiStatus
    {
        Ok,
        NotFound,
        ZeroResults,
        InvalidRequest,
        RequestDenied,
        OverQueryLimit,
        UnknownError,
        MaxRouteLengthExceeded,
        MaxWaypointsExceeded,
        Unspecifed
    }
}