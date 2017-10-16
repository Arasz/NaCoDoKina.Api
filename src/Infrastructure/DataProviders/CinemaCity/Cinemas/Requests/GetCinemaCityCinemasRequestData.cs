using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.Requests;
using Infrastructure.Settings.CinemaNetwork;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Infrastructure.DataProviders.CinemaCity.Cinemas.Requests
{
    public class GetCinemaCityCinemasRequestData : CinemaCityRequestDataBase
    {
        public GetCinemaCityCinemasRequestData(CinemaNetworksSettings cinemaNetworksSettings, IEnumerable<IRequestParameter> requestParameters, ILogger<GetCinemaCityCinemasRequestData> logger)
            : base("cinemas/with-event/until/{UntilDate}", requestParameters, cinemaNetworksSettings, logger)
        {
        }
    }
}