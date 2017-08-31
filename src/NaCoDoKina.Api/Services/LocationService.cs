using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Models;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    public class LocationService : ILocationService
    {
        public LocationService()
        {
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