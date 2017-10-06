﻿using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.DataProviders.Client;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Infrastructure.Settings;
using NaCoDoKina.Api.Repositories;
using NaCoDoKina.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Movies.BuildSteps
{
    public class GetMovieDataBuildStep : GetDataBuildStep<Movie>
    {
        private readonly ILogger<GetMovieDataBuildStep> _logger;
        private readonly ICinemaNetworkRepository _cinemaNetworkRepository;
        private readonly CinemaNetworksSettings _cinemaNetworksSettings;

        private Entities.Cinemas.CinemaNetwork _cinemaNetwork;

        private async Task<Entities.Cinemas.CinemaNetwork> GetCinemaNetwork()
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

        private Movie MapCinemaMovieToMovie(Body.Movie movie, Entities.Cinemas.CinemaNetwork cinemaNetwork)
        {
            var cinemaNetworkUrl = cinemaNetwork.CinemaNetworkUrl;

            string BuildResourcePath(string baseUrl, string resourceUrl)
            {
                if (resourceUrl.IsNullOrEmpty())
                {
                    _logger.LogInformation("Resource url for movie {@movie} from cinema network {@cinemaNetwork} was null or empty", movie, cinemaNetwork);
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

        protected override async Task<Movie[]> BuildModelsFromResponseContent(string content)
        {
            var deserializedMovies = SerializationService.Deserialize<CinemaCityResponse<Body>>(content);

            var cinemaNetwork = await GetCinemaNetwork();

            using (_logger.BeginScope(nameof(GetMovieDataBuildStep)))
            {
                return deserializedMovies
                    .Body.Films
                    .Select(movie => MapCinemaMovieToMovie(movie, cinemaNetwork)).ToArray();
            }
        }

        public GetMovieDataBuildStep(IWebClient webClient, MovieRequestData parsableRequestData, ISerializationService serializationService,
            ICinemaNetworkRepository cinemaNetworkRepository, CinemaNetworksSettings cinemaNetworksSettings,
            ILogger<GetMovieDataBuildStep> logger)
            : base(webClient, parsableRequestData, serializationService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cinemaNetworkRepository = cinemaNetworkRepository ?? throw new ArgumentNullException(nameof(cinemaNetworkRepository));
            _cinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
        }
    }
}