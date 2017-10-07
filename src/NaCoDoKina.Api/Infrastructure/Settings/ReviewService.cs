namespace NaCoDoKina.Api.Infrastructure.Settings
{
    public class ReviewService
    {
        /// <summary>
        /// Review service site url 
        /// </summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// Search query in review service 
        /// </summary>
        public SearchQuery SearchQuery { get; set; }

        /// <summary>
        /// Service name 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mappings specific for movie review 
        /// </summary>
        public PropertySelector[] ReviewBindingMappings { get; set; }

        public string BuildQueryUrl(string query)
        {
            return $"{BaseUrl}{SearchQuery.SearchPath}{query}";
        }
    }
}