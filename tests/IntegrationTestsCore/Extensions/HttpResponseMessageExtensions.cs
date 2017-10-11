using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace IntegrationTestsCore.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public class UnexpectedStatusCodeException : Exception
        {
            public UnexpectedStatusCodeException(HttpStatusCode expected, HttpStatusCode given)
                : base($"Expected {expected} status code but gotten {given}")
            {
            }
        }

        /// <summary>
        /// Throws when content status code is different from given 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="statusCode"> Expected status code </param>
        public static void EnsureStatusCode(this HttpResponseMessage response, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            if (response.StatusCode != statusCode)
                throw new UnexpectedStatusCodeException(statusCode, response.StatusCode);
        }
    }

    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonObjectAsync<T>(this HttpContent content)
        {
            var contentAsString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(contentAsString);
        }
    }
}