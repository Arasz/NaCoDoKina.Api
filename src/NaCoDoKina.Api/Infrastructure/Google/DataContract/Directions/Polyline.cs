using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions
{
    [DataContract]
    public class Polyline
    {
        [DataMember(Name = "points")]
        public string Points { get; set; }
    }
}