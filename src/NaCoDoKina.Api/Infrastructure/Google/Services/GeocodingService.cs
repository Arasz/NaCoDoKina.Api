using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.Infrastructure.Google.DataContract;
using NaCoDoKina.Api.Infrastructure.Services.Google;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Infrastructure.Google.Services
{
    public class GeocodingService : BaseHttpApiClient, IGeocodingService
    {
        private readonly string _apiKey;

        private string _baseUrl = @"https://maps.googleapis.com/maps/api/geocode/";

        private string _outputFormat = "json";

        private string CreateRequestUrl(string address) => $"{_baseUrl}{_outputFormat}?address={ToUrlEncoded(address)}&key={_apiKey}";

        public async Task<GeocodingApiResponse> GeocodeAsync(string address)
        {
            var url = CreateRequestUrl(address);
            var response = await HttpClient.GetAsync(url);

            try
            {
                var content = await response.Content.ReadAsStringAsync();
                return Deserialize<GeocodingApiResponse>(content);
            }
            catch (Exception e)
            {
                Logger.LogError("Error during deserialization of google api response: @e", e);
                throw;
            }
        }

        public GeocodingService(HttpClient httpClient, ILogger<BaseHttpApiClient> logger, string apiKey) : base(httpClient, logger)
        {
            _apiKey = apiKey;
        }
    }
}