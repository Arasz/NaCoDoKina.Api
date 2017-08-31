using NaCoDoKina.Api.Entities;
using NaCoDoKina.Api.Models;
using System;
using System.Threading.Tasks;

namespace NaCoDoKina.Api.Services
{
    /// <summary>
    /// Contract for location oriented business logic 
    /// </summary>
    public interface ILocationService
    {
        /// <summary>
        /// Calculates travel time 
        /// </summary>
        /// <param name="travelPlan"> All data needed for calculation </param>
        /// <returns> Time needed to reach point B from point A </returns>
        Task<TimeSpan> CalculateTravelTimeAsync(TravelPlan travelPlan);

        /// <summary>
        /// Translates address to location (geocoding) 
        /// </summary>
        /// <param name="address"> String with address </param>
        /// <returns> Longitude and latitude for given address </returns>
        Task<Location> TranslateAddressToLocationAsync(string address);
    }
}