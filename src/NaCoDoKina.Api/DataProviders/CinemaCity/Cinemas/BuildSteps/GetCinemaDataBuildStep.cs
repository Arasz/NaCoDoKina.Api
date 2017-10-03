using NaCoDoKina.Api.DataProviders.CinemaCity.Common;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Services;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    /// <summary>
    /// Retrieves all possible data from cinema service 
    /// </summary>
    public class GetCinemaDataBuildStep : GetDataBuildStep<Cinema>
    {
        public override int Position => 1;

        public override string Name => "Fetch data from web service";

        public GetCinemaDataBuildStep(IWebClient webClient, CinemasRequestData cinemasRequestData,
            ISerializationService serializationService) : base(webClient, cinemasRequestData, serializationService)
        {
        }

        private class Cinema
        {
            public string Id { get; set; }

            public string GroupId { get; set; }

            public string Link { get; set; }

            public string Address { get; set; }

            public string DisplayName { get; set; }
        }

        protected override Task<Entities.Cinemas.Cinema[]> BuildModelsFromResponseContent(string responseContent)
        {
            CinemaCityResponse<Cinema>.SetCollectionDataMemberName("cinemas");
            var deserializedCinemas = SerializationService.Deserialize<CinemaCityResponse<Cinema>>(responseContent);

            var cinemas = deserializedCinemas
                .ContentBody.Collection
                .Select(cinema => new Entities.Cinemas.Cinema
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