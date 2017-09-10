using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Response;
using NaCoDoKina.Api.Infrastructure.Google.Exceptions;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Google.Services
{
    public abstract class BaseGoogleService<TRequest, TResponse> : BaseHttpApiClient
        where TRequest : GoogleApiRequest
        where TResponse : GoogleApiResponse

    {
        private readonly IRequestParser<TRequest> _requestParser;
        protected readonly string ApiKey;

        protected abstract string BaseUrl { get; }

        private string _outputFormat = "json";

        protected string CreateRequestUrl(string parsedRequest) =>
            $"{BaseUrl}{_outputFormat}?{parsedRequest}";

        protected BaseGoogleService(GoogleServiceDependencies<TRequest> googleServiceDependencies)
            : base(googleServiceDependencies.HttpClient, googleServiceDependencies.Logger)
        {
            _requestParser = googleServiceDependencies.RequestParser;
            ApiKey = googleServiceDependencies.ApiKey;
        }

        public async Task<TResponse> MakeRequest(TRequest apiRequest)
        {
            apiRequest.Key = ApiKey;
            var parsedRequest = _requestParser.Parse(apiRequest);
            var url = CreateRequestUrl(parsedRequest);
            var response = await HttpClient.GetAsync(url);

            try
            {
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
            catch (Exception exception)
            {
                Logger.LogError("Error during deserialization of google api response: @e", exception);
                throw new GoogleApiException(exception);
            }
        }
    }
}