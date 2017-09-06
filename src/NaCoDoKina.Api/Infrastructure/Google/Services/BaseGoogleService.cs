﻿using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request;

namespace NaCoDoKina.Api.Infrastructure.Google.Services
{
    public abstract class BaseGoogleService<TRequest, TResponse> : BaseHttpApiClient
        where TRequest : GoogleApiRequest

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
                return Deserialize<TResponse>(content);
            }
            catch (Exception exception)
            {
                Logger.LogError("Error during deserialization of google api response: @e", exception);
                throw;
            }
        }
    }
}