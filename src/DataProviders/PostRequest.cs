using System.Net.Http;

namespace DataProviders
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