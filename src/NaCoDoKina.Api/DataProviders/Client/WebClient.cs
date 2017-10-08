﻿using ApplicationCore.Results;
using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.Requests;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.Client
{
    /// <summary>
    /// Web client which makes request to service and parses response 
    /// </summary>
    public class WebClient : IWebClient
    {
        private readonly ILogger<WebClient> _logger;
        private readonly HttpClient _httpClient;

        public WebClient(ILogger<WebClient> logger, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Make request and return response content 
        /// </summary>
        /// <param name="requestData"> Request data </param>
        /// <param name="requestParameters"> Parameters dynamically supplied to request </param>
        /// <returns> Response content </returns>
        public virtual async Task<Result<string>> MakeRequestAsync(IParsableRequestData requestData, params IRequestParameter[] requestParameters)
        {
            using (_logger.BeginScope($"{GetType().Name} - {nameof(MakeRequestAsync)}"))
            {
                _logger.LogDebug("Parsing requestData {@requestData}", requestData);

                var request = requestData.Parse(requestParameters);

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
                    return Result.Failure<string>("Unknown request type");
                }

                var content = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Request failed with status code {code} and content {content}", httpResponse.StatusCode, content);
                    return Result.Failure<string>($"Request failed with code {httpResponse.StatusCode}");
                }

                return Result.Success(content);
            }
        }

        public async Task<Result<string>> MakeGetRequestAsync(string url)
        {
            using (_logger.BeginScope($"{GetType().Name} - {nameof(MakeGetRequestAsync)}"))
            {
                _logger.LogDebug("Making GET request to {url}", url);

                var httpResponse = await _httpClient.GetAsync(url);

                var content = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.StatusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Request to url {url} failed with status code {code} and content {content}", url, httpResponse.StatusCode, content);
                    return Result.Failure<string>($"Request failed with code {httpResponse.StatusCode}");
                }

                return Result.Success(content);
            }
        }
    }
}