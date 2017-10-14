using Infrastructure.Models.Travel;
using System;

namespace Infrastructure.Services
{
    public class EuclideanTravelInformationEstimator : ITravelInformationEstimator
    {
        private const double MetersPerHour = 50000;

        private static double CalculateEuclideanDistance(TravelPlan travelPlan)
        {
            var distance = Math.Pow((travelPlan.Origin.Latitude - travelPlan.Destination.Latitude), 2)
                           + Math.Pow((travelPlan.Origin.Longitude - travelPlan.Destination.Longitude), 2);
            return Math.Sqrt(distance);
        }

        public TravelInformation Estimate(TravelPlan travelPlan)
        {
            var distance = CalculateEuclideanDistance(travelPlan);

            double FromHoursToSeconds(double hours) => Math.Round(hours * 60 * 60);

            var durationEstimate = FromHoursToSeconds(distance / MetersPerHour);

            return new TravelInformation(travelPlan, distance, TimeSpan.FromSeconds(durationEstimate));
        }
    }
}