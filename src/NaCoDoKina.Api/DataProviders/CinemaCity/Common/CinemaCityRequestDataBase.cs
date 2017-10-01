using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Common
{
    public abstract class CinemaCityRequestDataBase : RequestDataBase
    {
        private readonly string _requestSpecificPath;

        protected CinemaCityRequestDataBase(DateTime until, string requestSpecificPath, CinemaNetworksSettings cinemaNetworksSettings)
        {
            _requestSpecificPath = requestSpecificPath;
            Until = until;
            CinemaNetworksSettings = cinemaNetworksSettings;
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
            var path = string.Format(PathPattern, _requestSpecificPath, Until.ToString("yyyy-MM-dd"));

            var query = string.Format(QueryPattern, Attributes, LanguageTag);

            var url = BuildUrl(path, query);

            return new GetRequest(url);
        }
    }
}