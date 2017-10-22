using ApplicationCore.Entities.Movies;
using ApplicationCore.Repositories;
using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Context;
using Infrastructure.DataProviders.CinemaCity.Showtimes.Requests;
using Infrastructure.DataProviders.Client;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.Requests;
using Infrastructure.Extensions;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Showtimes.BuildSteps
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

        public class Body
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

            public Movie[] Films { get; set; }
        }

        protected override async Task<MovieShowtime[]> ParseDataToEntitiesAsync(string content, MovieShowtimesContext context)
        {
            var cinemaCityResponse = SerializationService.Deserialize<CinemaCityResponse<Body>>(content);

            var showtimes = new List<MovieShowtime>(cinemaCityResponse.Body.Events.Length);

            var moviesWithEvents = cinemaCityResponse.Body.Events
                .Join(cinemaCityResponse.Body.Films,
                    e => e.FilmId, m => m.Id,
                    (e, m) => (Event: e, Movie: m));

            foreach (var tuple in moviesWithEvents)
            {
                var movie = await _movieRepository.GetMovieByTitleAsync(tuple.Movie.Name);

                var showtime = new MovieShowtime
                {
                    ExternalId = tuple.Event.Id,
                    Movie = movie,
                    Cinema = context.Cinema,
                    Available = !tuple.Event.SoldOut,
                    BookingLink = tuple.Event.BookingLink,
                    Language = "",
                    ShowTime = tuple.Event.EventDateTime,
                    ShowType = tuple.Event.FormatAttributes()
                };
                showtimes.Add(showtime);
            }
            return showtimes.ToArray();
        }
    }
}