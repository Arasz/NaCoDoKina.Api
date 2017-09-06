using System.Linq;
using System.Text.RegularExpressions;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Request
{
    public abstract class GoogleRequestParserBase<TRequest> : IRequestParser<TRequest>
        where TRequest : GoogleApiRequest
    {
        private readonly Regex _formatRegex = new Regex("[A-Z]", RegexOptions.Compiled);

        protected string FormatPropertyName(string name)
        {
            var tokens = _formatRegex.Split(name)
                .Select(token => token.ToLowerInvariant());

            return string.Join('_', tokens);
        }

        public abstract string Parse(TRequest request);
    }
}