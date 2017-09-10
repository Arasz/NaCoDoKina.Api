using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Directions.Response
{
    [DataContract]
    public class TextValue
    {
        [DataMember(Name = "value")]
        public double Value { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}