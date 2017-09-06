using System.Text.RegularExpressions;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request
{
    public abstract class GoogleRequestParserBase<TRequest> : IRequestParser<TRequest>
        where TRequest : GoogleApiRequest
    {
        private readonly Regex _formatRegex = new Regex("[A-Z]", RegexOptions.Compiled);

        protected string FormatPropertyName(string name)
        {
            var formatted = _formatRegex.Replace(name, match => $"_{match.Value.ToLowerInvariant()}");

            return formatted.TrimStart('_');
        }

        public abstract string Parse(TRequest request);
    }
}