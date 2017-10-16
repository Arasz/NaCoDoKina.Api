using System.Runtime.Serialization;

namespace Infrastructure.Services.Google.DataContract.Directions.Response
{
    [DataContract]
    public class Polyline
    {
        [DataMember(Name = "points")]
        public string Points { get; set; }
    }
}