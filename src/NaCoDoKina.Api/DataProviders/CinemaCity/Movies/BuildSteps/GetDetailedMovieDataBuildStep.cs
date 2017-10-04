using ApplicationCore.Results;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.DataProviders.Parsers;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies.BuildSteps
{
    public class GetDetailedMovieDataBuildStep : IBuildStep<Movie>
    {
        private readonly IWebPageMapper<Movie> _webPageMapper;
        private readonly CinemaNetworksSettings _networksSettings;
        public string Name => "Detailed information about movie";

        public int Position => 2;

        public GetDetailedMovieDataBuildStep(IWebPageMapper<Movie> webPageMapper, CinemaNetworksSettings networksSettings)
        {
            _webPageMapper = webPageMapper ?? throw new ArgumentNullException(nameof(webPageMapper));
            _networksSettings = networksSettings ?? throw new ArgumentNullException(nameof(networksSettings));
        }

        public async Task<Result<Movie[]>> BuildMany(Movie[] entities)
        {
            var (networkName, _) = _networksSettings.CinemaCityNetwork;

            foreach (var movie in entities)
            {
                var movieUrl = movie.ExternalMovies
                    .First(externalMovie => externalMovie.CinemaNetwork.Name == networkName)
                    .MovieUrl;

                await _webPageMapper.MapAsync(movieUrl, movie);
            }

            return Result.Success(entities);
        }
    }
}