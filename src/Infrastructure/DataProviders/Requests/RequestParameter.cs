using ApplicationCore.Results;

namespace Infrastructure.DataProviders.Requests
{
    public class RequestParameter : IRequestParameter
    {
        public RequestParameter()
        {
        }

        public RequestParameter(string template, string value)
        {
            Template = template;
            Value = value;
        }

        private string _template;

        /// <summary>
        /// Template string in url string exchanged for value <example> movieId </example> 
        /// </summary>
        public string Template
        {
            get => _template;
            set
            {
                _template = value;
                PreparedTemplate = "{" + Template + "}";
            }
        }

        /// <summary>
        /// Parameter value parsed to string 
        /// </summary>
        public string Value { get; set; }

        private string PreparedTemplate { get; set; }

        public Result<string> SubstituteTemplate(string pattern)
        {
            if (pattern.Contains(PreparedTemplate))
            {
                return Result.Success(pattern.Replace(PreparedTemplate, Value));
            }
            return Result.Failure<string>($"Template {Template} not found in pattern {pattern}");
        }
    }
}