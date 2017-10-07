using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Context;
using NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Requests;
using NaCoDoKina.Api.DataProviders.Client;
using NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps;
using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Services;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.BuildSteps
{
    public class GetWebApiMovieShowtimeBuildStep : GetWebApiDataBuildStep<MovieShowtime, MovieShowtimesContext>

    {
        public GetWebApiMovieShowtimeBuildStep(IWebClient webClient, GetMoviesPlayedInCinemaRequestData parsableRequestData, ISerializationService serializationService, ILogger<GetWebApiMovieShowtimeBuildStep> logger)
            : base(webClient, parsableRequestData, serializationService, logger)
        {
        }

        public override string Name => "Get showtimes for all movies played in all cinemas";

        public override int Position => 1;

        protected override IRequestParameter[] CreateRequestParameters(MovieShowtime[] entities, MovieShowtimesContext context)
        {
            var parameters = new IRequestParameter[]
            {
                new RequestParameter(nameof(context.CinemaId), context.CinemaId),
                new RequestParameter(nameof(context.ShowtimeDate), context.ShowtimeDate.ToChinaDate()),
            };
            return parameters;
        }

        protected override async Task<MovieShowtime[]> ParseDataToEntities(string content)
        {
            throw new System.NotImplementedException();
        }
    }
}