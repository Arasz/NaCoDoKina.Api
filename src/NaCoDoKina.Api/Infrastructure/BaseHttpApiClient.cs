using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web;

namespace NaCoDoKina.Api.Infrastructure
{
    public abstract class BaseHttpApiClient
    {
        protected readonly HttpClient HttpClient;
        protected readonly ILogger<BaseHttpApiClient> Logger;

        protected BaseHttpApiClient(HttpClient httpClient, ILogger<BaseHttpApiClient> logger)
        {
            HttpClient = httpClient;
            Logger = logger;
        }

        protected string ToUrlEncoded(string str) => HttpUtility.UrlEncode(str);

        protected string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);

        protected T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}