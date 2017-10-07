using ApplicationCore.Results;
using NaCoDoKina.Api.DataProviders.Requests;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Client
{
    public interface IWebClient
    {
        /// <summary>
        /// Make request and return response content 
        /// </summary>
        /// <param name="requestData"> Request data </param>
        /// <param name="requestParameters"></param>
        /// <returns> Response content </returns>
        Task<Result<string>> MakeRequestAsync(IParsableRequestData requestData, params IRequestParameter[] requestParameters);

        /// <summary>
        /// Makes get request to given url 
        /// </summary>
        /// <param name="url"> Service request </param>
        /// <returns> Request result with response content </returns>
        Task<Result<string>> MakeGetRequestAsync(string url);
    }
}