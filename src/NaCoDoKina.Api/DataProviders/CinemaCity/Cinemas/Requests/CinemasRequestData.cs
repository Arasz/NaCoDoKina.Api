using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.Requests
{
    public class CinemasRequestData : CinemaCityRequestDataBase
    {
        public CinemasRequestData(CinemaNetworksSettings cinemaNetworksSettings, ILogger<CinemasRequestData> logger) : base(DateTime.Now.AddYears(1), "cinemas/with-event", cinemaNetworksSettings, logger)
        {
        }
    }
}