namespace NaCoDoKina.Api.DataProviders.Requests
{
    /// <summary>
    /// Base class for all request data, defines common properties 
    /// </summary>
    public abstract class RequestDataBase : IParsableRequestData
    {
        protected abstract string PathPattern { get; }

        protected abstract string QueryPattern { get; }

        protected abstract string BaseUrl { get; }

        public virtual string BuildUrl(string resourcePath, string query)
        {
            return $"{BaseUrl}/{resourcePath}?{query}";
        }

        public abstract Request Parse();
    }
}