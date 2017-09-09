using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Models
{
    /// <summary>
    /// All data needed for travel time calculations 
    /// </summary>
    public class TravelPlan
    {
        public Location Origin { get; }

        public Location Destination { get; }

        public MeansOfTransport MeansOfTransport { get; }

        public TravelPlan(Location origin, Location destination, MeansOfTransport meansOfTransport = MeansOfTransport.Public)
        {
            Origin = origin;
            Destination = destination;
            MeansOfTransport = meansOfTransport;
        }
    }
}