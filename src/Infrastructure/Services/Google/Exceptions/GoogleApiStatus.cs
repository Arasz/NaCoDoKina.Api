namespace Infrastructure.Services.Google.Exceptions
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