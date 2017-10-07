using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Requests;
using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Infrastructure.Settings;
using System.Collections.Generic;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies.Requests
{
    public class MovieRequestData : CinemaCityRequestDataBase
    {
        public MovieRequestData(CinemaNetworksSettings cinemaNetworksSettings, IEnumerable<IRequestParameter> requestParameters, ILogger<GetMoviesPlayedInCinemaRequestData> logger)
            : base("films/until/{UntilDate}", requestParameters, cinemaNetworksSettings, logger)
        {
        }
    }
}