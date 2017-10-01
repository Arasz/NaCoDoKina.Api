using System.Threading.Tasks;
using ApplicationCore.Results;

namespace NaCoDoKina.Api.DataProviders
{
    public interface IWebClient
    {
        /// <summary>
        /// Make request and return response content 
        /// </summary>
        /// <param name="requestData"> Request data </param>
        /// <returns> Response content </returns>
        Task<Result<string>> MakeRequestAsync(IParsableRequestData requestData);
    }
}