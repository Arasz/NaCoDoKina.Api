using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies
{
    public class MovieRequestData : CinemaCityRequestDataBase
    {
        public MovieRequestData(CinemaNetworksSettings cinemaNetworksSettings) : base(DateTime.Now.AddDays(12), "films", cinemaNetworksSettings)
        {
        }
    }
}