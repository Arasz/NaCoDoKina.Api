using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Context;
using NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.Requests;
using NaCoDoKina.Api.DataProviders.Client;
using NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps;
using NaCoDoKina.Api.DataProviders.Requests;
using NaCoDoKina.Api.Entities.Movies;
using NaCoDoKina.Api.Infrastructure.Extensions;
using NaCoDoKina.Api.Repositories;
using NaCoDoKina.Api.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Showtimes.BuildSteps
{
    public class GetWebApiShowtimeBuildStep : GetWebApiDataBuildStep<MovieShowtime, MovieShowtimesContext>
    {
        private readonly IMovieRepository _movieRepository;

        public GetWebApiShowtimeBuildStep(IWebClient webClient, GetMoviesPlayedInCinemaRequestData parsableRequestData, ISerializationService serializationService, IMovieRepository movieRepository, ILogger<GetWebApiShowtimeBuildStep> logger)
            : base(webClient, parsableRequestData, serializationService, logger)
        {
            _movieRepository = movieRepository ?? throw new ArgumentNullException(nameof(movieRepository));
        }

        public override string Name => "Get showtimes for all movies played in all cinemas";

        public override int Position => 1;

        protected override IRequestParameter[] CreateRequestParameters(MovieShowtime[] entities, MovieShowtimesContext context)
        {
            var parameters = new IRequestParameter[]
            {
                new RequestParameter(nameof(context.Cinema), context.Cinema.ExternalId),
                new RequestParameter(nameof(context.ShowtimeDate), context.ShowtimeDate.ToChinaDate()),
            };
            return parameters;
        }

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

            public class Event
            {
                public string Id { get; set; }

                public string FilmId { get; set; }

                public string CinemaId { get; set; }

                public DateTime BusinessDay { get; set; }

                public DateTime EventDateTime { get; set; }

                public string BookingLink { get; set; }

                public bool SoldOut { get; set; }

                public string[] AttributeIds { get; set; }

                public string FormatAttributes()
                {
                    var builder = new StringBuilder();

                    foreach (var attributeId in AttributeIds)
                    {
                        builder.Append(attributeId);
                        builder.Append(", ");
                    }

                    return builder
                         .ToString()
                         .TrimEnd(',', ' ');
                }
            }

            public Event[] Events { get; set; }

            public Movie[] Movies { get; set; }
        }

        protected override async Task<MovieShowtime[]> ParseDataToEntities(string content, MovieShowtimesContext context)
        {
            var cinemaCityResponse = SerializationService.Deserialize<CinemaCityResponse<Body>>(content);

            var showtimes = new List<MovieShowtime>(cinemaCityResponse.Body.Events.Length);

            foreach (var e in cinemaCityResponse.Body.Events)
            {
                var movie = await _movieRepository.GetMovieByExternalIdAsync(e.FilmId);

                var showtime = new MovieShowtime
                {
                    Movie = movie,
                    Cinema = context.Cinema,
                    Available = !e.SoldOut,
                    BookingLink = e.BookingLink,
                    Language = "",
                    ShowTime = e.EventDateTime,
                    ShowType = e.FormatAttributes()
                };
                showtimes.Add(showtime);
            }
            return showtimes.ToArray();
        }
    }
}