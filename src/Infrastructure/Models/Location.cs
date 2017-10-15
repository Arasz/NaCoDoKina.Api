using System;
using System.Globalization;

namespace Infrastructure.Models
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

        public Location LowerPrecision(int places)
        {
            double Truncate(double number)
            {
                var multiplicand = Math.Pow(10, places);

                var tmp = Math.Truncate(number * multiplicand);

                return tmp / multiplicand;
            }

            return new Location(Truncate(Latitude), Truncate(Longitude));
        }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public void Deconstruct(out double longitude, out double latitude)
        {
            longitude = Longitude;
            latitude = Latitude;
        }

        public override string ToString() => $"{Longitude.ToString(CultureInfo.InvariantCulture)}," +
                                             $"{Latitude.ToString(CultureInfo.InvariantCulture)}";
    }
}