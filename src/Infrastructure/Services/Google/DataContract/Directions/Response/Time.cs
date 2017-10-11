using System;
using System.Runtime.Serialization;

namespace Infrastructure.Services.Google.DataContract.Directions.Response
{
    [DataContract]
    public class Time
    {
        [DataMember(Name = "value")]
        public DateTime Value { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "time_zone")]
        public string TimeZone { get; set; }
    }
}