using System.Text.RegularExpressions;

namespace NaCoDoKina.Api.Infrastructure.Services.Google.DataContract.Common.Request
{
    public abstract class GoogleRequestParserBase<TRequest> : IRequestParser<TRequest>
        where TRequest : GoogleApiRequest
    {
        private static readonly Regex FormatRegex = new Regex("[A-Z]", RegexOptions.Compiled);

        protected string FormatPropertyName(string name)
        {
            var formatted = FormatRegex.Replace(name, match => $"_{match.Value.ToLowerInvariant()}");

            return formatted.TrimStart('_');
        }

        public abstract string Parse(TRequest request);
    }
}