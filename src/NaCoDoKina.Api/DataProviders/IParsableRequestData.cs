using NaCoDoKina.Api.DataProviders.Requests;

namespace NaCoDoKina.Api.DataProviders
{
    public interface IParsableRequestData
    {
        Request Parse();
    }
}