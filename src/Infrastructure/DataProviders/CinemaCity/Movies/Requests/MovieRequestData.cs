using System.Collections.Generic;
using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Requests;
using Infrastructure.DataProviders.Requests;
using Infrastructure.Settings;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataProviders.CinemaCity.Movies.Requests
{
    public class MovieRequestData : CinemaCityRequestDataBase
    {
        public MovieRequestData(CinemaNetworksSettings cinemaNetworksSettings, IEnumerable<IRequestParameter> requestParameters, ILogger<GetMoviesPlayedInCinemaRequestData> logger)
            : base("films/until/{UntilDate}", requestParameters, cinemaNetworksSettings, logger)
        {
        }
    }
}