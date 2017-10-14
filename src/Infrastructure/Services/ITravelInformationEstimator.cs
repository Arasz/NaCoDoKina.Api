using Infrastructure.Models.Travel;

namespace Infrastructure.Services
{
    public interface ITravelInformationEstimator
    {
        /// <summary>
        /// Estimates travel distance and duration from travel plan 
        /// </summary>
        /// <param name="travelPlan"></param>
        /// <returns> Estimation </returns>
        TravelInformation Estimate(TravelPlan travelPlan);
    }
}