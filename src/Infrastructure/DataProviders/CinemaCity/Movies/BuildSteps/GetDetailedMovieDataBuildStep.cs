using ApplicationCore.Entities.Movies;
using Infrastructure.DataProviders.CinemaCity.Movies.Bindings;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Movies.BuildSteps
{
    public class GetDetailedMovieDataBuildStep : GetWebPageDataBuildStep<Movie, EmptyContext>
    {
        private readonly CinemaNetworksSettings _networksSettings;
        public override string Name => "Detailed information about movie";

        public override int Position => 2;

        public GetDetailedMovieDataBuildStep(CinemaCityMovieWebPageBinder cinemaCityMovieWebPageBinder, CinemaNetworksSettings networksSettings, ILogger<GetDetailedMovieDataBuildStep> logger)
            : base(cinemaCityMovieWebPageBinder, logger)
        {
            _networksSettings = networksSettings ?? throw new ArgumentNullException(nameof(networksSettings));
        }

        /// <inheritdoc/>
        protected override Task<string> GetWebPageUrl(Movie entity)
        {
            var networkName = _networksSettings.CinemaCityNetwork.Name;

            var url = entity.ExternalMovies
                .First(externalMovie => externalMovie.CinemaNetwork.Name == networkName)
                .MovieUrl;

            return Task.FromResult(url);
        }
    }
}