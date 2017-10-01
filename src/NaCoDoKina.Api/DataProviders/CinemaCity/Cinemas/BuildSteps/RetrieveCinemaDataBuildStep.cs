using ApplicationCore.Results;
using NaCoDoKina.Api.DataProviders.EntityBuilder;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Services;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas
{
    /// <summary>
    /// Retrieves all possible data from cinema service 
    /// </summary>
    public class RetrieveCinemaDataBuildStep : IBuildStep<Cinema>
    {
        private readonly ISerializationService _serializationService;

        public int Position => 1;

        public string Name => "Fetch data from web service";

        private readonly IWebClient _webClient;
        private readonly IParsableRequestData _cinemasRequestData;

        public RetrieveCinemaDataBuildStep(IWebClient webClient, IParsableRequestData cinemasRequestData, ISerializationService serializationService)
        {
            _serializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
            _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
            _cinemasRequestData = cinemasRequestData ?? throw new ArgumentNullException(nameof(cinemasRequestData));
        }

        public async Task<Result<Cinema>> Build(Cinema entity)
        {
            var result = await _webClient.MakeRequestAsync(_cinemasRequestData);

            if (!result.IsSuccess)
                return Result.Failure<Cinema>(result.FailureReason);

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

        private Cinema ParseServiceResponse(string responseContent)
        {
            var internalModel = _serializationService.Deserialize<CinemaModel>(responseContent);

            return new Cinema
            {
                Name = internalModel.DisplayName,
                Address = internalModel.Address,
                Website = new ResourceLink(internalModel.Link),
                ExternalId = internalModel.Id,
            };
        }
    }
}