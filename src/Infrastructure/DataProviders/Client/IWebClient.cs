using ApplicationCore.Results;
using Infrastructure.DataProviders.Requests;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.Client
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
    }
}