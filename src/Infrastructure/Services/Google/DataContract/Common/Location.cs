using System.Globalization;
using System.Runtime.Serialization;

namespace Infrastructure.Services.Google.DataContract.Common
{
    [DataContract]
    public class Location
    {
        public Location(double latitude, double longitude)
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        public Location()
        {
        }

        [DataMember(Name = "lng")]
        public double Longitude { get; set; }

        [DataMember(Name = "lat")]
        public double Latitude { get; set; }

        public void Deconstruct(out double longitude, out double latitude)
        {
            longitude = Longitude;
            latitude = Latitude;
        }

        public override string ToString() => $"{Latitude.ToString(CultureInfo.InvariantCulture)}," +
                                             $"{Longitude.ToString(CultureInfo.InvariantCulture)}";
    }
}