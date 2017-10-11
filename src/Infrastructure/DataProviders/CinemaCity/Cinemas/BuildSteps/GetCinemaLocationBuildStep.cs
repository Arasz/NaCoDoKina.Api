using ApplicationCore.Entities;
using ApplicationCore.Entities.Cinemas;
using ApplicationCore.Results;
using Infrastructure.DataProviders.EntityBuilder.BuildSteps;
using Infrastructure.DataProviders.EntityBuilder.Context;
using Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace Infrastructure.DataProviders.CinemaCity.Cinemas.BuildSteps
{
    public class GetCinemaLocationBuildStep : IBuildStep<Cinema, EmptyContext>
    {
        private readonly ITravelService _travelService;
        public string Name => "Get cinema location for address";

        public int Position => 3;

        public bool Enabled => true;

        public async Task<Result<Cinema[]>> BuildMany(Cinema[] entities, EmptyContext context)
        {
            foreach (var cinema in entities)
            {
                var location = await _travelService.TranslateAddressToLocationAsync(cinema.Address);

                if (location is null)
                    return Result.Failure<Cinema[]>($"Location not found for cinema {cinema}");

                cinema.Location = new Location(location.Longitude, location.Latitude);
            }

            return Result.Success(entities);
        }

        public GetCinemaLocationBuildStep(ITravelService travelService)
        {
            _travelService = travelService ?? throw new ArgumentNullException(nameof(travelService));
        }
    }
}