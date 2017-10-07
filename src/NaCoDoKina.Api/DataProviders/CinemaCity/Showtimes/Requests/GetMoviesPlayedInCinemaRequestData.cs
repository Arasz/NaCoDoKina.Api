using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Infrastructure.Settings;
using System.Collections.Generic;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Requests
{
    public class GetMoviesPlayedInCinemaRequestData : CinemaCityRequestDataBase
    {
        public GetMoviesPlayedInCinemaRequestData(CinemaNetworksSettings cinemaNetworksSettings, IEnumerable<IRequestParameter> defaultRequestParameters, ILogger<GetMoviesPlayedInCinemaRequestData> logger)
            : base("film-events/in-cinema/{CinemaId}/at-date/{ShowtimeDate}", defaultRequestParameters, cinemaNetworksSettings, logger)
        {
        }
    }
}