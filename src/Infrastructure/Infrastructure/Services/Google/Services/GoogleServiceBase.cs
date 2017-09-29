using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common.Response;
using NaCoDoKina.Api.Infrastructure.Services.Google.Exceptions;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.Services
{
    public abstract class GoogleServiceBase<TRequest, TResponse> : BaseHttpApiClient
        where TRequest : GoogleApiRequest
        where TResponse : GoogleApiResponse

    {
        private readonly IRequestParser<TRequest> _requestParser;
        protected readonly string ApiKey;
        protected ILogger<BaseHttpApiClient> Logger { get; }

        private string _outputFormat = "json";

        protected override string CreateRequestUrl(string parsedRequest) =>
            $"{BaseUrl}{_outputFormat}?{parsedRequest}";

        protected GoogleServiceBase(GoogleServiceDependencies<TRequest> googleServiceDependencies)
            : base(googleServiceDependencies.HttpClient)
        {
            Logger = googleServiceDependencies.Logger;
            _requestParser = googleServiceDependencies.RequestParser;
            ApiKey = googleServiceDependencies.ApiKey;
        }

        public async Task<TResponse> MakeRequest(TRequest apiRequest)
        {
            apiRequest.Key = ApiKey;
            var parsedRequest = _requestParser.Parse(apiRequest);
            var url = CreateRequestUrl(parsedRequest);

            try
            {
                var response = await HttpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                var deserializedResponse = Deserialize<TResponse>(content);

                if (deserializedResponse.Status != "OK")
                    throw new GoogleApiException(deserializedResponse.Status, deserializedResponse.ErrorMessage);

                return deserializedResponse;
            }
            catch (GoogleApiException)
            {
                throw;
            }
            catch (Exception unknownException)
            {
                Logger.LogError("Unknown error during google api interaction: {@unknownException}", unknownException);
                throw new GoogleApiException(unknownException);
            }
        }
    }
}