using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Common
{
    public abstract class CinemaCityRequestDataBase : RequestDataBase
    {
        private readonly string _requestSpecificPath;
        private readonly ILogger<CinemaCityRequestDataBase> _logger;

        protected CinemaCityRequestDataBase(DateTime until, string requestSpecificPath,
            CinemaNetworksSettings cinemaNetworksSettings, ILogger<CinemaCityRequestDataBase> logger)
        {
            _requestSpecificPath = requestSpecificPath ?? throw new ArgumentNullException(nameof(requestSpecificPath));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            Until = until;
            CinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
        }

        protected override string PathPattern => "data-api-service/v1/quickbook/10103/{0}/until/{1}";

        public string LanguageTag => "pl_PL";

        public string Attributes => "";

        protected override string QueryPattern => "attr={0}&lang={1}";

        protected override string BaseUrl => CinemaNetworksSettings.CinemaCityNetwork.Url;

        public DateTime Until { get; }

        public CinemaNetworksSettings CinemaNetworksSettings { get; }

        public override Request Parse()
        {
            using (_logger.BeginScope(nameof(Parse)))
            {
                _logger.LogDebug("Parse request {@request}", this);

                var date = Until.ToString("yyyy-MM-dd");
                var path = string.Format(PathPattern, _requestSpecificPath, date);

                _logger.LogDebug("Format path pattern {pattern} to path {path} with specific path {specificPath} and date {date}", PathPattern, path, _requestSpecificPath, Until);

                var query = string.Format(QueryPattern, Attributes, LanguageTag);

                _logger.LogDebug("Format query pattern {pattern} to query {query} with attributes {attributes} and language tag {langTag}", QueryPattern, query, Attributes, LanguageTag);

                var url = BuildUrl(path, query);

                _logger.LogDebug("Built url {url}", url);

                return new GetRequest(url);
            }
        }
    }
}