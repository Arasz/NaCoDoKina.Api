namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Common
{
    public class Location
    {
        public Location()
        {
        }

        public Location(double lat, double lng)
        {
            Lat = lat;
            Lng = lng;
        }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}