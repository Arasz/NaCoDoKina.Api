﻿using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Entities.Resources;
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
        private readonly ICinemaNetworkRepository _cinemaNetworkRepository;
        private readonly CinemaNetworksSettings _cinemaNetworksSettings;
        public override string Name => "Get movie data from service";

        public override int Position => 1;

        private class MovieInternal
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

        protected override async Task<Movie[]> BuildModelsFromResponseContent(string content)
        {
            CinemaCityResponse<MovieInternal>.SetCollectionDataMemberName("movies");

            var deserializedMovies = SerializationService.Deserialize<CinemaCityResponse<MovieInternal>>(content);

            var cinemaNetwork = await _cinemaNetworkRepository.GetByNameAsync(_cinemaNetworksSettings.CinemaCityNetwork.Name);

            return deserializedMovies
                .ContentBody.Collection
                .Select(movie => new Movie
                {
                    ExternalIds = new List<ExternalId>
                    {
                        new ExternalId
                        {
                            MovieExternalId = movie.Id,
                            CinemaNetwork = cinemaNetwork
                        }
                    },
                    PosterUrl = movie.PosterLink,
                    Title = Name,
                    Details = new MovieDetails
                    {
                        Length = TimeSpan.FromMinutes(movie.Length),
                        Title = movie.Name,
                        MediaResources = new List<MediaLink>
                        {
                            new MediaLink
                            {
                                MediaType = MediaType.Video,
                                Url = movie.VideoLink,
                            },
                            new MediaLink
                            {
                                MediaType = MediaType.Poster,
                                Url = movie.PosterLink
                            }
                        },
                    },
                }).ToArray();
        }

        public GetMovieDataBuildStep(IWebClient webClient, IParsableRequestData parsableRequestData, ISerializationService serializationService,
            ICinemaNetworkRepository cinemaNetworkRepository, CinemaNetworksSettings cinemaNetworksSettings)
            : base(webClient, parsableRequestData, serializationService)
        {
            _cinemaNetworkRepository = cinemaNetworkRepository ?? throw new ArgumentNullException(nameof(cinemaNetworkRepository));
            _cinemaNetworksSettings = cinemaNetworksSettings ?? throw new ArgumentNullException(nameof(cinemaNetworksSettings));
        }
    }
}