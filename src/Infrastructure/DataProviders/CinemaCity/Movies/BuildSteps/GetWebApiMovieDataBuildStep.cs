using ApplicationCore.Entities.Movies;
using ApplicationCore.Entities.Resources;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.CinemaCity.Movies.Requests;
using Infrastructure.DataProviders.Client;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Infrastructure.Settings.CinemaNetwork;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CinemaNetwork = ApplicationCore.Entities.Cinemas.CinemaNetwork;

namespace Infrastructure.DataProviders.CinemaCity.Movies.BuildSteps
{
    public class GetWebApiMovieDataBuildStep : GetWebApiDataBuildStep<Movie, EmptyContext>
    {
        private readonly ICinemaNetworkRepository _cinemaNetworkRepository;
        private readonly CinemaNetworksSettings _cinemaNetworksSettings;

        private CinemaNetwork _cinemaNetwork;

        private async Task<CinemaNetwork> GetCinemaNetwork()
        {
            if (_cinemaNetwork is null)
            {
                _cinemaNetwork = await _cinemaNetworkRepository.GetByNameAsync(_cinemaNetworksSettings.CinemaCityNetwork.Name);
            }

            return _cinemaNetwork;
        }

        public override string Name => "Get movie data from service";

        public override int Position => 1;

        private class Body
        {
            public class Movie
            {
                public string Id { get; set; }

                public string Name { get; set; }

                public int Length { get; set; }

                public string PosterLink { get; set; }

                public string VideoLink { get; set; }

                public string Link { get; set; }

                public int Weight { get; set; }

                public string ReleaseYear { get; set; }

                public string[] AttributeIds { get; set; }
            }

            public Movie[] Films { get; set; }
        }

        private Movie MapCinemaMovieToMovie(Body.Movie movie, CinemaNetwork cinemaNetwork)
        {
            var cinemaNetworkUrl = cinemaNetwork.CinemaNetworkUrl;

            string BuildResourcePath(string baseUrl, string resourceUrl)
            {
                if (resourceUrl.IsNullOrEmpty())
                {
                    Logger.LogDebug("Empty resource url for movie {@Movie} in cinema network {@CinemaNetwork}", movie, cinemaNetwork);
                    return string.Empty;
                }

                return $"{baseUrl}{resourceUrl}";
            }

            var movieDetails = new MovieDetails
            {
                Length = TimeSpan.FromMinutes(movie.Length),
                Title = movie.Name,
                MediaResources = new List<MediaLink>()
            };

            var movieVideoUrl = BuildResourcePath(string.Empty, movie.VideoLink);

            if (!movieVideoUrl.IsNullOrEmpty())
            {
                movieDetails.MediaResources.Add(new MediaLink
                {
                    MediaType = MediaType.Video,
                    Url = movieVideoUrl
                });
            }

            var posterUrl = BuildResourcePath(cinemaNetworkUrl, movie.PosterLink);

            if (!posterUrl.IsNullOrEmpty())
            {
                movieDetails.MediaResources.Add(new MediaLink
                {
                    MediaType = MediaType.Poster,
                    Url = posterUrl
                });
            }

            return new Movie
            {
                Title = movie.Name,
                ExternalMovies = new List<ExternalMovie>
                {
                    new ExternalMovie
                    {
                        ExternalId = movie.Id,
                        CinemaNetwork = cinemaNetwork,
                        MovieUrl =  BuildResourcePath(cinemaNetworkUrl, movie.Link)
                    }
                },
                PosterUrl = posterUrl,
                Details = movieDetails
            };
        }

        protected override async Task<Movie[]> ParseDataToEntities(string content, EmptyContext context)
        {
            var deserializedMovies = SerializationService.Deserialize<CinemaCityResponse<Body>>(content);

            var cinemaNetwork = await GetCinemaNetwork();

            using (Logger.BeginScope(nameof(ParseDataToEntities)))
            {
                return deserializedMovies
                    .Body.Films
                    .Select(movie => MapCinemaMovieToMovie(movie, cinemaNetwork)).ToArray();
            }
        }

        public GetWebApiMovieDataBuildStep(IWebClient webClient, GetMoviesPlayedInCinemaCityRequestData parsableRequestData, ISerializationService serializationService,
            ICinemaNetworkRepository cinemaNetworkRepository, CinemaNetworksSettings cinemaNetworksSettings,
            ILogger<GetWebApiMovieDataBuildStep> logger)
            : base(webClient, parsableRequestData, serializationService, logger)
        {
            _cinemaNetworkRepository = cinemaNetworkRepository ?? throw new ArgumentNullException(nameof(cinemaNetworkRepository));
            _cinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
        }
    }
}