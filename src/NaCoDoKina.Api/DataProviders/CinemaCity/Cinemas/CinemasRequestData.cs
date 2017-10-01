using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas
{
    public class CinemasRequestData : CinemaCityRequestDataBase
    {
        public CinemasRequestData(CinemaNetworksSettings cinemaNetworksSettings) : base(DateTime.Now.AddYears(1), "cinemas/with-event", cinemaNetworksSettings)
        {
        }
    }
}