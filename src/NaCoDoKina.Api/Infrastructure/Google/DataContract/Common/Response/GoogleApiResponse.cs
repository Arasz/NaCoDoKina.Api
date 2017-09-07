using System.Runtime.Serialization;

namespace NaCoDoKina.Api.Infrastructure.Google.DataContract.Common.Response
{
    [DataContract]
    public class GoogleApiResponse
    {
        [DataMember(Name = "status", IsRequired = true)]
        public string Status { get; set; }

        [DataMember(Name = "error_message")]
        public string ErrorMessage { get; set; }
    }
}