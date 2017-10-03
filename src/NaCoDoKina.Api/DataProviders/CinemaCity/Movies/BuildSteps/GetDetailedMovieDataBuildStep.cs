using ApplicationCore.Results;
using NaCoDoKina.Api.DataProviders.Client;
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
        private readonly IHtmlParser<Movie> _htmlParser;
        private readonly CinemaNetworksSettings _networksSettings;
        private readonly IWebClient _webClient;
        public string Name => "Detailed information about movie";

        public int Position => 2;

        public GetDetailedMovieDataBuildStep(IWebClient webClient, IHtmlParser<Movie> htmlParser, CinemaNetworksSettings networksSettings)
        {
            _htmlParser = htmlParser ?? throw new ArgumentNullException(nameof(htmlParser));
            _networksSettings = networksSettings ?? throw new ArgumentNullException(nameof(networksSettings));

            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
        }

        public async Task<Result<Movie[]>> BuildMany(Movie[] entities)
        {
            var (networkName, _) = _networksSettings.CinemaCityNetwork;

            foreach (var movie in entities)
            {
                var movieUrl = movie.ExternalMovies
                    .First(externalMovie => externalMovie.CinemaNetwork.Name == networkName)
                    .MovieUrl;

                var result = await _webClient.MakeGetRequestAsync(movieUrl);

                if (!result.IsSuccess)
                    return Result.Failure<Movie[]>($"Failed to get details for movie {movie.Title} under url {movieUrl}");

                _htmlParser.Parse(result.Value, movie);
            }

            return Result.Success(entities);
        }
    }
}