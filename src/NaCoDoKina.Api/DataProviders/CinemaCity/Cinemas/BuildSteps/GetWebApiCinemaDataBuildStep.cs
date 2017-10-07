using Microsoft.Extensions.Logging;
using NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.Requests;
using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.DataProviders.Client;
using NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Services;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    /// <summary>
    /// Retrieves all possible data from cinema service 
    /// </summary>
    public class GetWebApiCinemaDataBuildStep : GetWebApiDataBuildStep<Cinema>
    {
        public override int Position => 1;

        public override string Name => "Fetch data from web service";

        public GetWebApiCinemaDataBuildStep(IWebClient webClient, CinemasRequestData cinemasRequestData,
            ISerializationService serializationService, ILogger<GetWebApiCinemaDataBuildStep> logger)
            : base(webClient, cinemasRequestData, serializationService, logger)
        {
        }

        private class Body
        {
            public class Cinema
            {
                public string Id { get; set; }

                public string GroupId { get; set; }

                public string Link { get; set; }

                public string Address { get; set; }

                public string DisplayName { get; set; }
            }

            public Cinema[] Cinemas { get; set; }
        }

        protected override Task<Cinema[]> ParseDataToEntities(string responseContent)
        {
            var deserializedCinemas = SerializationService.Deserialize<CinemaCityResponse<Body>>(responseContent);

            var cinemas = deserializedCinemas
                .Body.Cinemas
                .Select(cinema => new Cinema
                {
                    Name = cinema.DisplayName,
                    Address = cinema.Address,
                    CinemaUrl = cinema.Link,
                    ExternalId = cinema.Id,
                }).ToArray();

            return Task.FromResult(cinemas);
        }
    }
}