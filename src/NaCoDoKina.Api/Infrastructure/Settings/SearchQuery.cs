using NaCoDoKina.Api.Infrastructure.Extensions;
using System;
using System.Text;

namespace NaCoDoKina.Api.Infrastructure.Settings
{
    /// <summary>
    /// Representation of search query in external service 
    /// </summary>
    public class SearchQuery
    {
        /// <summary>
        /// Search path relative to base url 
        /// </summary>
        public string SearchPath { get; set; }

        /// <summary>
        /// Query result binding data 
        /// </summary>
        public QueryResults QueryResults { get; set; }

        /// <summary>
        /// Query parameters 
        /// </summary>
        public QueryParameter[] QueryParameters { get; set; }

        public string Build(Func<QueryParameter, string> replaceValueCallback = null)
        {
            var queryBuilder = new StringBuilder("?");

            foreach (var queryParameter in QueryParameters)
            {
                var value = queryParameter.Value;

                if (queryParameter.Replecable)
                    value = replaceValueCallback?.Invoke(queryParameter);

                queryBuilder.Append(queryParameter.Parse(value));
                queryBuilder.Append("&");
            }

            var trimmedLastCharLength = queryBuilder.Length - 1;
            var query = queryBuilder.ToString(0, trimmedLastCharLength);

            return query;
        }
    }

    public class QueryResults
    {
        /// <summary>
        /// CSS selector for results collection 
        /// </summary>
        public string ResultsCollectionSelector { get; set; }

        /// <summary>
        /// CSS selectors for result element binding 
        /// </summary>
        public PropertySelector[] ResultElementsSelectors { get; set; }
    }

    public class QueryParameter
    {
        /// <summary>
        /// Property name which maps to this query parameter 
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Query parameter name visible in url 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Query parameter value 
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// If query value can be replaced 
        /// </summary>
        public bool Replecable { get; set; }

        public string Parse(string replaceValue = default(string))
        {
            var value = replaceValue.IsNullOrEmpty() || !Replecable ? Value : replaceValue;
            return $"{Name}={value}";
        }
    }
}