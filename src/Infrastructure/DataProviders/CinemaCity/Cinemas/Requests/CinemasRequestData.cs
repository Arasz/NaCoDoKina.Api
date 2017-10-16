using System.Collections.Generic;
using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.Requests;
using Infrastructure.Settings;
using Infrastructure.Settings.CinemaNetwork;
using Microsoft.Extensions.Logging;

namespace Infrastructure.DataProviders.CinemaCity.Cinemas.Requests
{
    public class CinemasRequestData : CinemaCityRequestDataBase
    {
        public CinemasRequestData(CinemaNetworksSettings cinemaNetworksSettings, IEnumerable<IRequestParameter> requestParameters, ILogger<CinemasRequestData> logger)
            : base("cinemas/with-event/until/{UntilDate}", requestParameters, cinemaNetworksSettings, logger)
        {
        }
    }
}