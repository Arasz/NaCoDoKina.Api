using ApplicationCore.Results;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    /// <summary>
    /// Retrieves all possible data from cinema service 
    /// </summary>
    public class GetCinemaDataBuildStep : IBuildStep<Cinema>
    {
        private readonly ISerializationService _serializationService;

        public int Position => 1;

        public string Name => "Fetch data from web service";

        private readonly IWebClient _webClient;
        private readonly IParsableRequestData _cinemasRequestData;

        public GetCinemaDataBuildStep(IWebClient webClient, CinemasRequestData cinemasRequestData, ISerializationService serializationService)
        {
            _serializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
            _cinemasRequestData = cinemasRequestData ?? throw new ArgumentNullException(nameof(cinemasRequestData));
        }

        public async Task<Result<IEnumerable<Cinema>>> BuildMany(IEnumerable<Cinema> entities)
        {
            var result = await _webClient.MakeRequestAsync(_cinemasRequestData);

            if (!result.IsSuccess)
                return Result.Failure<IEnumerable<Cinema>>(result.FailureReason);

            var cinema = ParseServiceResponse(result.Value);
            return Result.Success(cinema);
        }

        private class CinemaModel
        {
            public string Id { get; set; }

            public string GroupId { get; set; }

            public string Link { get; set; }

            public string Address { get; set; }

            public string DisplayName { get; set; }
        }

        private IEnumerable<Cinema> ParseServiceResponse(string responseContent)
        {
            var cinemas = _serializationService.Deserialize<List<CinemaModel>>(responseContent)
                .Select(cinema => new Cinema
                {
                    Name = cinema.DisplayName,
                    Address = cinema.Address,
                    Website = new ResourceLink(cinema.Link),
                    ExternalId = cinema.Id,
                });

            return cinemas;
        }
    }
}