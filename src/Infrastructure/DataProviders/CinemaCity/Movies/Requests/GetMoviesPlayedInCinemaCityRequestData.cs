using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Requests;
using Infrastructure.DataProviders.Requests;
using Infrastructure.Settings.CinemaNetwork;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Infrastructure.DataProviders.CinemaCity.Movies.Requests
{
    public class GetMoviesPlayedInCinemaCityRequestData : CinemaCityRequestDataBase
    {
        public GetMoviesPlayedInCinemaCityRequestData(CinemaNetworksSettings cinemaNetworksSettings, IEnumerable<IRequestParameter> requestParameters, ILogger<GetMoviesPlayedInCinemaRequestData> logger)
            : base("films/until/{UntilDate}", requestParameters, cinemaNetworksSettings, logger)
        {
        }
    }
}