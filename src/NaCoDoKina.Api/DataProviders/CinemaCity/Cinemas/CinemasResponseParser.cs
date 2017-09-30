using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Entities.Resources;
using NaCoDoKina.Api.Services;
using System;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas
{
    public class CinemasResponseParser : IResponseParser<Cinema>
    {
        private readonly ISerializationService _serializationService;

        public CinemasResponseParser(ISerializationService serializationService)
        {
            _serializationService = serializationService ?? throw new ArgumentNullException(nameof(serializationService));
        }

        private class CinemaModel
        {
            public string Id { get; set; }

            public string GroupId { get; set; }

            public string Link { get; set; }

            public string Address { get; set; }

            public string DisplayName { get; set; }
        }

        public Cinema Parse(string responseContent)
        {
            var internalModel = _serializationService.Deserialize<CinemaModel>(responseContent);

            return new Cinema
            {
                Name = internalModel.DisplayName,
                Address = internalModel.Address,
                Website = new ResourceLink(internalModel.Link)
            };
        }
    }
}