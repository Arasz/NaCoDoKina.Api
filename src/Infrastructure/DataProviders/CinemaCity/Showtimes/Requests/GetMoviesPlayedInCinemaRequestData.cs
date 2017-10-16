using System.Collections.Generic;
using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.Requests;
using Infrastructure.Settings;
using Infrastructure.Settings.CinemaNetwork;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataProviders.CinemaCity.Showtimes.Requests
{
    public class GetMoviesPlayedInCinemaRequestData : CinemaCityRequestDataBase
    {
        public GetMoviesPlayedInCinemaRequestData(CinemaNetworksSettings cinemaNetworksSettings, IEnumerable<IRequestParameter> defaultRequestParameters, ILogger<GetMoviesPlayedInCinemaRequestData> logger)
            : base("film-events/in-cinema/{Cinema}/at-date/{ShowtimeDate}", defaultRequestParameters, cinemaNetworksSettings, logger)
        {
        }
    }
}