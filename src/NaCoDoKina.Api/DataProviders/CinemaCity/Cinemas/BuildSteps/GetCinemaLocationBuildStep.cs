using ApplicationCore.Results;
using AutoMapper;
using NaCoDoKina.Api.DataProviders.EntityBuilder.BuildSteps;
using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Entities.Cinemas;
using NaCoDoKina.Api.Services;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    public class GetCinemaLocationBuildStep : IBuildStep<Cinema>
    {
        private readonly IMapper _mapper;
        private readonly ITravelService _travelService;
        public string Name => "Get cinema location for address";

        public int Position => 3;

        public async Task<Result<Cinema[]>> BuildMany(Cinema[] entities)
        {
            foreach (var cinema in entities)
            {
                var location = await _travelService.TranslateAddressToLocationAsync(cinema.Address);

                if (location is null)
                    return Result.Failure<Cinema[]>($"Location not found for cinema {cinema}");

                cinema.Location = _mapper.Map<Location>(location);
            }

            return Result.Success(entities);
        }

        public GetCinemaLocationBuildStep(ITravelService travelService, IMapper mapper)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _travelService = travelService ?? throw new ArgumentNullException(nameof(travelService));
        }
    }
}