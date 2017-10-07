using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Infrastructure.Settings;
using System.Collections.Generic;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.Requests
{
    public class CinemasRequestData : CinemaCityRequestDataBase
    {
        public CinemasRequestData(CinemaNetworksSettings cinemaNetworksSettings, IEnumerable<IRequestParameter> requestParameters, ILogger<CinemasRequestData> logger)
            : base("cinemas/with-event/until/{UntilDate}", requestParameters, cinemaNetworksSettings, logger)
        {
        }
    }
}