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

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public void Deconstruct(out double latitude, out double longitude)
        {
            longitude = Longitude;
            latitude = Latitude;
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

        public override string ToString() => $"{Latitude.ToString(CultureInfo.InvariantCulture)}," +
                                             $"{Longitude.ToString(CultureInfo.InvariantCulture)}";

        protected bool Equals(Location other)
        {
            return Longitude.Equals(other.Longitude) && Latitude.Equals(other.Latitude);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals((Location)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Longitude.GetHashCode() * 397) ^ Latitude.GetHashCode();
            }
        }
    }
}