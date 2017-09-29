using ApplicationCore.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataProviders
{
    public abstract class ClientBase<TRequest, TResponse>
        where TRequest : IParsableRequestData
        where TResponse : IResponse, new()
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        protected ClientBase(ILogger logger, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public virtual async Task<Result<TResponse>> MakeRequest(TRequest requestData)
        {
            using (_logger.BeginScope($"{GetType().Name} - {nameof(MakeRequest)}"))
            {
                _logger.LogDebug("Parsing requestData {@requestData}", requestData);

                var requestUrl = requestData.Parse();

                HttpResponseMessage httpResponse;
                switch (requestUrl)
                {
                    case GetRequest getRequest:
                        httpResponse = await _httpClient.GetAsync(getRequest.BuildUrl());
                        break;

                    case PostRequest postRequest:
                        httpResponse = await _httpClient.PostAsync(postRequest.BuildUrl(), postRequest.Body);
                        break;
                }

                _logger.LogDebug("Request parser {@requestUrl}", requestData);

                return Task.FromResult(Result.Success(new TResponse()));
            }
        }
    }
}