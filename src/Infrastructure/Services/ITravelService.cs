using System.Threading.Tasks;
using Infrastructure.Models;
using Infrastructure.Models.Travel;

namespace Infrastructure.Services
{
    /// <summary>
    /// Contract for location oriented business logic 
    /// </summary>
    public interface ITravelService
    {
        /// <summary>
        /// Calculates travel time 
        /// </summary>
        /// <param name="travelPlan"> All data needed for calculation </param>
        /// <returns>
        /// Time needed to reach point B from point A. Returns min value when time couldnt be calculated
        /// </returns>
        Task<TravelInformation> CalculateInformationForTravelAsync(TravelPlan travelPlan);

        /// <summary>
        /// Translates address to location (geocoding) 
        /// </summary>
        /// <param name="address"> String with address </param>
        /// <returns> Longitude and latitude for given address </returns>
        Task<Location> TranslateAddressToLocationAsync(string address);
    }
}