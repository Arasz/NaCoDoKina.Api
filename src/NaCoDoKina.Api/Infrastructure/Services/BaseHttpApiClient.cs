using System;
using System.Net.Http;
using System.Web;
using Newtonsoft.Json;

namespace NaCoDoKina.Api.Infrastructure.Services
{
    public abstract class BaseHttpApiClient
    {
        protected readonly HttpClient HttpClient;

        protected abstract string BaseUrl { get; }

        protected virtual string CreateRequestUrl(string parsedRequest) =>
            $"{BaseUrl}{parsedRequest}";

        protected BaseHttpApiClient(HttpClient httpClient)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        protected string ToUrlEncoded(string str) => HttpUtility.UrlEncode(str);

        protected string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);

        protected T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}