using Infrastructure.Extensions;
using Infrastructure.Models.Travel;
using System;

namespace Infrastructure.Services.Travel
{
    /// <summary>
    /// Calculates distance estimation based on Haversine formula (distance in 'straight' line)
    ///
    /// Haversine formula: a = sin²(Δφ/2) + cos φ1 ⋅ cos φ2 ⋅ sin²(Δλ/2) c = 2 ⋅ atan2( √a, √(1−a) )
    /// d = R ⋅c where φ is latitude, λ is longitude, R is earth’s radius (mean radius = 6,371km);
    /// note that angles need to be in radians to pass to trig functions! <see cref="http://www.movable-type.co.uk/scripts/latlong.html"/><see cref="http://www.movable-type.co.uk/scripts/latlong.html"/>
    /// </summary>
    public class HaversineTravelInformationEstimator : ITravelInformationEstimator
    {
        private const double MetersPerHour = 50e3;

        private const double EarthRadius = 6371e3;

        private double CalculateDistance(TravelPlan travelPlan)
        {
            var origin = travelPlan.Origin;
            var dest = travelPlan.Destination;

            var deltaLat = dest.Latitude - origin.Latitude;
            var deltaLong = dest.Longitude - origin.Longitude;

            var a = Math.Pow(Math.Sin(deltaLat.ToRadians() / 2), 2) +
                Math.Cos(origin.Latitude.ToRadians()) *
                Math.Cos(dest.Latitude.ToRadians()) *
                Math.Pow(Math.Sin(deltaLong.ToRadians() / 2), 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = EarthRadius * c;
            return Math.Round(distance);
        }

        public TravelInformation Estimate(TravelPlan travelPlan)
        {
            var distance = CalculateDistance(travelPlan);

            double FromHoursToSeconds(double hours) => Math.Round(hours * 60 * 60);

            var durationEstimate = FromHoursToSeconds(distance / MetersPerHour);

            return new TravelInformation(travelPlan, distance, TimeSpan.FromSeconds(durationEstimate));
        }
    }
}