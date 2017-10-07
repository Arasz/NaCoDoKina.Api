using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies.Requests
{
    public class MovieRequestData : CinemaCityRequestDataBase
    {
        public MovieRequestData(CinemaNetworksSettings cinemaNetworksSettings, ILogger<MovieRequestData> logger) : base(DateTime.Now.AddDays(12), "films", cinemaNetworksSettings, logger)
        {
        }
    }
}