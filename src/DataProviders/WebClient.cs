using ApplicationCore.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataProviders
{
    /// <summary>
    /// Web client which makes request to service and parses response 
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public class WebClient<TResponse>
    {
        private readonly IResponseParser<TResponse> _responseParser;
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;

        protected WebClient(IResponseParser<TResponse> responseParser, ILogger logger, HttpClient httpClient)
        {
            _responseParser = responseParser ?? throw new ArgumentNullException(nameof(responseParser));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public virtual async Task<Result<TResponse>> MakeRequestAsync(IParsableRequestData requestData)
        {
            using (_logger.BeginScope($"{GetType().Name} - {nameof(MakeRequestAsync)}"))
            {
                _logger.LogDebug("Parsing requestData {@requestData}", requestData);

                var request = requestData.Parse();

                HttpResponseMessage httpResponse = null;
                switch (request)
                {
                    case GetRequest getRequest:
                        _logger.LogDebug("Request parsed {@request}", request);
                        httpResponse = await _httpClient.GetAsync(getRequest.BuildUrl());
                        break;

                    case PostRequest postRequest:
                        _logger.LogDebug("Request parsed {@request}", request);
                        httpResponse = await _httpClient.PostAsync(postRequest.BuildUrl(), postRequest.Body);
                        break;
                }

                if (httpResponse is null)
                {
                    _logger.LogError("Request data {@requestData} parsed to unknown request type", requestData);
                    return Result.Failure<TResponse>("Unknown request type");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Request failed with status code {code} and content {content}", httpResponse.StatusCode, content);
                    return Result.Failure<TResponse>($"Request failed with code {httpResponse.StatusCode}");
                }

                _logger.LogDebug("Parse response content {content}", content);

                var response = _responseParser.Parse(content);

                _logger.LogDebug("Response parsed {@response}", response);

                return Result.Success(response);
            }
        }
    }
}