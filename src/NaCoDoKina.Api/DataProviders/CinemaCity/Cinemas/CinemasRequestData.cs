using NaCoDoKina.Api.DataProviders.Requests;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas
{
    public class CinemasRequestData : RequestDataBase
    {
        public CinemasRequestData(DateTime until)
        {
            Until = until;
        }

        protected override string PathPattern => "data-api-service/v1/quickbook/10103/cinemas/with-event/until/{date}";

        public string LanguageTag => "pl_PL";

        public string Attributes => "";

        protected override string QueryPattern => "attr={atr}&lang={lang}";

        protected override string BaseUrl => "https://www1.cinema-city.pl/";

        /// <summary>
        /// Date to which movie has any show 
        /// </summary>
        public DateTime Until { get; }

        public override Request Parse()
        {
            var path = string.Format(PathPattern, Until);

            var query = string.Format(QueryPattern, Attributes, LanguageTag);

            var url = BuildUrl(path, query);

            return new GetRequest(url);
        }
    }
}