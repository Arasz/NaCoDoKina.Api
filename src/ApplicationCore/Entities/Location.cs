﻿using System.Globalization;

namespace ApplicationCore.Entities
{
    public class Location
    {
        public Location()
        {
        }

        public Location(double latitude, double longitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public void Deconstruct(out double latitude, out double longitude)
        {
            longitude = Longitude;
            latitude = Latitude;
        }

        public override string ToString() => $"{Latitude.ToString(CultureInfo.InvariantCulture)}," +
                                             $"{Longitude.ToString(CultureInfo.InvariantCulture)}";
    }
}