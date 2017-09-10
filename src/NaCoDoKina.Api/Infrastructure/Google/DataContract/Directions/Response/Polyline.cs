using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response
{
    [DataContract]
    public class Polyline
    {
        [DataMember(Name = "points")]
        public string Points { get; set; }
    }
}