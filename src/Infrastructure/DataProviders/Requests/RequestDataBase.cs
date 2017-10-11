namespace Infrastructure.DataProviders.Requests
{
    /// <summary>
    /// Base class for all request data, defines common properties 
    /// </summary>
    public abstract class RequestDataBase : IParsableRequestData
    {
        /// <summary>
        /// Pattern for relative to base resource path 
        /// </summary>
        protected string ResourcePattern => $"{PathPattern}?{QueryPattern}";

        /// <summary>
        /// Path pattern (after base url, before ?) 
        /// </summary>
        protected abstract string PathPattern { get; }

        /// <summary>
        /// Query string pattern (after ? in url) 
        /// </summary>
        protected abstract string QueryPattern { get; }

        /// <summary>
        /// Base request url 
        /// </summary>
        protected abstract string BaseUrl { get; }

        /// <summary>
        /// Appends given resource path to base url 
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <returns> Absolute url </returns>
        protected string AbsoluteUrl(string resourcePath) => $"{BaseUrl}/{resourcePath}";

        public abstract Request Parse(params IRequestParameter[] requestParameters);
    }
}