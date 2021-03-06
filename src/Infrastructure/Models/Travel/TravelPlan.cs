﻿namespace Infrastructure.Models.Travel
{
    /// <summary>
    /// All data needed for travel time calculations 
    /// </summary>
    public class TravelPlan
    {
        public Location Origin { get; }

        public Location Destination { get; }

        public TravelMode TravelMode { get; }

        public TravelPlan(Location origin, Location destination, TravelMode travelMode = TravelMode.Driving)
        {
            Origin = origin;
            Destination = destination;
            TravelMode = travelMode;
        }
    }
}