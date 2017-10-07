using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Movies.Bindings;
using NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies.BuildSteps
{
    public class GetDetailedMovieDataBuildStep : GetWebPageDataBuildStep<Movie>
    {
        private readonly CinemaNetworksSettings _networksSettings;
        public override string Name => "Detailed information about movie";

        public override int Position => 2;

        public GetDetailedMovieDataBuildStep(MovieWebPageBinder movieWebPageBinder, CinemaNetworksSettings networksSettings, ILogger<GetDetailedMovieDataBuildStep> logger)
            : base(movieWebPageBinder, logger)
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