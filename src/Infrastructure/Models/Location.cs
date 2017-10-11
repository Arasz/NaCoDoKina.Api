using System.Globalization;

namespace Infrastructure.Models
{
    public class Location
    {
        public Location()
        {
        }

        public Location(double longitude, double latitude)
        {
            Longitude = longitude;
            Latitude = latitude;
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