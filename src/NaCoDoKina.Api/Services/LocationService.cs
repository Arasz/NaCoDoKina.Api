using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Infrastructure.Google.Services;
using NaCoDoKina.Api.Models;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class LocationService : ILocationService
    {
        private readonly IDirectionsService _directionsService;
        private readonly IGeocodingService _geocodingService;

        public LocationService(IDirectionsService directionsService, IGeocodingService geocodingService)
        {
            _directionsService = directionsService;
            _geocodingService = geocodingService;
        }

        public Task<TimeSpan> CalculateTravelTimeAsync(TravelPlan travelPlan)
        {
            throw new NotImplementedException();
        }

        public Task<Location> TranslateAddressToLocationAsync(string address)
        {
            throw new NotImplementedException();
        }
    }
}