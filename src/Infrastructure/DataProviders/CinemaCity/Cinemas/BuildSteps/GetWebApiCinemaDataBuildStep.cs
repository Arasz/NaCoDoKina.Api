using ApplicationCore.Entities.Cinemas;
using Infrastructure.DataProviders.CinemaCity.Cinemas.Requests;
using Infrastructure.DataProviders.CinemaCity.Common;
using Infrastructure.DataProviders.Client;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    /// <summary>
    /// Retrieves all possible data from cinema service 
    /// </summary>
    public class GetWebApiCinemaDataBuildStep : GetWebApiDataBuildStep<Cinema, EmptyContext>
    {
        public override int Position => 1;

        public override string Name => "Fetch data from web service";

        public GetWebApiCinemaDataBuildStep(IWebClient webClient, GetCinemaCityCinemasRequestData getCinemaCityCinemasRequestData,
            ISerializationService serializationService, ILogger<GetWebApiCinemaDataBuildStep> logger)
            : base(webClient, getCinemaCityCinemasRequestData, serializationService, logger)
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

        protected override Task<Cinema[]> ParseDataToEntities(string responseContent, EmptyContext context)
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
                    GroupId = cinema.GroupId
                }).ToArray();

            return Task.FromResult(cinemas);
        }
    }
}