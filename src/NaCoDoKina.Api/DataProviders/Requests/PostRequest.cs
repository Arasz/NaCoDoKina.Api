using System.Net.Http;

namespace NaCoDoKina.Api.DataProviders.Requests
{
    public class PostRequest : Request
    {
        public HttpContent Body { get; }

        public PostRequest(string baseUrl, HttpContent body) : base(baseUrl)
        {
            Body = body;
        }
    }
}