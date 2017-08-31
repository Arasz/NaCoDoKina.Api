using NaCoDoKina.Api.Entities;

namespace NaCoDoKina.Api.Models
{
    /// <summary>
    /// All data needed for travel time calculations 
    /// </summary>
    public class TravelPlan
    {
        public Location LocationA { get; }

        public Location LocationB { get; }

        public MeansOfTransport MeansOfTransport { get; }

        public TravelPlan(Location locationA, Location locationB, MeansOfTransport meansOfTransport = MeansOfTransport.Public)
        {
            LocationA = locationA;
            LocationB = locationB;
            MeansOfTransport = meansOfTransport;
        }
    }
}